using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnlineLobbyController : NetworkBehaviour
{
    [SerializeField] GameObject uiPrefab;
    [SerializeField] LobbyPlayerSetter uiSetter;
    [SerializeField] string playerName;
    [SerializeField] string teamName;
    [SerializeField] GameObject teamPrefab;
    [SerializeField] GameObject shipPrefab;
    [SerializeField] GameObject shootPrefab;
    private bool gameStarted = false;

    public string PlayerName { 
        get => playerName; 
        set 
        {
            playerName = value;
            uiSetter.Name = value;
        }
    }
    public bool Role { 
        get => uiSetter.Role; 
        set 
        {
            uiSetter.Role = value;
        }
    }
    public bool Ready { 
        get => uiSetter.Ready; 
        set 
        {
            uiSetter.Ready = value;
        }
    }

    private void Awake()
    {
        var container = FindObjectOfType<LobbyContainer>();
        var instance = Instantiate(uiPrefab, container.transform);
        uiSetter = instance.GetComponent<LobbyPlayerSetter>();
    }
    private void Start()
    {
        SubmitAskPlayersDataServerRpc();
    }

    public void OnReady()
    {
        if(IsOwner)
        {
            SubmitReadyServerRpc();
        }
    }
    public void OnStart()
    {
        if((IsOwnedByServer || IsOwner) && !gameStarted)
        {
            gameStarted = true;
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        var teamName = FindObjectOfType<MainMenu>().TeamName;

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

        GameObject teamGo = Instantiate(teamPrefab, Vector3.zero, Quaternion.identity);
        teamGo.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, false);
        var team = teamGo.GetComponent<Team>();
        team.name = teamName;

        //team.TeamName.Value = lbl_teamname.text;
        var playerList = new List<ulong>();

        GameObject driverGo = Instantiate(shipPrefab, Vector3.zero, Quaternion.identity);
        driverGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId, false);
        driverGo.transform.parent = teamGo.transform;
        var ship = driverGo.GetComponentInChildren<ClientAutoritative.ShipController>();
        team.AddPlayer(ship.GetComponent<Player>());
        //SubmitAddPlayerToTeam();
        playerList.Add(driverGo.GetComponent<NetworkObject>().NetworkObjectId);
        var idList = NetworkManager.ConnectedClientsIds;

        for(int i = 0; i < idList.Count; i++)
        {
            if(OwnerClientId != idList[i])
            {
                GameObject shooterGo = Instantiate(shootPrefab, Vector3.zero, Quaternion.identity);
                shooterGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(idList[i], false);
                shooterGo.transform.parent = teamGo.transform;
                ship.Seats[0].controller = shooterGo.GetComponentInChildren<ShooterAnimatorController>();
                var shooter = shooterGo.GetComponentInChildren<ShooterController>();
                shooter.Seat = ship.Seats[0];
                shooter.Ship = ship;
                shooter.FindSeatClientRpc(driverGo.GetComponent<NetworkObject>().NetworkObjectId, 0);
                team.AddPlayer(shooter.GetComponent<Player>());
                playerList.Add(shooterGo.GetComponent<NetworkObject>().NetworkObjectId);
            }
        }

        SubmitCreateTeamServerRpc(teamGo.GetComponent<NetworkObject>().NetworkObjectId, playerList.ToArray());
    }

    [ServerRpc]
    private void SubmitCreateTeamServerRpc(ulong networkObjectId, ulong[] playersObjectId)
    {
        if(!IsHost)AddPlayersToTeam(networkObjectId, playersObjectId);
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

    [ServerRpc(RequireOwnership = true)]
    private void SubmitReadyServerRpc()
    {
        Ready = true;
        SetReadyClientRpc();
    }

    [ClientRpc]
    private void SetReadyClientRpc()
    {
        Ready = true;
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitAskPlayersDataServerRpc()
    {
        SetDataClientRpc(playerName, uiSetter.Ready, uiSetter.Role);
    }
    [ClientRpc]
    public void SetDataClientRpc(string playerName, bool ready, bool role)
    {
        Debug.Log("client RPC, player name = " + playerName);
        this.PlayerName = playerName;
        this.Ready = ready;
        this.Role = role;
    }

}
