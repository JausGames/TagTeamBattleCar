using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MatchmakingClient : NetworkBehaviour
{
    [SerializeField] PlayerData data;
    [SerializeField] TeamUi teamUi;
    public PlayerData Data { get => data; set => data = value; }
    public List<ulong> TeammateList { get => teammateList; set => teammateList = value; }

    List<ulong> teammateList = new List<ulong>();

    private void Start()
    {
        teamUi = FindObjectOfType<TeamUi>();
        if(IsOwner)
        {
            teamUi.addMateEvent.AddListener(delegate { MatchmakingServer.Instance.SubmitTryFindMateServerRpc(data.ClientId, teamUi.MateName); });
            teamUi.leaveTeam.AddListener(LeaveTeam);
        }
    }
    public void CreateData(string name, ulong clientId)
    {
        data = new PlayerData(name, clientId, this.NetworkObjectId);
    }

    private void LeaveTeam()
    {
        Debug.Log("MatchmakingClient, LeaveTeam : quitter = " + Data.Name);
        for (int i = 0; i < teammateList.Count; i++)
        {
            if(teammateList[i] != Data.ClientId)
            {
                RemoveMateServerRpc(teammateList[i], Data.Name+"#"+Data.ClientId);
                Debug.Log("MatchmakingClient, LeaveTeam : teammate #" + teammateList[i]);
            }
        }
        teammateList.Clear();
        teammateList.Add(Data.ClientId);
        teamUi.RemoveAllTeammateUi();
    }

    [ServerRpc]
    internal void RemoveMateServerRpc(ulong mate, string quitterName)
    {
        Debug.Log("MatchmakingClient, RemoveMateServerRpc");
        MatchmakingServer.Instance.IdToObjectId.TryGetValue(mate, out var mateObjectId);
        GetNetworkObject(mateObjectId).GetComponent<MatchmakingClient>().RemoveMateClientRpc(quitterName);
    }

    [ClientRpc]
    internal void RemoveMateClientRpc(string quitterName)
    {
        Debug.Log("MatchmakingClient, RemoveMateClientRpc : NetworkObjectId = " + NetworkObjectId + ", is owner = " + IsOwner);
        if (IsOwner)
        {
            Debug.Log("MatchmakingClient, RemoveMateClientRpc : is client object #" + NetworkObjectId);
            RemoveMate(quitterName);
        }
    }
    internal void RemoveMate(string mateName)
    {
        Debug.Log("MatchmakingClient, RemoveMate : mateName = " + mateName);
        if (teamUi)
            teamUi.RemoveTeammateUi(mateName);

        var id = mateName.Substring(mateName.IndexOf("#") + 1);
        teammateList.Remove(Convert.ToUInt64(id));
    }

    [ClientRpc]
    internal void AddMateClientRpc(string mateName)
    {
        if (IsOwner)
        {
            AddMate(mateName);

        }
    }
    internal void AddMate(string mateName)
    {
        if(teamUi)
            teamUi.AddTeammateUi(mateName);
        else
            StartCoroutine(WaitForTeamUi(mateName));

        var id = mateName.Substring(mateName.IndexOf("#") + 1);
        teammateList.Add(Convert.ToUInt64(id));
    }
    IEnumerator WaitForTeamUi(string mateName)
    {
        while(teamUi == null)
        {
            teamUi = FindObjectOfType<TeamUi>();
            yield return new WaitForEndOfFrame();
        }
        teamUi.AddTeammateUi(mateName);
    }

    internal void SetUpClient(string name, ulong clientId, ulong playerObjectId)
    {
        if (!IsOwner) return;
        AddMate(name + "#" + clientId);
        CreateData(name, clientId);
    }
}
