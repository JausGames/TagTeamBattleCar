using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

abstract public class PlayerController : NetworkBehaviour
{
    abstract public void Die();
}
