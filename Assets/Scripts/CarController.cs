using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] Vector2 rotation;
    [SerializeField] Vector2 previousRotation;
    [SerializeField] float torqueApply;
    [SerializeField] bool parkingBreaking;
    [SerializeField] bool rotationAxeIsY = true;

    [Space]
    [Header("Car stats")]
    [SerializeField] float rotationSpeed = 125f;
    [SerializeField] float torqueForce = 500f;
    [SerializeField] float breakForce = 50f;
    [SerializeField] AnimationCurve accelerationCurve;
    [SerializeField] AnimationCurve autobreakCurve;
    [SerializeField] AnimationCurve steerCurve;
    [SerializeField] bool onFloor;

    [Space]
    [Header("Components")]
    [SerializeField] Rigidbody body;
    [SerializeField] Transform centerOfMass;
    [SerializeField] ParticleSystem[] boostParticles;

    [Space]
    [Header("Debug")]
    [SerializeField] Vector3 vectorForward;
    [SerializeField] Vector3 vectorRight;
    [SerializeField] Vector3 vectorUp;

    private void Start()
    {
        body.centerOfMass = centerOfMass.localPosition; 
    }

    public Vector2 Rotation { get => rotation; set => rotation = value; }
    public float TorqueApply { get => torqueApply; set => torqueApply = value; }
    public bool RotationAxeIsY { get => rotationAxeIsY; set => rotationAxeIsY = value; }
    public bool ParkingBreaking { get => parkingBreaking; set => parkingBreaking = value; }


    // Update is called once per frame
    void Update()
    {
        /*if(isPhysicBased)
        {
            RaycastHit[] hits = new RaycastHit[4];

            for (int i = 0; i < corners.Length; i++)
            {
                ApplyForce(corners[i], hits[i]);
                //array[i] = GetHitDistance(corners[i], hits[i]);
            }
        }*/


            if (onFloor)
            {
                vectorForward = Vector3.Dot(transform.forward, Vector3.forward) * Vector3.forward;
                vectorRight = Vector3.Dot(transform.forward, Vector3.right) * Vector3.right;
                vectorUp = Vector3.Dot(transform.forward, Vector3.up) * Vector3.up;
                Debug.Log("CarController, Update : Speed = " + body.velocity.magnitude);

            //Rotate the ship

            var steerRatio = steerCurve.Evaluate(body.velocity.magnitude / 65f);

            if (Mathf.Abs(rotation.x) > 0.1f)
            {
                if(Mathf.Sign(previousRotation.x) != Mathf.Sign(rotation.x)) body.angularVelocity = body.angularVelocity.x * Vector3.right + body.angularVelocity.z * Vector3.up;
                body.AddTorque(rotation.x * Time.deltaTime * rotationSpeed * steerRatio * transform.up, ForceMode.Acceleration);
            }
            else body.angularVelocity = body.angularVelocity.x * Vector3.right + body.angularVelocity.z * Vector3.up;
            previousRotation = rotation;

            //Check if add force to accelerate or autobreak
            if (Mathf.Abs(torqueApply) > 0.1f)
                {
                    var accelerationRatio = accelerationCurve.Evaluate(body.velocity.magnitude / 65f);

                    //var forwardRatio = Vector3.Dot((body.velocity.x * Vector3.right + body.velocity.y * Vector3.up + body.velocity.z * Vector3.forward).normalized, (vectorForward + vectorRight + vectorUp).normalized);
                    //var breakSpeedRatio = inverseVelocityBreakCurve.Evaluate(forwardRatio);

                    //Debug.Log("CarController, Update : forward Ratio = " + forwardRatio);
                    //Debug.Log("CarController, Update : breakSpeedRatio = " + breakSpeedRatio);
                    /*body.AddForce(accelerationRation * torqueApply * torqueForce * Mathf.Pow(2f - forwardRatio, 2f) * Time.deltaTime * (vectorForward + vectorRight + vectorUp)
                        + torqueInverseVelocityBreak * breakSpeedRatio * -body.velocity * Time.deltaTime, 
                        ForceMode.Acceleration);*/
                    body.AddForce(accelerationRatio * torqueApply * torqueForce * Time.deltaTime * (vectorForward + vectorRight + vectorUp));
                }
                else
                {
                    //Auto break if not adding torque
                    var breakRatio = autobreakCurve.Evaluate(body.velocity.magnitude / 30f);
                    var locVel = transform.InverseTransformDirection(body.velocity);
                    body.AddForce((-locVel.x * transform.right - locVel.z * transform.forward) * breakRatio * breakForce * Time.deltaTime);
                }

                if (!parkingBreaking)
                {
                    //moslty have forward 
                    // issues : when parking break is hold and release, speed isn't really continue
                    Vector3 localVelocity = transform.InverseTransformDirection(body.velocity);
                    localVelocity.x /= 20f;
                    body.velocity = transform.TransformDirection(localVelocity);
                }
            }
            else
            {
                //Not on floor : can rotate multiple axes
                var rotationAxe = transform.up;
                if (!rotationAxeIsY) rotationAxe = -transform.forward;
                if (Mathf.Abs(rotation.magnitude) > 0.1f) body.AddTorque(rotation.x * Time.deltaTime * rotationSpeed * rotationAxe
                                                                    + rotation.y * Time.deltaTime * rotationSpeed * transform.right);
            }

            //add to base gravity
            body.AddForce(Vector3.down * 10f * Time.deltaTime);

            //Play boost particle
            if (torqueApply > 0f)
            {
                foreach (ParticleSystem prtc in boostParticles)
                {
                    if (!prtc.isPlaying)
                        prtc.Play();
                }
            }
            else
            {
                foreach (ParticleSystem prtc in boostParticles)
                {
                    if (!prtc.isStopped)
                        prtc.Stop();
                }
            }

        


    }

    /*void ApplyForce(Transform anchor, RaycastHit hit)
    {
        if(Physics.Raycast(anchor.position, -anchor.up, out hit, hoverMask))
        {
            float force = 0;
            force = Mathf.Abs(1f / Mathf.Pow(hit.point.y - anchor.position.y, 2f));
            //if (Vector3.Dot(hit.normal, Vector3.up) < 0.95f) force = 0f;
            force = force * Vector3.Dot(hit.normal, Vector3.up);
            Debug.Log("CarController, ApplyForce : force = " + force);
            body.AddForceAtPosition(transform.up * Mathf.Min(force, 0.8f) * hoverForce, anchor.position, ForceMode.Acceleration);
        }
    }*/
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + body.velocity.normalized * 10f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (vectorForward + vectorRight + vectorUp).normalized * 10f);
    }
}
