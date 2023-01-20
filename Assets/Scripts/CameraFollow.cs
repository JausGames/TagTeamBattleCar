using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform targetRig;
    public  float speed = 20f;
    private new Camera camera;
    [SerializeField] Vector3 offset;
    //[SerializeField] Quaternion quaternionOffset = Quaternion.identity;


    Vector3 rotationOffset;
    [SerializeField] float rotationFadeoutDuration;
    [SerializeField] float rotationFadeoutTime;
    [SerializeField] AnimationCurve rotationOffsetCurve;
    private Quaternion desiredRotation;
    [SerializeField]private Transform target;

    private void Start()
    {
        camera = GetComponentInChildren<Camera>();
        desiredRotation = Quaternion.identity;
    }
    public Vector3 RotationOffset { get => rotationOffset;
        set 
        { 
            rotationOffset = value;
            rotationFadeoutTime = Time.time + rotationFadeoutDuration;
        }
    }
    [SerializeField]
    Vector3 CurrentOffset
    {
        get
        {
            var t = rotationOffsetCurve.Evaluate(Mathf.Min(1f, (rotationFadeoutDuration - (rotationFadeoutTime - Time.time)) / rotationFadeoutDuration));
            var curr = rotationOffset * t + Vector3.zero * (1f - t);
            return curr;
        }
    }

    public Camera Camera { get => camera; set => camera = value; }

    public void RotateCamera(Vector3 look, float cameraSpeed, Transform seatTransform, out float angle, out Vector3 target)
    {

        var eulerRot = desiredRotation.eulerAngles + new Vector3(look.normalized.y * Time.deltaTime * cameraSpeed * look.magnitude, look.normalized.x * Time.deltaTime * cameraSpeed * look.magnitude, 0f);
        eulerRot.x = eulerRot.x < 0 ? eulerRot.x + 360f : eulerRot.x;
        var newX = eulerRot.x > 0 && eulerRot.x <= 150f ? Mathf.Clamp(eulerRot.x, 0f, 50f) : Mathf.Clamp(eulerRot.x, 310f, 360f);
        eulerRot = newX * Vector3.right + eulerRot.y * Vector3.up + eulerRot.z * Vector3.forward;

        //transform.rotation = Quaternion.Euler(eulerRot);
        desiredRotation = Quaternion.Euler(eulerRot);

        var forward = (transform.forward.x * Vector2.right + transform.forward.z * Vector2.up).normalized;
        var seatForward = (seatTransform.forward.x * Vector2.right + seatTransform.forward.z * Vector2.up).normalized;

        angle = Vector2.SignedAngle(seatForward, forward);
        //target = transform.position + camera.transform.forward * 10f;
        target = this.target.position;
    }

    private void LateUpdate()
    {
        //transform.position = Vector3.MoveTowards(transform.position, target.position + offset.x * targetRig.right + offset.z * targetRig.forward + offset.y * targetRig.up, speed);
        transform.position = Vector3.MoveTowards(transform.position, targetRig.position + offset.y * Vector3.up, speed);
        transform.rotation = desiredRotation * Quaternion.Euler(CurrentOffset);

        Debug.Log("CameraFollow, LateUpdate : CurrentOffset = " + CurrentOffset);


        /*if(CurrentOffset != Vector3.zero)
        {
            transform.rotation *= Quaternion.Euler(CurrentOffset);
        }*/
    }
}
