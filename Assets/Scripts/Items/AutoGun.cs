using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGun : Weapon
{
    [SerializeField] bool shooting = false;
    ShooterController owner = null;
    public override bool Use(ShooterController owner, bool use)
    {
        this.owner = owner;
        shooting = use;
        var canShoot = base.Use(owner, use);
        if (!canShoot) return false;
        var hits = ShootRaycast(owner.CameraFollow.Camera, owner.Hitablemask);
        //VFX
        owner.ShootBullet();
        source.PlayOneShot(clipFire);
        FindRayVictims(owner, hits);
        return true;
    }

    private void FixedUpdate()
    {
        if(shooting)
            Use(owner, shooting);

    }

}
