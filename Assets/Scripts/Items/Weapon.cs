using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : Item
{
    [SerializeField] public Transform canonEnd;
    [SerializeField] public List<ParticleSystem> shootParticles;
}
