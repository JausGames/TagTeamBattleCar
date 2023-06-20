using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System.Text;
using Unity.Netcode.Transports.UNET;
using ParrelSync;

public class MainMenu : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] GameObject ConectionMenu;
    [Header("UI Components")]
    [SerializeField] TMPro.TMP_InputField text_ipAdress;
    [SerializeField] TMPro.TMP_InputField text_currentIp;
    [SerializeField] TMPro.TMP_Text lbl_ping;

    [SerializeField] Button btn_starServer;
    [SerializeField] Button btn_starGame;
    [SerializeField] Button btn_joinGame;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] TMPro.TMP_InputField inputName;


    [SerializeField] List<GameObject> clientMenus;
    [SerializeField] List<GameObject> serverMenus;


    [Header("Client Components")]
    [SerializeField] TeamUi teamUi;


    Ping ping;
    List<int> pingsArray = new List<int>();
    UNetTransport unetTransform;
    int pingCount = 49;

    //string ipAdress = "172.30.114.48";
    string ipAdress = "127.0.0.1";

    private void Update()
    {
        if (ping.isDone)
        {
            pingCount++;
            pingsArray.Add(ping.time);
            if(pingsArray.Count > 100)  pingsArray.Remove(0); 
            if(pingCount == 50)
            {
                var averageValue = 0f;
                foreach (int ping in pingsArray)  averageValue += ping; 
                averageValue = Mathf.Round(averageValue /= pingsArray.Count);
                lbl_ping.text = averageValue.ToString() + "ms";
                pingCount = 0;
            }
            ping = new Ping(ipAdress);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        unetTransform = GetComponent<UNetTransport>();

        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

        text_ipAdress.onValueChanged.AddListener(delegate { unetTransform.ConnectAddress = ipAdress = text_ipAdress.text; });
        text_currentIp.onValueChanged.AddListener(delegate { unetTransform.ConnectAddress = ipAdress = text_currentIp.text; });

        btn_starServer.onClick.AddListener(Server);
        btn_starGame.onClick.AddListener(Host);
        btn_joinGame.onClick.AddListener(Client);

        ping = new Ping("127.0.0.1");

#if UNITY_EDITOR
        //Is this unity editor instance opening a clone project?
        if (ClonesManager.IsClone())
        {
            string customArgument = ClonesManager.GetArgument();
            inputName.text = customArgument;
            Client();
            
        }
        else
        {
            inputName.text = "bogoss";
            Host();
        }
#endif




    }
    private void OnDestroy()
    {
        // Prevent error in the editor
        if (NetworkManager.Singleton == null) { return; }

        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }
    public void Server()
    {
        if (inputName.text == "") return;
        // Hook up password approval check
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(inputName.text);
        NetworkManager.Singleton.StartServer();
    }
    public void Host()
    {
        if (inputName.text == "") return;
        // Hook up password approval check
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(inputName.text);
        NetworkManager.Singleton.StartHost();
    }

    public void Client()
    {
        if (inputName.text == "") return;
        // Set password ready to send to the server to validate
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(inputName.text);
        NetworkManager.Singleton.StartClient();
    }
    public void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            //MLAPI
            //NetworkManager.Singleton.StopHost();
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            //MLAPI
            //NetworkManager.Singleton.StopClient();
        }

        //passwordEntryUI.SetActive(true);
        //leaveButton.SetActive(false);
    }

    private void HandleServerStarted()
    {
        foreach(var menu in serverMenus)
        {
            menu.SetActive(true);
        }

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
            foreach (var menu in clientMenus)
            {
                menu.SetActive(true);
            }
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

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;
        var playerName = Encoding.Default.GetString(connectionData);

        // Your approval logic determines the following values
        response.Approved = true;
        response.CreatePlayerObject = false;

        // The prefab hash value of the NetworkPrefab, if null the default NetworkManager player prefab is used
        response.PlayerPrefabHash = null;

        // Position to spawn the player object (if null it uses default of Vector3.zero)
        response.Position = Vector3.zero;

        // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        response.Rotation = Quaternion.identity;

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;
        Debug.Log("connection approval : name = " + playerName +", id = " + clientId);

        GameObject go = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        var networkObject = go.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(clientId, false);
        var client = go.GetComponent<MatchmakingClient>();
        

        MatchmakingServer.Instance.AddPlayer(playerName, clientId, networkObject.NetworkObjectId);
    }

}
