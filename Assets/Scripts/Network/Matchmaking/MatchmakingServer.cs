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
    public Dictionary<ulong, string> IdToName { get => idToName; set => idToName = value; }

    Dictionary<ulong, ulong> idToObjectId = new Dictionary<ulong, ulong>();
    public Dictionary<ulong, ulong> IdToObjectId { get => idToObjectId; set => idToObjectId = value; }

    [Header("Matchmaking")]
    #region Singleton
    static MatchmakingServer instance;

    static public MatchmakingServer Instance { get => instance; }
    private void Awake()
    {
        if (instance == null) instance = this;
        DontDestroyOnLoad(this);
    }
    #endregion
    [SerializeField] List<ulong> gameQueue = new List<ulong>();
    List<ulong> playerInTeam = new List<ulong>();
    List<List<ulong>> teams = new List<List<ulong>>();
    [SerializeField] List<int> unfinishedTeam = new List<int>();
    [Header("Matchmaker settings")]
    [SerializeField] private int minPlayer = 6;

    [Header("Instantiated prefabs")]
    [SerializeField] private GameObject teamPrefab;
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private GameObject shootPrefab;
    [SerializeField] private GameObject aiShipPrefab;
    [SerializeField] private GameObject aiShootPrefab;

    #region Client set up
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
        var client = GetNetworkObject(playerObjectId).GetComponent<MatchmakingClient>();
        if (isHost)
            client.SetUpClient(name, clientId, playerObjectId);
        else
        {
            SetClientUpClientRpc(name, clientId, playerObjectId);
            client.TeammateList.Add(Convert.ToUInt64(clientId));
        }
    }
    [ClientRpc]
    public void SetClientUpClientRpc(string name, ulong clientId, ulong playerObjectId)
    {
        GetNetworkObject(playerObjectId).GetComponent<MatchmakingClient>().SetUpClient(name, clientId, playerObjectId);

    }
    #endregion

    #region Submit team up
    [ServerRpc(RequireOwnership = false)]
    internal void SubmitTryFindMateServerRpc(ulong requesterId, string nameToFind)
    {
        var playerExist = idToName.TryGetValue(requesterId, out var requesterName);
        idToObjectId.TryGetValue(requesterId, out var requesterObjectId);
        //Debug.Log("MatchmakingServer, SubmitTryFindMateServerRpc : requesterId = " + requesterId + ", nameToFind = "+ nameToFind);
        if (playerExist)
        {
            //Debug.Log("MatchmakingServer, SubmitTryFindMateServerRpc : player exist ");
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].Name +"#"+ playerList[i].ClientId == nameToFind)
                {
                    //Debug.Log("MatchmakingServer, SubmitTryFindMateServerRpc : is concern player");
                    var requester = GetNetworkObject(requesterObjectId).GetComponent<MatchmakingClient>();
                    var newMember = GetNetworkObject(playerList[i].PlayerObjectId).GetComponent<MatchmakingClient>();

                    var currentTeam = new List<ulong>(requester.TeammateList);
                    //Debug.Log("MatchmakingServer, SubmitTryFindMateServerRpc : currentTeam.Count = " + currentTeam.Count);
                    foreach (var mateId in currentTeam)
                    {
                        //Debug.Log("MatchmakingServer, SubmitTryFindMateServerRpc : add existing teamate = " + mateId);
                        var mateExist = idToName.TryGetValue(mateId, out var mateName);
                        mateExist = idToObjectId.TryGetValue(mateId, out var objectId);

                        if (mateExist)
                        {
                            //Debug.Log("MatchmakingServer, SubmitTryFindMateServerRpc : add existing teamate, mateExist " + mateExist);
                            var member = GetNetworkObject(objectId).GetComponent<MatchmakingClient>();
                            member.AddMateClientRpc(nameToFind);
                            // if host it will add it twice
                            if(!member.IsOwnedByServer) member.TeammateList.Add(Convert.ToUInt64(playerList[i].ClientId));

                            newMember.AddMateClientRpc(mateName + "#" + mateId);
                            // if host it will add it twice
                            if (!newMember.IsOwnedByServer) newMember.TeammateList.Add(Convert.ToUInt64(mateId));
                        }

                    }

                }
            }
        }
    }
    #endregion

    #region Queueing

    [ServerRpc(RequireOwnership = false)] 
    public void AddPlayerToQueueServerRpc(ulong clientId)
    {
        gameQueue.Add(clientId);
        Debug.Log("MatchmakingServer, AddPlayerToQueueServerRpc : clientId = " + clientId + ", queue count = " + gameQueue.Count);

        idToUiObject.TryGetValue(clientId, out var uiObject);
        idToObjectId.TryGetValue(clientId, out var objectId);
        uiObject.GetComponent<ConnectedPlayerUiSetter>().PlayerState = "In queue";
        GetNetworkObject(objectId).GetComponent<MatchmakingClient>().SetInQueueClientRpc(true);

        if (gameQueue.Count >= minPlayer) 
            StartCoroutine(StartGame());
    }


    [ServerRpc(RequireOwnership = false)]
    public void RegisterTeamServerRpc(ulong mate1id, ulong mate2id, ulong mate3id)
    {
        teams.Add(new List<ulong> { mate1id, mate2id, mate3id });
    }
    [ServerRpc(RequireOwnership = false)]
    public void RegisterTeamServerRpc(ulong mate1id, ulong mate2id)
    {
        teams.Add(new List<ulong> { mate1id, mate2id });
        unfinishedTeam.Add(teams.Count - 1);
    }
    [ServerRpc(RequireOwnership = false)] 
    public void RemovePlayerToQueueServerRpc(ulong clientId)
    {
        gameQueue.Remove(clientId);

        idToUiObject.TryGetValue(clientId, out var uiObject);
        idToObjectId.TryGetValue(clientId, out var objectId);
        uiObject.GetComponent<ConnectedPlayerUiSetter>().PlayerState = "Not ready";
        GetNetworkObject(objectId).GetComponent<MatchmakingClient>().SetInQueueClientRpc(false);
    }

    #endregion

    #region Start game

    private IEnumerator StartGame()
    {
        //var teamName = FindObjectOfType<MainMenuLobbyTest>().TeamName;

        Debug.Log("OnlineLobbyController, StartGame");
        var status = NetworkManager.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        while (status != SceneEventProgressStatus.Started)
        {
            Debug.Log("OnlineLobbyController, StartGame : status = " + status);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);

        while (!NetworkManager.Singleton.IsListening)
        {
            Debug.Log("OnlineLobbyController, StartGame : IsListening = " + NetworkManager.Singleton.IsListening);
            yield return new WaitForEndOfFrame();
        }
        /*for (int i = 0; i < gameQueue.Count; i++)
        {
            GameObject teamGo = Instantiate(teamPrefab, Vector3.zero, Quaternion.identity);
            teamGo.GetComponent<NetworkObject>().SpawnWithOwnership(gameQueue[i], false);
            var team = teamGo.GetComponent<Team>();
            //team.name = teamName;
            var playerList = new List<ulong>();

            GameObject driverGo = Instantiate(shipPrefab, Vector3.zero, Quaternion.identity);
            driverGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(gameQueue[i], false);
            driverGo.transform.parent = teamGo.transform;
            var ship = driverGo.GetComponentInChildren<ClientAutoritative.ShipController>();
            team.AddPlayer(ship.GetComponentInParent<Player>());
            playerList.Add(driverGo.GetComponent<NetworkObject>().NetworkObjectId);
        }*/

        for (int i = 0; i < teams.Count; i++)
        {
            GameObject teamGo = Instantiate(teamPrefab, Vector3.zero, Quaternion.identity);
            teamGo.GetComponent<NetworkObject>().SpawnWithOwnership(teams[i][0], false);
            var team = teamGo.GetComponent<Team>();
            //team.name = teamName;

            var playerList = new List<ulong>();

            GameObject driverGo = Instantiate(shipPrefab, Vector3.zero, Quaternion.identity);
            driverGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(teams[i][0], false);
            driverGo.transform.parent = teamGo.transform;
            var ship = driverGo.GetComponentInChildren<ClientAutoritative.ShipController>();
            team.AddPlayer(ship.GetComponentInParent<Player>());
            playerList.Add(driverGo.GetComponent<NetworkObject>().NetworkObjectId);

            for (int j = 0; j < 2; j++)
            {
                GameObject shooterGo = Instantiate(shootPrefab, Vector3.zero, Quaternion.identity);
                shooterGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(teams[i][j+1], false);
                shooterGo.transform.parent = teamGo.transform;
                var shooter = shooterGo.GetComponentInChildren<ShooterController>();
                shooter.Seat = ship.Seats[j];
                shooter.Ship = ship;
                shooter.FindSeatClientRpc(driverGo.GetComponent<NetworkObject>().NetworkObjectId, j);
                team.AddPlayer(shooter.GetComponent<Player>());
                playerList.Add(shooterGo.GetComponent<NetworkObject>().NetworkObjectId);
            }

            SubmitCreateTeamServerRpc(teamGo.GetComponent<NetworkObject>().NetworkObjectId, playerList.ToArray());
        }

        SpawnAi();

    }

    private void SpawnAi()
    {

        GameObject teamGo = Instantiate(teamPrefab, Vector3.zero, Quaternion.identity);
        teamGo.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, false);
        var team = teamGo.GetComponent<Team>();
        team.name = "Ai Team";

        var playerAiList = new List<ulong>();

        GameObject driverAiGo = Instantiate(aiShipPrefab, Vector3.right * 2f, Quaternion.identity);
        driverAiGo.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, true);
        driverAiGo.transform.parent = teamGo.transform;
        var shipAi = driverAiGo.GetComponentInChildren<ClientAutoritative.ShipController>();
        //team.AddPlayer(shipAi.GetComponent<Player>());
        //SubmitAddPlayerToTeam();
        //playerList.Add(driverAiGo.GetComponent<NetworkObject>().NetworkObjectId);

        for (int i = 0; i < 2; i++)
        {
            GameObject shooterGo = Instantiate(aiShootPrefab, Vector3.zero, Quaternion.identity);
            shooterGo.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, false);
            shooterGo.transform.parent = teamGo.transform;
            //shipAi.Seats[0].controller = shooterGo.GetComponentInChildren<ShooterAnimatorController>();
            var shooter = shooterGo.GetComponentInChildren<ShooterController>();
            shooter.Seat = shipAi.Seats[i];
            shooter.Ship = shipAi;
            shooter.FindSeatClientRpc(driverAiGo.GetComponent<NetworkObject>().NetworkObjectId, i);
            //team.AddPlayer(shooter.GetComponent<Player>());
            //playerList.Add(shooterGo.GetComponent<NetworkObject>().NetworkObjectId);

        }
    }

    [ServerRpc]
    private void SubmitCreateTeamServerRpc(ulong networkObjectId, ulong[] playersObjectId)
    {
        if (!IsHost) AddPlayersToTeam(networkObjectId, playersObjectId);
        SetUpTeamClientRpc(networkObjectId, playersObjectId);
        var team = GetNetworkObject(networkObjectId).GetComponent<Team>();
        team.Credits.Value = GameSettings.startingCredits;
    }

    [ClientRpc]
    private void SetUpTeamClientRpc(ulong networkObjectId, ulong[] playersObjectId)
    {
        AddPlayersToTeam(networkObjectId, playersObjectId);
    }

    private Team AddPlayersToTeam(ulong networkObjectId, ulong[] playersObjectId)
    {
        var team = GetNetworkObject(networkObjectId).GetComponent<Team>();
        var players = new List<Player>();
        for (int i = 0; i < playersObjectId.Length; i++)
        {
            var player = GetNetworkObject(playersObjectId[i]).GetComponentInChildren<Player>();
            players.Add(player);
        }
        team.SetTeamMates(players);
        return team;
    }

    #endregion

}

[Serializable]
public class PlayerData
{
    [SerializeField] string name;
    [SerializeField] ulong clientId;
    [SerializeField] ulong playerObjectId;
    public string Name { get => name; set => name = value; }
    public ulong ClientId { get => clientId; set => clientId = value; }
    public ulong PlayerObjectId { get => playerObjectId; set => playerObjectId = value; }

    public PlayerData(string name, ulong clientId, ulong playerObjectId)
    {
        this.name = name;
        this.clientId = clientId;
        this.playerObjectId = playerObjectId;
    }

}
