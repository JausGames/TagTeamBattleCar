using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] float diametre = 1f;
    [SerializeField] float direction = 1f;

    public void RotateWheel(float speed)
    {
        var angularSpeed = ((2f * speed) / diametre) * (180f / Mathf.PI);
        transform.Rotate(Vector3.forward, Mathf.Sign(direction) * angularSpeed * Time.deltaTime);
    }

    
}
