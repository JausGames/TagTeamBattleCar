using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    public  Camera camera;
    [SerializeField] Vector3 offset;

    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
