using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : Item
{
    [Header("Stats")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float coolDown = .4f;
    [SerializeField] protected int maximumAmmo;
    [SerializeField] protected int magazineCapacity;
    [SerializeField] private Vector3 recoil = Vector3.right * -2f;

    protected float nextShot;
    protected int ammo;
    private int remainingAmmo;

    [Header("Components")]
    [SerializeField] public Transform canonEnd;
    [SerializeField] public List<ParticleSystem> shootParticles;
    protected Vector3 rndRecoil 
    {
        get
        {
            var t = Random.Range(0f, 1f);
            return recoil * t + (recoil - 2f * recoil.y * Vector3.up) * (1f - t);
        }
    }

    public override bool Use(ShooterController owner)
    {
        Debug.Log("Weapon, Use : Time = " + Time.time);
        Debug.Log("Weapon, Use : nextShot = " + nextShot);
        if (Time.time < nextShot || (ammo == 0 && remainingAmmo == 0)) return false; // check cooldown & ammunition
        if (ammo == 0 && remainingAmmo > 0) { owner.StartReloading(); return false; }  // Auto reload

        Debug.Log("Weapon, Use : shoting");
        ammo--;
        nextShot = Time.time + coolDown;

        owner.CameraFollow.RotationOffset = rndRecoil;
        //VFX
        owner.ShootBullet();
        return true;
    }
    private void Awake()
    {
        ResetAmmo();
    }
    internal void ResetAmmo()
    {
        ammo = magazineCapacity;
        remainingAmmo = maximumAmmo - magazineCapacity;
    }


    public void Reload()
    {
        if (ammo < 0) return;
        var ammoUsed = Mathf.Min(magazineCapacity, remainingAmmo);
        remainingAmmo -= magazineCapacity;
        ammo = ammoUsed;
    }

    protected RaycastHit[] ShootRaycast(Camera camera, LayerMask mask)
    {
        return Physics.RaycastAll(camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), Camera.MonoOrStereoscopicEye.Mono), Mathf.Infinity, mask);
    }
    protected void FindRayVictims(ShooterController owner, RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            switch (hit.collider.gameObject.layer)
            {
                // Hit player body
                case 3:
                case 6:
                    var ennemy = hit.collider.GetComponent<Player>() ? hit.collider.GetComponent<Player>() : hit.collider.GetComponentInParent<Player>();
                    Debug.Log("ShooterController, Shoot : #" + owner.NetworkObjectId + " shot #" + ennemy.NetworkObjectId);
                    owner.SummitGetHitServerRpc(ennemy.NetworkObjectId, damage, owner.NetworkObjectId);
                    owner.GetHitCreditsEarnEvent.Invoke(Mathf.Min(ennemy.Health.Value, damage));
                    return;
                default:
                    break;
            }
        }
    }



}
