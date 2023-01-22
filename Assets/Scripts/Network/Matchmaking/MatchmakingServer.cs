using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class MatchmakingServer : NetworkBehaviour
{
    [Header("Ui")]
    [SerializeField] GameObject playerUiItem;
    [SerializeField] Transform listContainer;

    List<PlayerData> playerList = new List<PlayerData>();
    public List<PlayerData> PlayerList { get => playerList; set => playerList = value; }
    Dictionary<ulong, GameObject> idToUiObject = new Dictionary<ulong, GameObject>();
    Dictionary<ulong, string> idToName = new Dictionary<ulong, string>();
    Dictionary<ulong, ulong> idToObjectId = new Dictionary<ulong, ulong>();

    [Header("Matchmaking")]
    static MatchmakingServer instance;

    static public MatchmakingServer Instance { get => instance; }
    public Dictionary<ulong, ulong> IdToObjectId { get => idToObjectId; set => idToObjectId = value; }
    public Dictionary<ulong, string> IdToName { get => idToName; set => idToName = value; }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AddPlayer(string name, ulong clientId, ulong playerObjectId)
    {
        var itemGo = Instantiate(playerUiItem, listContainer);
        var uiSetter = itemGo.GetComponent<ConnectedPlayerUiSetter>();
        uiSetter.PlayerName = name;
        uiSetter.PlayerId = clientId.ToString();
        uiSetter.PlayerState = "Not ready";

        playerList.Add(new PlayerData(name, clientId, playerObjectId));
        idToUiObject.Add(clientId, itemGo);
        idToName.Add(clientId, name);
        idToObjectId.Add(clientId, playerObjectId);

    }

    public void SetClientUp(string name, ulong clientId, ulong playerObjectId, bool isHost = false)
    {
        if (isHost)
            GetNetworkObject(playerObjectId).GetComponent<MatchmakingClient>().SetUpClient(name, clientId, playerObjectId);
        else
            SetClientUpClientRpc(name, clientId, playerObjectId);
    }
    [ClientRpc]
    public void SetClientUpClientRpc(string name, ulong clientId, ulong playerObjectId)
    {
        GetNetworkObject(playerObjectId).GetComponent<MatchmakingClient>().SetUpClient(name, clientId, playerObjectId);

    }

    [ServerRpc(RequireOwnership = false)]
    internal void SubmitTryFindMateServerRpc(ulong requesterId, string nameToFind)
    {
        var playerExist = idToName.TryGetValue(requesterId, out var requesterName);
        idToObjectId.TryGetValue(requesterId, out var requesterObjectId);
        if (playerExist)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].Name +"#"+ playerList[i].ClientId == nameToFind)
                {
                    var requester = GetNetworkObject(requesterObjectId).GetComponent<MatchmakingClient>();
                    var newMember = GetNetworkObject(playerList[i].PlayerObjectId).GetComponent<MatchmakingClient>();

                    var currentTeam = new List<ulong>(requester.TeammateList);
                    foreach (var mateId in currentTeam)
                    {

                        var mateExist = idToName.TryGetValue(mateId, out var mateName);
                        mateExist = idToObjectId.TryGetValue(mateId, out var objectId);

                        if (mateExist)
                        {
                            var member = GetNetworkObject(objectId).GetComponent<MatchmakingClient>();
                            //member.TeammateList.Add(newMember.Data.ClientId);
                            member.AddMateClientRpc(nameToFind);

                            newMember.AddMateClientRpc(mateName + "#" + mateId);
                            //newMember.TeammateList.Add(mateId);
                        }

                    }

                }
            }
        }
    }
}

[Serializable]
public class PlayerData
{
    [SerializeField] string name;
    [SerializeField] ulong clientId;
    [SerializeField] ulong playerObjectId;

    public PlayerData(string name, ulong clientId, ulong playerObjectId)
    {
        this.name = name;
        this.clientId = clientId;
        this.playerObjectId = playerObjectId;
    }

    public string Name { get => name; set => name = value; }
    public ulong ClientId { get => clientId; set => clientId = value; }
    public ulong PlayerObjectId { get => playerObjectId; set => playerObjectId = value; }
}
