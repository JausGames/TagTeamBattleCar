using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    public  float speed = 20f;
    public  Camera camera;
    [SerializeField] Vector3 offset;

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position + offset, speed);
    }
}
