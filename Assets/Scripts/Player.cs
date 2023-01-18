using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;

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

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public NetworkVariable<float> Health { get => health; set => health = value; }

    public void Start()
    {
        if (IsServer)
        {
            health.Value = maxHealth;
        }
        if (IsOwner)
        {
            healthbar.SetMaxHealth(maxHealth);
            healthbar.SetHealth(health.Value);
            health.OnValueChanged += UpdateHealthBar;
        }
        if (IsLocalPlayer)
        {
            var virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            virtualCamera.enabled = true;
            virtualCamera.LookAt = lookAt;
            virtualCamera.Follow = transform;
        }
    }
    public void Update()
    {
        if (!IsLocalPlayer || !lookAt || !body) return;
        lookAt.transform.position = transform.position + body.velocity.normalized;
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

    private void UpdateHealthBar(float previous, float current)
    {
        healthbar.SetHealth(current);
    }

    public void GetHit(float damage)
    {
        Debug.Log("ShooterController, GetHit : damage = " + damage);
        SetHealth(Mathf.Max(0f, health.Value - damage));
        if (health.Value == 0) SubmitDieServerRpc(NetworkObjectId);
    }

    [ServerRpc]
    private void SubmitDieServerRpc(ulong playerid)
    {
        Debug.Log("ShooterController, SubmitDieServerRpc : playerid = " + playerid);
        KillPlayerClientRpc(playerid);
    }

    [ClientRpc]
    private void KillPlayerClientRpc(ulong playerid)
    {
        Debug.Log("ShooterController, KillPlayerClientRpc : playerid = " + playerid);
        GetNetworkObject(playerid).gameObject.GetComponentInChildren<Player>().Die();
    }
    private void Die()
    {
        Debug.Log("ShooterController, Die : Die = " + NetworkObjectId);
        Destroy(gameObject);
    }

}
