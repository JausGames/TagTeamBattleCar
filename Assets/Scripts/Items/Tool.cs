using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Tool : Item
{
    public override bool Use(ShooterController owner, bool use)
    {
        owner.SwitchItem(true);
        return true;
    }
}
