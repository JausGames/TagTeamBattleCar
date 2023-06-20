using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepareKit : Tool
{
    [SerializeField] float bonus = 250f;

    public override bool Use(ShooterController owner, bool use)
    {
        owner.Ship.GetComponent<Player>().AddHealth(bonus);
        return base.Use(owner, use);
         
    }
}
