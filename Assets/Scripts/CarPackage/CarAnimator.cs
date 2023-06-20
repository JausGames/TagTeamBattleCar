using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimator : MonoBehaviour
{
    [SerializeField] List<Animator> animators = new List<Animator>();
    float inputX;
    [SerializeField] float clampSpeed = 5f;

    public void SetSpeed(float speed, float speedX, float inputX)
    {
        this.inputX = inputX;
        foreach (var animator in animators)
        {
            animator.SetFloat("Speed", speed);
            animator.SetFloat("SpeedX", speedX);
            //animator.SetFloat("SpeedX", Mathf.MoveTowards(animator.GetFloat("SpeedX"), speedX, clampSpeed * Time.deltaTime));
            animator.SetFloat("InputX", Mathf.MoveTowards(animator.GetFloat("InputX"), inputX, clampSpeed * Time.deltaTime));
        }
    }
    private void Update()
    {
        foreach (var animator in animators)
        {
            var current = animator.GetFloat("InputX");
            var ratio = current * inputX > 0 ? 1f : 1f;
            animator.SetFloat("InputX", Mathf.MoveTowards(current, inputX, clampSpeed * ratio * Time.deltaTime));
        }
    }
}
