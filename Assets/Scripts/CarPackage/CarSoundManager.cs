using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundManager : MonoBehaviour
{

    [SerializeField] List<Sound> sounds;
    [SerializeField] List<AudioSource> sources;

    float maxSpeed = 60f;

    private void Awake()
    {
        for(int i = 0; i < sounds.Count; i++)
        {
            sources.Add(gameObject.AddComponent<AudioSource>());
            sources[i].Stop();
            sources[i].loop = true;
            sources[i].clip = sounds[i].Clip;
            sources[i].minDistance = 8f;
            sources[i].maxDistance = 100f;
            sources[i].spatialBlend = 1f;
            sources[i].Play();
        }
    }
    public void SetCarEnginePitch(float speed, float torque)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            sources[i].pitch = Mathf.MoveTowards(sources[i].pitch, sounds[i].SpeedToPitchCurve.Evaluate(speed / maxSpeed), sounds[i].ClampSpeed * Time.deltaTime);
            sources[i].volume = Mathf.MoveTowards(sources[i].volume, sounds[i].SpeedToVolumeCurve.Evaluate(speed / maxSpeed), sounds[i].ClampSpeed * Time.deltaTime);
        }
    }
    public void SetMaxSpeed(float maxSpeed)
    {
        this.maxSpeed = maxSpeed;
    }
    [System.Serializable]
    class Sound
    {
        [SerializeField] AudioClip clip;
        [SerializeField] AnimationCurve speedToPitchCurve;
        [SerializeField] AnimationCurve speedToVolumeCurve;
        [SerializeField] float maxVolume;
        [SerializeField] float clampSpeed;

        public AudioClip Clip { get => clip; set => clip = value; }
        public float MaxVolume { get => maxVolume; set => maxVolume = value; }
        public AnimationCurve SpeedToPitchCurve { get => speedToPitchCurve; set => speedToPitchCurve = value; }
        public AnimationCurve SpeedToVolumeCurve { get => speedToVolumeCurve; set => speedToVolumeCurve = value; }
        public float ClampSpeed { get => clampSpeed; set => clampSpeed = value; }
    }
}
