using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : Item
{
    [Header("Stats")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float coolDown = .4f;
    [SerializeField] private Vector3 recoil = Vector3.right * -2f;

    [Header("Componenents")]
    [SerializeField] public Transform canonEnd;
    [SerializeField] public List<ParticleSystem> shootParticles;

    protected float nextShot;

    protected Vector3 rndRecoil 
    {
        get
        {
            var t = Random.Range(0f, 1f);
            return recoil * t + (recoil - 2f * recoil.y * Vector3.up) * (1f - t);
        }
    }
}
