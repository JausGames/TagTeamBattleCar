using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource motorAudioSource;
    [SerializeField] AnimationCurve speedToPitchCurve;
    [SerializeField] float pitchClamp = 20f;
    float lastSpeed;
    [SerializeField] float shutMotorClamp = 2f;
    float maxSpeed = 60f;
    float currentPitch = 1f;
    float maxVolume;

    private void Awake()
    {
        maxVolume = motorAudioSource.volume;
    }
    public void SetCarEnginePitch(float speed, float torque)
    {

        currentPitch = Mathf.MoveTowards(currentPitch, speedToPitchCurve.Evaluate(speed / maxSpeed), pitchClamp * Time.deltaTime);

        if (torque == 0f) motorAudioSource.volume = Mathf.MoveTowards(motorAudioSource.volume, 0f, shutMotorClamp * Time.deltaTime);
        else if (speed > 0f) motorAudioSource.volume = Mathf.MoveTowards(motorAudioSource.volume, maxVolume, shutMotorClamp * Time.deltaTime);


        motorAudioSource.pitch = currentPitch;
        lastSpeed = speed;
    }
    public void SetMaxSpeed(float maxSpeed)
    {
        this.maxSpeed = maxSpeed;
    }
}
