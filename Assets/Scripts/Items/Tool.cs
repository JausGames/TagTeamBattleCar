using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Tool : Item
{
    public override void Use(ShooterController owner)
    {
        owner.SwitchItem(true);
    }
}
