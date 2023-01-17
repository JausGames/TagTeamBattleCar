using ClientAutoritative;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Team : NetworkBehaviour
{
    [SerializeField] NetworkVariable<char> teamName = new NetworkVariable<char>(new char(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] List<ShooterController> shooters = new List<ShooterController>();
    [SerializeField] ClientAutoritative.ShipController driver;
    [SerializeField] NetworkVariable<float> credits = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public int Credits { get => Mathf.FloorToInt(credits.Value); }
    public List<ShooterController> Shooters { get => shooters; set => shooters = value; }
    public ShipController Driver { get => driver; set => driver = value; }
    public NetworkVariable<char> TeamName { get => teamName; set => teamName = value; }


    public bool ChangeAmount(float amount)
    {
        if (amount < 0 && credits.Value < amount) return false;
        credits.Value += amount;
        return true;
    }
    public bool CanChangeAmount(float amount)
    {
        if (amount < 0f && credits.Value < amount) return false;
        return true;
    }

    public bool ChangeCreditAmount(float amount)
    {
        if (CanChangeAmount(amount))
        {
            SubmitChangeMoneyServerRpc(amount);
            return true;
        }
        else return false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitChangeMoneyServerRpc(float amount)
    {
        ChangeAmount(amount);
    }

}
