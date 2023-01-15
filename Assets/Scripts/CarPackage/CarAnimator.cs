using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimator : MonoBehaviour
{
    [SerializeField] List<Animator> animators = new List<Animator>();

    public void SetSpeed(float speed)
    {
        foreach (var animator in animators)
            animator.SetFloat("Speed", speed);
    }
}
