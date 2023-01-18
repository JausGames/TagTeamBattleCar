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

    public override void Use(ShooterController owner)
    {
        
        if (Time.time < nextShot || (ammo == 0 && remainingAmmo == 0)) return; // check cooldown & ammunition
        if (ammo == 0 && remainingAmmo > 0) { Reload(); return; }  // Auto reload

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
    internal void ResetAmmo()
    {
        ammo = magazineCapacity;
        remainingAmmo = maximumAmmo - magazineCapacity;
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
        return Physics.RaycastAll(canonEnd.position, direction, Mathf.Infinity, mask);
    }
    protected void FindRayVictims(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            switch (hit.collider.gameObject.layer)
            {
                // Hit player body
                case 3:
                case 6:
                    /*var ennemy = hit.collider.GetComponent<Player>() ? hit.collider.GetComponent<Player>() : hit.collider.GetComponentInParent<Player>();
                    Debug.Log("ShooterController, Shoot : #" + owner.NetworkObjectId + " shot #" + ennemy.NetworkObjectId);
                    owner.SummitGetHitServerRpc(ennemy.NetworkObjectId, damage);*/
                    break;
                default:
                    break;
            }
        }
    }



}
