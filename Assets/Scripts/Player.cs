using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;
using System;
using UnityEngine.Events;

public class Player : NetworkBehaviour
{
    [Header("Stats")]
    [SerializeField] NetworkVariable<float> health = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] float maxHealth = 100f;

    [Space]
    [Header("Driver component")]
    [SerializeField] Transform lookAt;
    [SerializeField] Rigidbody body;

    [Space]
    [Header("Component")]
    [SerializeField] HealthBar healthbar;
    [SerializeField] List<HealthBar> matehealthbars;
    [SerializeField] CreditsUi creditsUi;
    [SerializeField] Team team;
    [SerializeField] PlayerController controller;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public NetworkVariable<float> Health { get => health; set => health = value; }
    public Team Team { get => team; set => team = value; }

    [HideInInspector] UnityEvent<ulong> dieCreditsEarnEvent = new UnityEvent<ulong>();
    public UnityEvent<ulong> DieCreditsEarnEvent { get => dieCreditsEarnEvent; set => dieCreditsEarnEvent = value; }
    public PlayerController Controller { get => controller; set => controller = value; }



    public void Start()
    {
        if (IsServer)
        {
            health.Value = maxHealth;
        }
        if (IsOwner && healthbar)
        {
            healthbar.SetMaxHealth(maxHealth);
            healthbar.SetHealth(health.Value);


            health.OnValueChanged += UpdateHealthBar;


            var threated = 0;
            for (int i = 0; i < 3; i++)
            {
                if (Team.Players[i] && Team.Players[i] != this)
                {
                    threated++;
                    Team.Players[i].Health.OnValueChanged += threated == 1 ? UpdateMateHealthBar1 : UpdateMateHealthBar2;
                    matehealthbars[threated - 1].SetMaxHealth(Team.Players[i].MaxHealth);
                    matehealthbars[threated - 1].SetHealth(Team.Players[i].MaxHealth);
                }
            }
        }

    }

    internal void UpdateCreditsUi(float previousValue, float newValue)
    {
        if (creditsUi)
            creditsUi.Amount = Mathf.FloorToInt(newValue);
    }
    #region Network var : Health
    public void SetHealth(float health)
    {
        this.health.Value = health;
    }
    public void AddHealth(float regenValue)
    {
        SubmitAddHealthServerRpc(regenValue);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitAddHealthServerRpc(float regenValue)
    {
        this.health.Value = Mathf.Min(health.Value + regenValue, maxHealth);
    }
    #endregion

    public void GetHit(float damage, ulong killerId)
    {
        Debug.Log("Player, GetHit : damage = " + damage);
        SetHealth(Mathf.Max(0f, health.Value - damage));
        if (health.Value == 0)
        {
            GetNetworkObject(killerId).GetComponentInChildren<Player>().DieCreditsEarnEvent.Invoke(NetworkObjectId);
            KillPlayerClientRpc(NetworkObjectId);
        }
    }
    private void UpdateHealthBar(float previous, float current)
    {
        healthbar.SetHealth(current);
    }
    private void UpdateMateHealthBar1(float previous, float current)
    {
        matehealthbars[0].SetHealth(current);
    }
    private void UpdateMateHealthBar2(float previous, float current)
    {
        matehealthbars[1].SetHealth(current);
    }

    [ServerRpc]
    private void SubmitDieServerRpc(ulong playerid)
    {
        Debug.Log("Player, SubmitDieServerRpc : playerid = " + playerid);
        
    }

    [ClientRpc]
    private void KillPlayerClientRpc(ulong playerid)
    {
        Debug.Log("Player, KillPlayerClientRpc : playerid = " + playerid);
        GetNetworkObject(playerid).gameObject.GetComponentInChildren<Player>().controller.Die();
    }

}
