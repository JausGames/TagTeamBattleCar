using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepareKit : Tool
{
    [SerializeField] float bonus = 250f;

    public override void Use(ShooterController owner)
    {
        owner.Ship.GetComponent<Player>().AddHealth(bonus);
        base.Use(owner);
    }
}
