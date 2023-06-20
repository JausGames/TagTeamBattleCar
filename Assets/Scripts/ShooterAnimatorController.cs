using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ShooterAnimatorController : MonoBehaviour
{
    [SerializeField] NetworkAnimator animator;
    float dirX;

    public NetworkAnimator Animator { get => animator; set => animator = value; }
    public float DirX { get => dirX;
        set
        {
            dirX = value;
        }
    }
    private void FixedUpdate()
    {
        var currDirX = animator.Animator.GetFloat("dirX");
        if(currDirX * dirX > 0)
            animator.Animator.SetFloat("dirX", Mathf.MoveTowards(currDirX, dirX, 0.1f));
        else
            animator.Animator.SetFloat("dirX", Mathf.MoveTowards(currDirX, dirX, 0.3f));

        var layerId = animator.Animator.GetLayerIndex("Legs");
        var weight = animator.Animator.GetLayerWeight(layerId);

        if (Mathf.Abs(dirX) < 0.1f)
            animator.Animator.SetLayerWeight(layerId, Mathf.MoveTowards(weight, 0f, 0.1f));
        else
            animator.Animator.SetLayerWeight(layerId, Mathf.MoveTowards(weight, 1f, 0.1f));
    }
    public void Reload()
    { 
        animator.Animator.SetTrigger("reload");
    }
    public void Die()
    { 
        animator.Animator.SetTrigger("die");
    }

    internal void SetLayer(int layerId)
    {
        Debug.Log("SetLayer : " + layerId);
        for (int i = 1; i < 5; i++)
        {
            if(i == layerId)
                animator.Animator.SetLayerWeight(i, 1f);
            else
                animator.Animator.SetLayerWeight(i, 0f);
        }
    }

    internal void PlayFiring()
    {
        animator.Animator.SetTrigger("firing");
    }
}
