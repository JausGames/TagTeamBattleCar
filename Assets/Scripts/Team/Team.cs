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
    [SerializeField] List<Player> players = new List<Player>();
    [SerializeField] NetworkVariable<float> credits = new NetworkVariable<float>(150f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<char> TeamName { get => teamName; set => teamName = value; }
    public NetworkVariable<float> Credits { get => credits; set => credits = value; }
    public List<Player> Players { get => players; set => players = value; }

    int test;

    private void Update()
    {
        /*if(IsOwner)
            if (Mathf.FloorToInt(Time.time) != test)
            {
                test = Mathf.FloorToInt(Time.time);
                var rnd = UnityEngine.Random.Range(-100f, 100f);
                if (CanChangeAmount(rnd))
                    ChangeAmount(rnd);
            }*/
    }
    public void AddPlayer(Player player)
    {
        players.Add(player);
        player.Team = this;
        credits.OnValueChanged += player.UpdateCreditsUi;
        if(player.GetComponent<ShooterController>())
        {
            var controller = player.GetComponent<ShooterController>();
            controller.GetHitCreditsEarnEvent.AddListener(OnEarnFromHit);
            player.DieCreditsEarnEvent.AddListener(OnEarnFromKill);
        }
    }


    private bool ChangeAmount(float amount)
    {
        if (amount < 0 && credits.Value < amount) return false;
        credits.Value += amount;
        return true;
    }
    private bool CanChangeAmount(float amount)
    {
        if (credits.Value + amount < 0f) return false;
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

    internal void SetTeamMates(List<Player> players)
    {
        foreach(Player player in players)
        {
            AddPlayer(player);
        }
    }
    internal void OnEarnFromHit(float damage)
    {
        ChangeCreditAmount(damage * .5f);
    }
    internal void OnEarnFromKill(ulong victimId)
    {
        var player = GetNetworkObject(victimId).GetComponentInChildren<Player>();
        float value = 100f;
        if (player.Controller is ShipController) value = 500f;
        else if (player.Controller is ShooterController) value = 100f;
        ChangeCreditAmount(value);
    }
}
