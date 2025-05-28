using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAnimatorEvent : MonoBehaviour
{
    [SerializeField]
    ShooterController shooter;
    public void OnReload()
    {
        Debug.Log("ShooterAnimatorEvent : OnReload");
        shooter.Reload();
    }
    public void OnRespawn()
    {
        Debug.Log("ShooterAnimatorEvent : OnRespawn");
        shooter.Respawn();
    }
    public void OnActivateTrigger()
    {
        Debug.Log("ShooterAnimatorEvent : OnActivateTrigger");
        if (!shooter.IsOwner) return;
        shooter.ActivateMeleeTrigger(true);
        //((MeleeWeapon)shooter.CurrentWeapon).ActivateTrigger(true);
    }
    public void OnDeactivateTrigger()
    {
        Debug.Log("ShooterAnimatorEvent : OnDeactivateTrigger");
        if (!shooter.IsOwner) return;
        shooter.ActivateMeleeTrigger(false);
        //((MeleeWeapon)shooter.CurrentWeapon).ActivateTrigger(false);
    }
}
