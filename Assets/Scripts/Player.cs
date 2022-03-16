using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    [SerializeField] ClientAutoritative.ShipController motor;
    [SerializeField] Transform lookAt;
    [SerializeField] Rigidbody body;

    public void Start()
    {
        Debug.Log("Network Informations : IsLocalPlayer " + IsLocalPlayer);

        if (!IsLocalPlayer) return;
        var virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCamera.LookAt = lookAt;
        virtualCamera.Follow = transform;
    }
    public void Update()
    {
        Debug.Log("IsLocalPlayer " + IsLocalPlayer);
        if (!IsLocalPlayer) return;
        lookAt.transform.position = transform.position + body.velocity.normalized;
    }

}
