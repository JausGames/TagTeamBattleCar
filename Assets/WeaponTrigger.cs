using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    [SerializeField] List<Player> touchedPlayer = new List<Player>();
    [SerializeField] List<Collider> colliders = new List<Collider>();
    [SerializeField] MeleeWeapon weapon;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("MeleeWeapon : OnTriggerEnter : trigger enabled");
        var enemy = other.GetComponent<Player>() ? other.GetComponent<Player>() : other.GetComponentInParent<Player>();
        if (enemy && !weapon.Owner.Player.Team.Players.Contains(enemy) && !touchedPlayer.Contains(enemy))
        {
            touchedPlayer.Add(enemy);
            weapon.Owner.SubmitGetHitServerRpc(enemy.NetworkObjectId, weapon.Damage, weapon.Owner.NetworkObjectId);
            weapon.Owner.SubmitStealCreditsServerRpc(enemy.NetworkObjectId, ((MeleeWeapon)weapon).StolenAmount, weapon.Owner.NetworkObjectId);
        }
    }

    internal void Enable(bool value)
    {
        if(!value)
            touchedPlayer.Clear();

        foreach (var col in colliders)
        {
            col.enabled = value;
        }
    }
}
