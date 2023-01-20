using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAnimatorEvent : MonoBehaviour
{
    [SerializeField]
    ShooterController shooter;
    public void OnReload()
    {
        shooter.Reload();
    }
}
