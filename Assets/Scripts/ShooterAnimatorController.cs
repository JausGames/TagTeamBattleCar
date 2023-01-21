using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ShooterAnimatorController : MonoBehaviour
{
    [SerializeField] NetworkAnimator animator;
    float angle;

    public NetworkAnimator Animator { get => animator; set => animator = value; }
    public float Angle { get => angle;
        set
        {
            angle = value >= 0 ? value / 360f : ((value + 360f) / 360f);
            animator.Animator.SetFloat("angle", angle);
        }
    }
    public void Reload()
    { 
        animator.Animator.SetTrigger("reload");
    }
    public void Die()
    { 
        animator.Animator.SetTrigger("die");
    }

}
