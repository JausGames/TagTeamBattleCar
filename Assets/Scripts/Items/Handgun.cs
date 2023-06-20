using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Weapon
{

    public override bool Use(ShooterController owner, bool use)
    {
        var canShoot = base.Use(owner, use);
        if (!canShoot) return false;
        var hits = ShootRaycast(owner.CameraFollow.Camera, owner.Hitablemask);
        //VFX
        owner.ShootBullet();
        source.PlayOneShot(clipFire);
        FindRayVictims(owner, hits);
        return true;
    }

}
