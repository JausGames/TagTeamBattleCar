using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class TestMoveOnline : NetworkBehaviour
{
    Vector2 move = Vector2.zero;
    [SerializeField] Rigidbody body;

    public void SetMove(CallbackContext context)
    {
        if (!IsOwner) return;
        var move = context.ReadValue<Vector2>();
        this.move = move;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDir = new Vector3(move.x, 0f, move.y);

        float moveSpeed = 30f;
        var value = moveDir * moveSpeed * Time.deltaTime;
        body.velocity += value;
        Debug.Log("value = " + value + ", vel = " + body.velocity);
    }
}
