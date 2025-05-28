using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestShooter : NetworkBehaviour
{

    enum Mode
    {
        Shooter,
        Driver
    }

    [SerializeField] private Mode mode;
    [Header("Instantiated prefabs")]
    [SerializeField] private GameObject teamPrefab;
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private GameObject shootPrefab;
    [SerializeField] private GameObject aiShipPrefab;
    [SerializeField] private GameObject aiShootPrefab;

    // Use this for initialization
    void Start()
    {
        Host();

        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

        switch (mode)
        {
            case Mode.Shooter:
                SpawnTeamWithPlayerAsShooter();
                break;
            case Mode.Driver:
                SpawnTeamWithPlayerAsDriver();
                break;
            default:
                break;
        }
        SpawnAi();
    }

    public void Host()
    {
        // Hook up password approval check
        //NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        //NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(inputName.text);
        NetworkManager.Singleton.StartHost();
    }

    private void HandleServerStarted()
    {

        // Temporary workaround to treat host as client
        if (NetworkManager.Singleton.IsHost)
        {
            //HandleClientConnected(NetworkManager.ServerClientId);
        }
    }
    private void HandleClientConnected(ulong clientId)
    {
        Debug.Log("MainMenu, HandleClientConnected : clientid = " + clientId);
        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        {
            var haveObject = MatchmakingServer.Instance.IdToObjectId.TryGetValue(clientId, out var objectId);
            MatchmakingServer.Instance.IdToName.TryGetValue(clientId, out var name);

            if (haveObject)
            {
                MatchmakingServer.Instance.SetClientUp(name, clientId, objectId, NetworkManager.Singleton.IsHost && clientId == NetworkManager.Singleton.LocalClientId);
            }
        }

        // Are we the client that is connecting?
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {

        }
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        // Are we the client that is disconnecting?
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            //passwordEntryUI.SetActive(true);
            //leaveButton.SetActive(false);
        }
    }
    private void SpawnTeamWithPlayerAsDriver()
    {

        GameObject teamGo = Instantiate(teamPrefab, Vector3.zero, Quaternion.identity);
        teamGo.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, false);
        var team = teamGo.GetComponent<Team>();
        team.name = "Player team";

        var playerList = new List<ulong>();

        GameObject driverGo = Instantiate(shipPrefab, Vector3.zero, Quaternion.identity);
        driverGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId, false);
        driverGo.transform.parent = teamGo.transform;
        var ship = driverGo.GetComponentInChildren<ClientAutoritative.ShipController>();
        team.AddPlayer(ship.GetComponentInParent<Player>());
        playerList.Add(driverGo.GetComponent<NetworkObject>().NetworkObjectId);


        for (int i = 0; i < 2; i++)
        {
            GameObject shooterGo = Instantiate(aiShootPrefab, Vector3.zero, Quaternion.identity);
            shooterGo.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, false);
            shooterGo.transform.parent = teamGo.transform;
            //shipAi.Seats[0].controller = shooterGo.GetComponentInChildren<ShooterAnimatorController>();
            var shooter = shooterGo.GetComponentInChildren<ShooterController>();
            shooter.Seat = ship.Seats[i];
            shooter.Ship = ship;
            shooter.FindSeatClientRpc(driverGo.GetComponent<NetworkObject>().NetworkObjectId, i);
            team.AddPlayer(shooter.GetComponent<Player>());
            playerList.Add(shooterGo.GetComponent<NetworkObject>().NetworkObjectId);

        }
        SubmitCreateTeamServerRpc(teamGo.GetComponent<NetworkObject>().NetworkObjectId, playerList.ToArray());
    }

    private void SpawnTeamWithPlayerAsShooter()
    {

        GameObject teamGo = Instantiate(teamPrefab, Vector3.zero, Quaternion.identity);
        teamGo.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, false);
        var team = teamGo.GetComponent<Team>();
        team.name = "Player team";

        var playerList = new List<ulong>();

        GameObject driverAiGo = Instantiate(aiShipPrefab, Vector3.up, Quaternion.identity);
        driverAiGo.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, false);
        driverAiGo.transform.parent = teamGo.transform;
        var shipAi = driverAiGo.GetComponentInChildren<ClientAutoritative.ShipController>();
        team.AddPlayer(shipAi.GetComponentInParent<Player>());
        //SubmitAddPlayerToTeam();
        playerList.Add(driverAiGo.GetComponent<NetworkObject>().NetworkObjectId);

        GameObject shooterGo = Instantiate(shootPrefab, Vector3.zero, Quaternion.identity);
        shooterGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId, false);
        shooterGo.transform.parent = teamGo.transform;
        var shooter = shooterGo.GetComponentInChildren<ShooterController>();
        shooter.Seat = shipAi.Seats[0];
        shooter.Ship = shipAi;
        shooter.FindSeatClientRpc(driverAiGo.GetComponent<NetworkObject>().NetworkObjectId, 0);
        team.AddPlayer(shooter.GetComponent<Player>());
        playerList.Add(shooterGo.GetComponent<NetworkObject>().NetworkObjectId);

        GameObject shooterAiGo = Instantiate(aiShootPrefab, Vector3.zero, Quaternion.identity);
        shooterAiGo.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, false);
        shooterAiGo.transform.parent = teamGo.transform;
        //shipAi.Seats[0].controller = shooterGo.GetComponentInChildren<ShooterAnimatorController>();
        var shooterAi = shooterAiGo.GetComponentInChildren<ShooterController>();
        shooterAi.Seat = shipAi.Seats[1];
        shooterAi.Ship = shipAi;
        shooterAi.FindSeatClientRpc(driverAiGo.GetComponent<NetworkObject>().NetworkObjectId, 1);
        team.AddPlayer(shooterAi.GetComponent<Player>());
        playerList.Add(shooterAiGo.GetComponent<NetworkObject>().NetworkObjectId);


        SubmitCreateTeamServerRpc(teamGo.GetComponent<NetworkObject>().NetworkObjectId, playerList.ToArray());
    }

    private void SpawnAi()
    {

        GameObject teamGo = Instantiate(teamPrefab, Vector3.zero, Quaternion.identity);
        teamGo.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, false);
        var team = teamGo.GetComponent<Team>();
        team.name = "Ai Team";

        var playerAiList = new List<ulong>();

        GameObject driverAiGo = Instantiate(aiShipPrefab, Vector3.right * 5f, Quaternion.identity);
        driverAiGo.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, true);
        driverAiGo.transform.parent = teamGo.transform;
        var shipAi = driverAiGo.GetComponentInChildren<ClientAutoritative.ShipController>();
        team.AddPlayer(shipAi.GetComponentInParent<Player>());
        //SubmitAddPlayerToTeam();
        playerAiList.Add(driverAiGo.GetComponent<NetworkObject>().NetworkObjectId);

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
            team.AddPlayer(shooter.GetComponent<Player>());
            playerAiList.Add(shooterGo.GetComponent<NetworkObject>().NetworkObjectId);

        }
        SubmitCreateTeamServerRpc(teamGo.GetComponent<NetworkObject>().NetworkObjectId, playerAiList.ToArray());
    }
    [ServerRpc]
    private void SubmitCreateTeamServerRpc(ulong networkObjectId, ulong[] playersObjectId)
    {
        //if (!IsHost) AddPlayersToTeam(networkObjectId, playersObjectId);
        SetUpTeamClientRpc(networkObjectId, playersObjectId);
        var team = GetNetworkObject(networkObjectId).GetComponent<Team>();
        team.Credits.Value = GameSettings.startingCredits;
    }

    [ClientRpc]
    private void SetUpTeamClientRpc(ulong networkObjectId, ulong[] playersObjectId)
    {
        if (!IsHost)
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
}
