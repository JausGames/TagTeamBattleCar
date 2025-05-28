using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Tool
{
    [SerializeField] float bonus = 200f;
    [SerializeField] GameObject prefab;

    public override bool Use(ShooterController owner, bool use)
    {
        Instantiate(prefab, owner.Player.Team.Cart.position, owner.Player.Team.Cart.rotation, owner.Player.Team.Cart);
        base.Use(owner, use);
        return true;
    }
}
