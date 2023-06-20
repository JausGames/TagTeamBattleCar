using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] float numberOfBullet = 8;
    [SerializeField] float dispersion = 1f;
    [SerializeField] float distanceDispersion = 5f;

    public override bool Use(ShooterController owner, bool use)
    {
        var canShoot = base.Use(owner, use);
        if (!canShoot) return false;

        for(int i = 0; i < numberOfBullet; i++)
        {
            var screenCenter = owner.CameraFollow.Camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), Camera.MonoOrStereoscopicEye.Mono);
            var randomX = Random.Range(-dispersion, dispersion);
            var randomY = Random.Range(-dispersion, dispersion);
            var passThroughPoint = screenCenter.GetPoint(distanceDispersion + 1f) + canonEnd.right * randomX + canonEnd.up * randomY;
            var direction = passThroughPoint - canonEnd.position;
            var hits = ShootRaycastFromGun(owner.Hitablemask, canonEnd.position, direction);
            //VFX
            owner.ShootBullet(passThroughPoint);
            FindRayVictims(owner, hits);

        }
        source.PlayOneShot(clipFire);
        return true;
    }

}
