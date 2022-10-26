using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimator : MonoBehaviour
{
    [SerializeField] List<Animator> animators = new List<Animator>();

    public void SetSpeed(float speed)
    {
        if (speed > 0.5f)
            Debug.Log("allez");

        foreach (var animator in animators)
            animator.SetFloat("Speed", speed);
    }
}
