using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Weapon
{

    public override bool Use(ShooterController owner)
    {
        var canShoot = base.Use(owner);
        if (!canShoot) return false;
        var hits = ShootRaycast(owner.CameraFollow.Camera, owner.Hitablemask);
        FindRayVictims(owner, hits);
        return true;
    }

}
