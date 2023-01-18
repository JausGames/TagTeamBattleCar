using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicKit : Tool
{
    [SerializeField] float bonus = 50f;

    public override void Use(ShooterController owner)
    {
        owner.GetComponent<Player>().AddHealth(bonus);
        base.Use(owner);
    }
}
