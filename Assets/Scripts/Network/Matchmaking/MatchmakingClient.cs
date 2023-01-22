using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MatchmakingClient : NetworkBehaviour
{
    PlayerData data;
    [SerializeField] TeamUi teamUi;
    public PlayerData Data { get => data; set => data = value; }

    private void Start()
    {
        teamUi = FindObjectOfType<TeamUi>();
        if(IsOwner)
            teamUi.addMateEvent.AddListener(delegate { MatchmakingServer.Instance.SubmitTryFindMateServerRpc(data.ClientId, teamUi.MateName); });
    }

    public void CreateData(string name, ulong clientId)
    {
        data = new PlayerData(name, clientId, this.NetworkObjectId);
    }

    [ClientRpc]
    internal void AddMateClientRpc(string mateName)
    {
        Debug.Log("AddMateClientRpc, mateName = " + mateName);
        if (IsOwner)
        {
            AddMate(mateName);
            Debug.Log("AddMateClientRpc, IsOwner = " + IsOwner);

        }
    }
    internal void AddMate(string mateName)
    {
        teamUi.AddTeammateUi(mateName);
    }

    internal void SetUpClient(string name, ulong clientId, ulong playerObjectId)
    {
        if (!IsOwner) return;
        AddMate(name + "#" + clientId);
        CreateData(name, clientId);
    }
}
