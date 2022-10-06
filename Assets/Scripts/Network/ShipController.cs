using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace ClientAutoritative
{

    public class ShipController : NetworkBehaviour
    {
        [Header("Inputs")]
        [SerializeField] float previousRotation;
        [SerializeField] NetworkVariable<float> rotation = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] NetworkVariable<float> torqueApply = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] NetworkVariable<bool> parkingBreaking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] NetworkVariable<bool> isRotationAxeY = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Space]
        [Header("Car stats")]
        [SerializeField] protected CarSettings carSettings;
        [SerializeField] protected bool onFloor;
        [SerializeField] float gravityModifier = 1000f;

        [Space]
        [Header("Components")]
        [SerializeField] protected Rigidbody body;
        [SerializeField] Transform centerOfMass;
        [SerializeField] ParticleSystem[] boostParticles;
        [SerializeField] CarSoundManager soundManager;

        [Space]
        [Header("Debug")]
        [SerializeField] Vector3 vectorForward;
        [SerializeField] Vector3 vectorRight;
        [SerializeField] Vector3 vectorUp;

        private void Start()
        {
            soundManager.SetMaxSpeed(carSettings.maxSpeed);
            body.centerOfMass = centerOfMass.localPosition;
            if (IsOwner) body.isKinematic = false;
        }

        #region Network var : Rotation
        public void SetRotation(float torque)
        {
            this.rotation.Value = torque;
        }
        #endregion
        #region Network var : Motor Torque
        public void SetMotorTorque(float torque)
        {
            this.torqueApply.Value = torque;
        }
        #endregion
        #region Network var : Parking break

        public void SetParkingBreak(bool breaking)
        {
            this.parkingBreaking.Value = breaking;
        }
        #endregion
        #region Network var : Rotation axe Y
        public void SetRotationAxeY(bool isY)
        {
            this.isRotationAxeY.Value = isY;
        }
        #endregion

        #region Update : Client/Owner
        // Update is called once per frame
        void Update()
        {

            if (IsOwner)
            {
                Vector3[] localMatrix;
                Vector3 localVelocity, localAngularVelocity;
                GetSpeedConversions(out localMatrix, out localVelocity, out localAngularVelocity);
                UpdateClient(localMatrix, localVelocity, localAngularVelocity);
            }

        }
        private void UpdateClient(Vector3[] localMatrix, Vector3 localVelocity, Vector3 localAngularVelocity)
        {
            Vector3 torque = Vector3.zero, force = Vector3.zero;
            bool resetSteer = false;

            if (onFloor)
            {

                torque += ApplySteeringRotation(rotation.Value, localAngularVelocity, out resetSteer);
                force += ApplyMotorAcceleration(torqueApply.Value, localVelocity, localMatrix);
            }
            else
            {
                //torque += CheckAerialRotations(isRotationAxeY.Value, rotation.Value);
                force += ApplyGravity();
            }

            body.AddForce(force * Time.deltaTime, ForceMode.Acceleration);
            //body.velocity += force * Time.deltaTime;
            body.AddTorque(torque * Time.deltaTime, ForceMode.Acceleration);
            ForceForwardDirection(parkingBreaking.Value, localVelocity);
            if (resetSteer) ResetSteeringWheel(localAngularVelocity);

            //Effects here
            PlayShipSvfxServerRpc(localVelocity.z, torqueApply.Value);
            Debug.Log("ShipController, VFXisplayed client ? " + (torqueApply.Value > 0));

        }

        [ServerRpc]
        void PlayShipSvfxServerRpc(float speed, float torque)
        {
            PlayShipSvfxClientRpc(speed, torque);
        }

        [ClientRpc]
        void PlayShipSvfxClientRpc(float speed, float torque)
        {
            PlaySpeedVfx();
            SetMotorPitch(speed > .5f ? speed : 0f, torque);
        }

        #endregion

        #region Driving methods
        protected void GetSpeedConversions(out Vector3[] localMatrix, out Vector3 localVelocity, out Vector3 localAngularVelocity)
        {
            localMatrix = GetLocalMatrix();
            localVelocity = transform.InverseTransformDirection(body.velocity);
            localAngularVelocity = transform.InverseTransformDirection(body.angularVelocity);
        }

        private Vector3[] GetLocalMatrix()
        {
            return new Vector3[] { Vector3.Dot(transform.forward, Vector3.forward) * Vector3.forward, Vector3.Dot(transform.forward, Vector3.right) * Vector3.right, Vector3.Dot(transform.forward, Vector3.up) * Vector3.up };
        }

        protected Vector3 ApplySteeringRotation(float rotation, Vector3 localAngularVelocity, out bool resetSteer)
        {
            //Rotate the ship
            resetSteer = false;
            var torque = Vector3.zero;
            var steerRatio = carSettings.steerCurve.Evaluate(body.velocity.magnitude / carSettings.maxSpeed);
            if (Mathf.Abs(rotation) > 0.1f)
            {
                if (Mathf.Sign(previousRotation) != Mathf.Sign(rotation)) resetSteer = true;
                torque = rotation * carSettings.rotationSpeed * steerRatio * transform.up;
            }
            else resetSteer = true;
            previousRotation = rotation;
            return torque;
        }

        protected Vector3 ApplyMotorAcceleration(float torqueApply, Vector3 localVelocity, Vector3[] localMatrix)
        {
            var force = Vector3.zero;
            //Check if add force to accelerate or autobreak
            if (Mathf.Abs(torqueApply) > 0.1f)
            {
                var accelerationRatio = carSettings.accelerationCurve.Evaluate(body.velocity.magnitude / carSettings.maxSpeed);
               force = accelerationRatio * torqueApply * carSettings.torqueForce * (localMatrix[0] + localMatrix[1] + localMatrix[2]);
            }
            else
            {
                //Auto break if not adding torque
                var breakRatio = carSettings.autobreakCurve.Evaluate(body.velocity.magnitude / (carSettings.maxSpeed * 0.5f));
                force = (-localVelocity.x * transform.right - localVelocity.z * transform.forward) * breakRatio * carSettings.breakForce;
            }
            return force;
        }
        protected void ForceForwardDirection(bool parkingBreaking, Vector3 localVelocity)
        {
            if (!parkingBreaking && onFloor)
            {
                //moslty have forward 
                var driftSpeedRatio = Mathf.Pow(localVelocity.x, 2f) / Mathf.Pow(Mathf.Abs(localVelocity.x) + Mathf.Abs(localVelocity.z), 2f);
                var breakRatio = carSettings.stopDriftingCurve.Evaluate(driftSpeedRatio);
                localVelocity.x *= breakRatio;
                body.velocity = Vector3.MoveTowards(body.velocity, transform.TransformDirection(localVelocity), carSettings.driftSpeedBoostRatio * Time.deltaTime);
            }
        }
        private void ResetSteeringWheel(Vector3 localAngularVelocity)
        {
            body.angularVelocity = localAngularVelocity.x * Vector3.right + localAngularVelocity.z * Vector3.up;
        }

        protected Vector3 CheckAerialRotations(bool rotationAxeIsY, float rotation)
        {

            //Not on floor : can rotate multiple axes
            var aerialRot = Vector3.zero;
            var rotationAxe = transform.up;
            if (!rotationAxeIsY) rotationAxe = -transform.forward;
            /*if (Mathf.Abs(rotation.magnitude) > 0.1f) aerialRot = rotation.x * carSettings.rotationSpeed * rotationAxe
                                                                + rotation.y * carSettings.rotationSpeed * transform.right;*/
            SetMotorPitch(0f, 0f);
            return aerialRot;
        }
        protected Vector3 ApplyGravity()
        {
            //add to base gravity
            return Vector3.down * gravityModifier;

        }
        #endregion

        #region SFX & VFX

        void PlaySpeedVfx()
        {
            var playParticles = false;
            if (body.velocity.magnitude > .5f && onFloor)
                playParticles = true;

            foreach (ParticleSystem prtc in boostParticles)
            {
                var em = prtc.emission;
                em.enabled = playParticles;
            }
        }
        private void SetMotorPitch(float speed, float torque)
        {
            soundManager.SetCarEnginePitch(speed, torque);
        }
        #endregion

        #region Check Floor

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer.Equals(0) && !onFloor)
                onFloor = true;
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer.Equals(0) && !onFloor)
                onFloor = true;

        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer.Equals(0) && onFloor)
                onFloor = false;
        }
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + body.velocity.normalized * 10f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (vectorForward + vectorRight + vectorUp).normalized * 10f);
        }
    }

}
