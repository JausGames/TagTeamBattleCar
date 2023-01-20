using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;
using System;

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
    [SerializeField] CreditsUi creditsUi;
    [SerializeField] Team team;
    [SerializeField] PlayerController controller;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public NetworkVariable<float> Health { get => health; set => health = value; }
    public Team Team { get => team; set => team = value; }

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

    public void GetHit(float damage)
    {
        Debug.Log("Player, GetHit : damage = " + damage);
        SetHealth(Mathf.Max(0f, health.Value - damage));
        if (health.Value == 0) KillPlayerClientRpc(NetworkObjectId);
    }
    private void UpdateHealthBar(float previous, float current)
    {
        healthbar.SetHealth(current);
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
