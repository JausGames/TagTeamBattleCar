using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MatchmakingClient : NetworkBehaviour
{
    [SerializeField] PlayerData data;
    [SerializeField] TeamUi teamUi;
    [SerializeField] MatchmakingUi matchmakingUi;
    public PlayerData Data { get => data; set => data = value; }
    public List<ulong> TeammateList { get => teammateList; set => teammateList = value; }

    List<ulong> teammateList = new List<ulong>();
    public bool IsClientHost { get => IsHost; }

    #region Start & Set up
    private void Start()
    {
        teamUi = FindObjectOfType<TeamUi>();
        if(IsOwner)
        {
            //Debug.Log("MatchmakingClient, Start : register button");
            teamUi.addMateEvent.AddListener(delegate { MatchmakingServer.Instance.SubmitTryFindMateServerRpc(data.ClientId, teamUi.MateName); });
            teamUi.leaveTeam.AddListener(LeaveTeam);
            matchmakingUi = FindObjectOfType<MatchmakingUi>();
            matchmakingUi.JoinQuitButton.onClick.AddListener(JoinQueue);
        }

    }
    public void CreateData(string name, ulong clientId)
    {
        data = new PlayerData(name, clientId, this.NetworkObjectId);
    }
    internal void SetUpClient(string name, ulong clientId, ulong playerObjectId)
    {
        if (!IsOwner) return;
        AddMate(name + "#" + clientId);
        CreateData(name, clientId);
    }
    #endregion

    #region Team management
    [ClientRpc]
    internal void AddMateClientRpc(string mateName)
    {
        if (IsOwner)
        {
            //Debug.Log("MatchmakingClient, AddMateClientRpc : mate id = " + mateName);
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
    private void LeaveTeam()
    {
        //Debug.Log("MatchmakingClient, LeaveTeam : quitter = " + Data.Name);
        for (int i = 0; i < teammateList.Count; i++)
        {
            if (teammateList[i] != Data.ClientId)
            {
                RemoveMateServerRpc(teammateList[i], Data.Name + "#" + Data.ClientId);
                //Debug.Log("MatchmakingClient, LeaveTeam : teammate #" + teammateList[i]);
            }
        }
        teammateList.Clear();
        teammateList.Add(Data.ClientId);
        teamUi.RemoveAllTeammateUi();
    }

    [ServerRpc]
    internal void RemoveMateServerRpc(ulong mate, string quitterName)
    {
        //Debug.Log("MatchmakingClient, RemoveMateServerRpc");
        MatchmakingServer.Instance.IdToObjectId.TryGetValue(mate, out var mateObjectId);
        GetNetworkObject(mateObjectId).GetComponent<MatchmakingClient>().RemoveMateClientRpc(quitterName);
    }

    [ClientRpc]
    internal void RemoveMateClientRpc(string quitterName)
    {
        //Debug.Log("MatchmakingClient, RemoveMateClientRpc : NetworkObjectId = " + NetworkObjectId + ", is owner = " + IsOwner);
        if (IsOwner)
        {
            //Debug.Log("MatchmakingClient, RemoveMateClientRpc : is client object #" + NetworkObjectId);
            RemoveMate(quitterName);
        }
    }
    internal void RemoveMate(string mateName)
    {
        //Debug.Log("MatchmakingClient, RemoveMate : mateName = " + mateName);
        if (teamUi)
            teamUi.RemoveTeammateUi(mateName);

        var id = mateName.Substring(mateName.IndexOf("#") + 1);
        teammateList.Remove(Convert.ToUInt64(id));
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
    #endregion

    #region Queueing
    void JoinQueue()
    {
        Debug.Log("MatchmakingClient, JoinQueue : OwnerClient id = " + OwnerClientId + ", teammate count = " + teammateList.Count);
        if (teammateList.Count == 2)
        {
            MatchmakingServer.Instance.RegisterTeamServerRpc(teammateList[0], teammateList[1]);
        }
        else if (teammateList.Count == 3)
        {
            MatchmakingServer.Instance.RegisterTeamServerRpc(teammateList[0], teammateList[1], teammateList[2]);
        }

        foreach(var mate in teammateList)
        {
            MatchmakingServer.Instance.AddPlayerToQueueServerRpc(mate);

            matchmakingUi.JoinQuitButton.onClick.RemoveListener(JoinQueue);
            matchmakingUi.JoinQuitButton.onClick.AddListener(LeaveQueue);
        }
    }

    [ClientRpc]
    internal void SetInQueueClientRpc(bool inQueue)
    {
        if (!IsOwner) return;
        matchmakingUi.QueueStatus = inQueue ? "In queue ..." : "Ready to join queue";
        matchmakingUi.ButtonLabel = inQueue ? "Quit" : "Join";
    }
    void LeaveQueue()
    {
        foreach (var mate in teammateList)
        {
            MatchmakingServer.Instance.RemovePlayerToQueueServerRpc(mate);
            matchmakingUi.JoinQuitButton.onClick.RemoveListener(LeaveQueue);
            matchmakingUi.JoinQuitButton.onClick.AddListener(JoinQueue);
        }
    }

    #endregion
}
