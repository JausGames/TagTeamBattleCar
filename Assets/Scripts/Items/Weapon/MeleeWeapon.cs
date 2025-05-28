using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private WeaponTrigger trigger = null;
    [SerializeField] private float stolenAmount = 50f;

    public float StolenAmount { get => stolenAmount; set => stolenAmount = value; }

    public override bool Use(ShooterController owner, bool use)
    {
        this.owner = owner;
        if (!base.Use(owner, use) && !use) return false;
        Debug.Log("MeleeWeapon : Use melee, owner = " + owner.NetworkObjectId);
        owner.ShootBullet();
        owner.ActivateIk(false);

        return true;
    }

    public void ActivateTrigger(bool value)
    {
        Debug.Log("MeleeWeapon : ActivateTrigger : value = " + value);
        //if (!value) owner.ActivateIk(true);
        trigger.Enable(value);
        foreach(var prtc in shootParticles)
        {
            if (value) prtc.Play();
            else prtc.Stop();
        }
    }

}
