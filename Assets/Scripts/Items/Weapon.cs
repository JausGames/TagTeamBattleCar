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

    [Header("Logic")]
    protected float nextShot;
    [SerializeField] protected int ammo;
    [SerializeField] private int remainingAmmo;

    [Header("Componenents")]
    [SerializeField] public Transform canonEnd;
    [SerializeField] public List<ParticleSystem> shootParticles;



    public override void Use(ShooterController owner)
    {
        if (Time.time < nextShot) return;
        if (ammo == 0 && remainingAmmo > 0) { Reload(); return; }
        else if (ammo == 0 && remainingAmmo == 0) return;
        ammo--;
        nextShot = Time.time + coolDown;

        owner.CameraFollow.RotationOffset = rndRecoil;
        //VFX
        owner.SubmitShootServerRpc();
    }

        private void Awake()
    {
        ResetAmmo();
    }

    protected void Reload()
    {
        if (ammo < 0) return;
        var ammoUsed = Mathf.Min(magazineCapacity, remainingAmmo);
        remainingAmmo -= magazineCapacity;
        ammo = ammoUsed;
    }

    protected RaycastHit[] ShootRaycast(Vector3 direction, LayerMask mask)
    {
        return Physics.RaycastAll(canonEnd.position, direction, 50f, mask);
    }


    protected Vector3 rndRecoil 
    {
        get
        {
            var t = Random.Range(0f, 1f);
            return recoil * t + (recoil - 2f * recoil.y * Vector3.up) * (1f - t);
        }
    }

    public int RemainingAmmo { get => remainingAmmo; set => remainingAmmo = value; }

    internal void ResetAmmo()
    {
        ammo = magazineCapacity;
        remainingAmmo = maximumAmmo - magazineCapacity;
    }
}
