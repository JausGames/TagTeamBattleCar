using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Inputs")] 
    [SerializeField] float steeringDirection;
    [SerializeField] float torqueApply;

    [Space]
    [Header("Car stats")]
    [SerializeField] float rotationSpeed = 125f;
    [SerializeField] float torqueForce = 15f;
    [SerializeField] AnimationCurve accelerationCurve;

    [Space]
    [Header("Components")]
    [SerializeField] Rigidbody body;

    public float SteeringDirection { get => steeringDirection; set => steeringDirection = value; }
    public float TorqueApply { get => torqueApply; set => torqueApply = value; }


    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(steeringDirection) > 0.1f) body.MoveRotation(body.rotation * Quaternion.Euler(steeringDirection * Time.deltaTime * rotationSpeed * transform.up));
        if (Mathf.Abs(torqueApply) > 0.1f) body.AddForce(torqueApply * torqueForce * transform.forward * Time.deltaTime, ForceMode.Acceleration);
    }
}
