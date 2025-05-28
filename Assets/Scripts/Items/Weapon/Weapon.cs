using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : Item
{
    [SerializeField] protected ShooterController owner = null;
    public ShooterController Owner { get => owner; set => owner = value; }
    [SerializeField] public string layerName;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float coolDown = .4f;
    protected float nextShot;
    [SerializeField] public List<ParticleSystem> shootParticles;

    public float Damage { get => damage; set => damage = value; }

    public override bool Use(ShooterController owner, bool use)
    {
        if (owner == null) 
            this.owner = owner;

        if (Time.time < nextShot) return false; // check cooldown
        nextShot = Time.time + coolDown;
        return true;
    }
}
