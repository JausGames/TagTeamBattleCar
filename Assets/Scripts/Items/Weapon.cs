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
    [SerializeField] protected int ammo;
    [SerializeField] private int remainingAmmo;

    [Header("Components")]
    [SerializeField] public Transform canonEnd;
    [SerializeField] public List<ParticleSystem> shootParticles;
    [SerializeField] public ParticleSystem bloodParticles;
    [SerializeField] public ParticleSystem woodParticles;
    [SerializeField] public ParticleSystem sandParticles;
    protected Vector3 rndRecoil 
    {
        get
        {
            var t = Random.Range(0f, 1f);
            return recoil * t + (recoil - 2f * recoil.y * Vector3.up) * (1f - t);
        }
    }

    public int RemainingAmmo { get => remainingAmmo; set => remainingAmmo = value; }

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
        for(int i = 0; i < 9; i++)
        {
            Debug.Log("Awake, Weapon : layermask = " + LayerMask.LayerToName(i));
        }
        ResetAmmo();
    }
    internal void ResetAmmo()
    {
        ammo = magazineCapacity;
        remainingAmmo = maximumAmmo - magazineCapacity;
    }


    public void Reload()
    {
        if (remainingAmmo == 0) return;
        var ammoUsed = Mathf.Min(magazineCapacity - ammo, remainingAmmo);
        Debug.Log("Weapon, Reload : ammoUsed = " + ammoUsed);
        remainingAmmo -= ammoUsed;
        ammo += ammoUsed;
    }

    protected RaycastHit[] ShootRaycast(Camera camera, LayerMask mask)
    {
        return Physics.RaycastAll(camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), Camera.MonoOrStereoscopicEye.Mono), Mathf.Infinity, mask);
    }
    protected void FindRayVictims(ShooterController owner, RaycastHit[] hits)
    {
        List<Player> players = new List<Player>();
        List<RaycastHit> rays = new List<RaycastHit>();
        List<int> layers = new List<int>();

        foreach (var hit in hits)
        {
            switch (hit.collider.gameObject.layer)
            {
                // Hit player body
                case 3:
                // Hit kart body
                case 6:
                // Hit horse body
                case 8:
                    var ennemy = hit.collider.GetComponent<Player>() ? hit.collider.GetComponent<Player>() : hit.collider.GetComponentInParent<Player>();
                    //if (players.Contains(ennemy)) break;
                    rays.Add(hit);
                    players.Add(ennemy);
                    layers.Add(hit.collider.gameObject.layer);
                    break;
                default:
                    rays.Add(hit);
                    players.Add(null);
                    layers.Add(hit.collider.gameObject.layer);
                    break;
            }
        }

        var dist = Mathf.Infinity;
        var closest = -1;
        for (var i = 0; i < rays.Count; i++)
        {
            var rayDist = (canonEnd.position - rays[i].point).sqrMagnitude;
            if (rayDist < dist)
            {
                closest = i;
                dist = rayDist;
            }
        }

        if (closest != -1 && players[closest])
        {
            Debug.Log("ShooterController, Shoot : #" + owner.NetworkObjectId + " shot #" + players[closest].NetworkObjectId);
            owner.GetHitCreditsEarnEvent.Invoke(Mathf.Min(players[closest].Health.Value, damage));
            owner.SummitGetHitServerRpc(players[closest].NetworkObjectId, damage, owner.NetworkObjectId);
            Debug.DrawLine(canonEnd.position, hits[closest].point, Color.red);
        }

        var layer = layers[closest];
        var origin = hits[closest].point;
        var direction = hits[closest].normal;
        owner.SubmitShotContactParticleServerRpc(layer, origin, direction);
    }

    
    public void InstantiateContactParticles(int layer, Vector3 origin, Vector3 direction)
    {
        var prtcl = layer == 6 ? woodParticles : layer == 0 ? sandParticles : bloodParticles;
        var prtclInstance = Instantiate(prtcl, origin, Quaternion.identity);
        prtclInstance.transform.LookAt(direction + prtclInstance.transform.position);
        Destroy(prtclInstance, 5f);
    }
}
