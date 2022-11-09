using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Netcode;

public class ShooterController : NetworkBehaviour
{
    [Header("Inputs")]
    [SerializeField] NetworkVariable<float> health = new NetworkVariable<float>(50f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] NetworkVariable<Vector3> look = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    [Space]
    [Header("Components")]
    public LayerMask hitablemask;
    [SerializeField] Transform cameraContainer;
    [SerializeField] Rigidbody body;
    [SerializeField] float cameraSpeed;

    // Update is called once per frame
    private void Start ()
    {
        hitablemask = 1 << 3;
        if (IsOwner)
        {
            body.isKinematic = false;
            cameraContainer.GetComponent<CameraFollow>().camera.enabled = true;
        }
    }
    private void Update()
    {
        if (IsOwner)
        {
            body.AddForce(Vector3.down, ForceMode.VelocityChange);
            cameraContainer.rotation = Quaternion.Euler(cameraContainer.rotation.eulerAngles + new Vector3(look.Value.y * Time.deltaTime * cameraSpeed, look.Value.x * Time.deltaTime * cameraSpeed, 0f));
        }
    }

    #region Network var : Rotation
    public void SetHealth(float health)
    {
        this.health.Value = health;
    }
    #endregion
    internal void Shoot(bool context)
    {
        if (!context) return;
        Debug.DrawRay(transform.position + 1f * Vector3.up, cameraContainer.forward * 50f, Color.red, .5f);
        var hits = Physics.RaycastAll(transform.position + 1f * Vector3.up, cameraContainer.forward, 50f, 1 << 3);
        foreach (var hit in hits)
        {
            var ennemy = hit.collider.GetComponent<ShooterController>() ? hit.collider.GetComponent<ShooterController>() : hit.collider.GetComponentInParent<ShooterController>();

            Debug.Log(NetworkObjectId + " shot " + ennemy.NetworkObjectId);
            SummitGetHitServerRpc(ennemy.NetworkObjectId, 50f);
        }
    }


    [ServerRpc]
    void SummitGetHitServerRpc(ulong playerid, float damage)
    {
        GetNetworkObject(playerid).GetComponent<ShooterController>().GetHit(damage);
    }
    private void GetHit(float damage)
    {
        SetHealth(Mathf.Max(0f, health.Value - damage));
        if (health.Value == 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    internal void Look(Vector2 vector2)
    {
        look.Value = vector2;
    }
}
