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
        if (!IsLocalPlayer) return;
        var virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCamera.enabled = true;
        virtualCamera.LookAt = lookAt;
        virtualCamera.Follow = transform;
    }
    public void Update()
    {
        if (!IsLocalPlayer) return;
        lookAt.transform.position = transform.position + body.velocity.normalized;
    }

}
