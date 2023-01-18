using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Weapon
{

    public override void Use(ShooterController owner)
    {
        base.Use(owner);
        var hits = ShootRaycast(owner.CameraContainer.forward, owner.Hitablemask);
        FindRayVictims(hits);
    }

}
