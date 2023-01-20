using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Tool : Item
{
    public override bool Use(ShooterController owner)
    {
        owner.SwitchItem(true);
        return true;
    }
}
