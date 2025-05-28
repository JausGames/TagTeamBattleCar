using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicKit : Tool
{
    [SerializeField] float bonus = 50f;

    public override bool Use(ShooterController owner, bool use)
    {
        owner.GetComponent<Player>().AddHealth(bonus);
        base.Use(owner, use);
        return true;
    }
}
