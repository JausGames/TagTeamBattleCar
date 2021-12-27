using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class CarSettings : ScriptableObject
{
    [Header("Speed")]
    public float maxSpeed = 60f;
    public float torqueForce = 500f;
    public AnimationCurve accelerationCurve;
    [Space]
    [Header("Steer")]
    public float rotationSpeed = 125f;
    public AnimationCurve steerCurve;
    [Space]
    [Header("Break")]
    public float breakForce = 50f;
    public AnimationCurve autobreakCurve;
    [Space]
    [Header("Drift")]
    public float driftSpeedBoostRatio = 0.2f;
    public AnimationCurve stopDriftingCurve;

}
