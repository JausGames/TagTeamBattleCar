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

        GameObject go = Instantiate(shipPrefab, Vector3.zero, Quaternion.identity);
        go.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId, false);
        var ship = go.GetComponentInChildren<ClientAutoritative.ShipController>();


        GameObject go1 = Instantiate(shootPrefab, Vector3.zero, Quaternion.identity, go.transform);
        go1.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId + 1, false);
        ship.Seats[0].controller = go1.GetComponentInChildren<ShooterAnimatorController>();
        go1.GetComponentInChildren<ShooterController>().Seat = ship.Seats[0];
        go1.GetComponentInChildren<ShooterController>().FindSeatClientRpc(go.GetComponent<NetworkObject>().NetworkObjectId, 0);

        /*GameObject go2 = Instantiate(shootPrefab, Vector3.zero, Quaternion.identity, go.transform);
        go2.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId + 2, false);
        ship.Seats[1].controller = go2.GetComponentInChildren<ShooterAnimatorController>();
        go2.GetComponentInChildren<ShooterController>().Seat = ship.Seats[1];
        go2.GetComponentInChildren<ShooterController>().FindSeatClientRpc(go.GetComponent<NetworkObject>().NetworkObjectId, 1);*/
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
