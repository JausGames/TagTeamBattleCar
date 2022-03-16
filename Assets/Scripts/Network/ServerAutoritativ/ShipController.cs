using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace ServerAutoritative
{

public class ShipController : NetworkBehaviour
{
    [Header("Inputs")]
    [SerializeField] Vector2 previousRotation;
    [SerializeField] NetworkVariable<Vector2> rotation = new NetworkVariable<Vector2>(NetworkVariableReadPermission.Everyone);
    [SerializeField] NetworkVariable<float> torqueApply = new NetworkVariable<float>(NetworkVariableReadPermission.Everyone);
    [SerializeField] NetworkVariable<bool> parkingBreaking = new NetworkVariable<bool>(NetworkVariableReadPermission.Everyone);
    [SerializeField] NetworkVariable<bool> isRotationAxeY = new NetworkVariable<bool>(NetworkVariableReadPermission.Everyone);

    [Space]
    [Header("Network value")]
    //[SerializeField] NetworkVariable<Vector3> force = new NetworkVariable<Vector3>(NetworkVariableReadPermission.Everyone);
    //[SerializeField] NetworkVariable<Vector3> torque = new NetworkVariable<Vector3>(NetworkVariableReadPermission.Everyone);
    //[SerializeField] NetworkVariable<bool> resetSteer = new NetworkVariable<bool>(NetworkVariableReadPermission.Everyone);
    [SerializeField] Vector3 oldForce = Vector3.zero;
    [SerializeField] Vector3 oldTorque = Vector3.zero;
    [SerializeField] bool oldResetSteer = false;

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
    [SerializeField] Light[] boostLights;
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
    }

    #region Network var : Rotation
    public void SetRotation(Vector3 rotation)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            ChangeRotation(rotation);
        }
        else
        {
            SubmitChangeRotationServerRpc(rotation);
        }
    }
    [ServerRpc]
    void SubmitChangeRotationServerRpc(Vector2 rotation, ServerRpcParams rpcParams = default)
    {
        ChangeRotation(rotation);
    }
    void ChangeRotation(Vector2 rotation)
    {
        this.rotation.Value = rotation;
    }
    #endregion
    #region Network var : Motor Torque
    public void SetMotorTorque(float torque)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            ChangeTorque(torque);
        }
        else
        {
            SubmitChangeTorqueServerRpc(torque);
        }
    }
    [ServerRpc]
    void SubmitChangeTorqueServerRpc(float torque, ServerRpcParams rpcParams = default)
    {
        ChangeTorque(torque);
    }
    void ChangeTorque(float torque)
    {
        this.torqueApply.Value = torque;
    }
    #endregion
    #region Network var : Parking break

    public void SetParkingBreak(bool breaking)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            ChangeParkingBreak(breaking);
        }
        else
        {
            SubmitChangeParkingBreakServerRpc(breaking);
        }
    }
    [ServerRpc]
    void SubmitChangeParkingBreakServerRpc(bool breaking, ServerRpcParams rpcParams = default)
    {
        ChangeParkingBreak(breaking);
    }
    void ChangeParkingBreak(bool breaking)
    {
        this.parkingBreaking.Value = breaking;
    }
    #endregion
    #region Network var : Rotation axe Y
    public void SetRotationAxeY(bool isY)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            ChangeRotationAxe(isY);
        }
        else
        {
            SubmitChangeRotationAxeServerRpc(isY);
        }
    }
    [ServerRpc]
    void SubmitChangeRotationAxeServerRpc(bool isYAxes, ServerRpcParams rpcParams = default)
    {
        ChangeRotationAxe(isYAxes);
    }
    void ChangeRotationAxe(bool isYAxes)
    {
        this.isRotationAxeY.Value = isYAxes;
    }
    #endregion

    #region Update : Server & Client
    // Update is called once per frame
    void Update()
    {

        Vector3[] localMatrix;
        Vector3 localVelocity, localAngularVelocity;
        GetSpeedConversions(out localMatrix, out localVelocity, out localAngularVelocity);

        //if (IsClient && IsLocalPlayer) UpdateClient(localMatrix, localVelocity, localAngularVelocity);

        if (IsServer)
        {
            UpdateClient(localMatrix, localVelocity, localAngularVelocity);
            UpdateServer(localVelocity, localAngularVelocity, torqueApply.Value > 0);
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
            torque += CheckAerialRotations(isRotationAxeY.Value, rotation.Value);
            force += ApplyGravity();
        }

        /*if (oldForce != force || oldTorque != torque || oldResetSteer != resetSteer)
        {
            this.force.Value = force;
            this.torque.Value = torque;
            this.resetSteer.Value = resetSteer;
            //RequestUpdateServerRpc(force, torque, resetSteer, localAngularVelocity);
            oldForce = force;
            oldTorque = torque;
            oldResetSteer = resetSteer;
        }*/

        body.AddForce(force * Time.deltaTime, ForceMode.Acceleration);
        body.AddTorque(torque * Time.deltaTime, ForceMode.Acceleration);
        ForceForwardDirection(parkingBreaking.Value, localVelocity);
        if (resetSteer) ResetSteeringWheel(localAngularVelocity);

        //Effects here
        PlayShipSvfxClientRpc(torqueApply.Value > 0, localVelocity.z);

    }
    
    [ClientRpc]
    void PlayShipSvfxClientRpc(bool isPlayed, float speed)
    {
        PlayBoostVfx(isPlayed);
        if (isPlayed) SetMotorPitch(speed);
        else SetMotorPitch(0f);
    }
    /*[ServerRpc]
    void RequestUpdateServerRpc(Vector3 force, Vector3 torque, bool resetSteer, Vector3 localAngularVelocity)
    {
        this.force.Value = force;
        this.torque.Value = torque;
        this.resetSteer.Value = resetSteer;
        if (resetSteer) ResetSteeringWheel(localAngularVelocity);
    }*/

    private void UpdateServer(Vector3 localVelocity, Vector3 localAngularVelocity, bool engineOn)
    {
        /*body.AddForce(force.Value * Time.deltaTime, ForceMode.Acceleration);
        body.AddTorque(torque.Value * Time.deltaTime, ForceMode.Acceleration);
        ForceForwardDirection(parkingBreaking.Value, localVelocity);
        if(resetSteer.Value) ResetSteeringWheel(localAngularVelocity);

        //Effects here
        PlayBoostVfx(engineOn);
        if (engineOn) SetMotorPitch(localVelocity.z);
        else SetMotorPitch(0f);*/
    }

    private void ResetSteeringWheel(Vector3 localAngularVelocity)
    {
        body.angularVelocity = localAngularVelocity.x * Vector3.right + localAngularVelocity.z * Vector3.up;
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

    protected Vector3 ApplySteeringRotation(Vector3 rotation, Vector3 localAngularVelocity, out bool resetSteer)
    {
        //Rotate the ship
        resetSteer = false;
        var torque = Vector3.zero;
        var steerRatio = carSettings.steerCurve.Evaluate(body.velocity.magnitude / carSettings.maxSpeed);
        if (Mathf.Abs(rotation.x) > 0.1f)
        {
            if (Mathf.Sign(previousRotation.x) != Mathf.Sign(rotation.x)) resetSteer = true;
            torque = rotation.x * carSettings.rotationSpeed * steerRatio * transform.up;
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
            //localVelocity.z += carSettings.driftSpeedBoostRatio * Mathf.Sign(localVelocity.z) * Mathf.Abs(localVelocity.x - localVelocity.x * breakRatio);
            Debug.Log("ShipController, ForceForwardDirection : Velocity : " + transform.TransformDirection(localVelocity));
            Debug.Log("ShipController, ForceForwardDirection : Clamped to : " + body.velocity);
            //body.AddForce((transform.TransformDirection(localVelocity) - body.velocity) * Time.deltaTime * 4000f, ForceMode.Acceleration);
            body.velocity = Vector3.Lerp(body.velocity, transform.TransformDirection(localVelocity), carSettings.driftSpeedBoostRatio * Time.deltaTime);
        }
    }

    protected Vector3 CheckAerialRotations(bool rotationAxeIsY, Vector3 rotation)
    {

        //Not on floor : can rotate multiple axes
        var aerialRot = Vector3.zero;
        var rotationAxe = transform.up;
        if (!rotationAxeIsY) rotationAxe = -transform.forward;
        if (Mathf.Abs(rotation.magnitude) > 0.1f) aerialRot = rotation.x * carSettings.rotationSpeed * rotationAxe
                                                            + rotation.y * carSettings.rotationSpeed * transform.right;
        SetMotorPitch(0f);
        return aerialRot;
    }
    protected Vector3 ApplyGravity()
    {
        //add to base gravity
        return Vector3.down * gravityModifier;

    }
    #endregion

    #region SFX & VFX

    void PlayBoostVfx(bool isPlayed)
    {
        if (isPlayed)
        {
            foreach (ParticleSystem prtc in boostParticles)
            {
                if (!prtc.isPlaying)
                    prtc.Play();
            }
            foreach (Light light in boostLights)
            {
                if (!light.enabled)
                    light.enabled = true;
            }
        }
        else
        {
            foreach (ParticleSystem prtc in boostParticles)
            {
                if (!prtc.isStopped)
                    prtc.Stop();
            }
            foreach (Light light in boostLights)
            {
                if (light.enabled)
                    light.enabled = false;
            }
        }
    }
    private void SetMotorPitch(float speed)
    {
        soundManager.SetCarEnginePitch(speed);
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
