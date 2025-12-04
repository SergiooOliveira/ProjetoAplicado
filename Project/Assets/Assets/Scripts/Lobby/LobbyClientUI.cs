using FishNet;
using FishNet.Connection;
using FishNet.Transporting;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class LobbyClientUI : MonoBehaviour
{
    #region Fields

    [Header("UI Elements")]
    public TMP_Text roomCodeText;
    public TMP_InputField joinInput;
    public TMP_Text feedbackText;
    public TMP_Text playerListText;
    public GameObject lobby;
    public GameObject createRoom;
    public GameObject room;
    public GameObject ip;
    public GameObject joinRoom;
    public GameObject play;

    [Header("IP Settings")]
    public TMP_InputField ipInput;
    public ushort port = 7777;

    private string currentRoomCode;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        var client = InstanceFinder.ClientManager;
        var server = InstanceFinder.ServerManager;

        client.OnClientConnectionState += OnClientConnectionStateChanged;

        // Notification of disconnection from any client (on the host)
        server.OnRemoteConnectionState += (conn, args) =>
        {
            if (args.ConnectionState == RemoteConnectionState.Stopped)
            {
                LobbyManager.Instance.RemovePlayer(conn);
            }
        };

        client.RegisterBroadcast<PlayerListUpdate>((msg, channel) =>
        {
            // Update list of players whenever received from the serve
            playerListText.text = string.Join("\n", msg.playerIds);
        });

        RegisterClientMessages();
    }

    void Start()
    {
        joinInput.onValueChanged.AddListener(OnCodeTyping);
    }

    #endregion

    #region Start Host

    public void StartAsHost()
    {
        // 1. Start server
        InstanceFinder.ServerManager.StartConnection();

        // 2. Start LOCAL client
        InstanceFinder.ClientManager.StartConnection("localhost", port);

        StartCoroutine(CreateRoom());
    }

    private IEnumerator CreateRoom()
    {
        yield return new WaitForSeconds(1f);

        lobby.SetActive(false);
        room.SetActive(true);

        var hostConn = InstanceFinder.ServerManager.Clients.Values.FirstOrDefault();
        CreateRoomUIForHost(hostConn);
    }

    private void CreateRoomUIForHost(NetworkConnection hostConn)
    {
        if (hostConn == null)
        {
            hostConn = InstanceFinder.ServerManager.Clients.Values.FirstOrDefault();
        }

        if (hostConn == null)
        {
            feedbackText.text = "Erro ao criar sala. Sem conexão do host.";
            return;
        }

        string code = LobbyManager.Instance.CreateRoom(hostConn);
        roomCodeText.text = "Código da sala: " + code;
        feedbackText.text = "Sala criada!";

        PlayerListUpdate updateMsg = new PlayerListUpdate
        {
            playerIds = LobbyManager.Instance.GetRoom(code)
                .players
                .Select(p => $"Player {p.ClientId}")
                .ToList()
        };
        playerListText.text = string.Join("\n", updateMsg.playerIds);
    }

    #endregion

    #region Start Client

    public void ConnectToHost()
    {
        string hostIP = ipInput.text.Trim();
        if (string.IsNullOrEmpty(hostIP))
        {
            feedbackText.text = "Insira o IP do host!";
            return;
        }

        feedbackText.text = "A conectar ao host...";
        InstanceFinder.ClientManager.StartConnection(hostIP, port);
    }

    void OnCodeTyping(string text)
    {
        if (text.Length == 6)
        {
            JoinRoom();
        }
    }

    public void JoinRoom()
    {
        currentRoomCode = joinInput.text.Trim();
        feedbackText.text = "A entrar...";
        InstanceFinder.ClientManager.Broadcast(new JoinRoomRequest { code = currentRoomCode });
    }

    #endregion

    #region Utilitys

    private void RegisterClientMessages()
    {
        var client = InstanceFinder.ClientManager;

        client.RegisterBroadcast<CreateRoomResponse>((msg, channel) =>
        {
            currentRoomCode = msg.code;
            roomCodeText.text = "Código da sala: " + msg.code;
            feedbackText.text = "Sala criada!";
            UpdatePlayerList();
        });

        client.RegisterBroadcast<JoinRoomResponse>((msg, channel) =>
        {
            feedbackText.text = msg.success ? "Entrou na sala!" : "Código inválido!";
            if (msg.success) joinInput.text = "";
            lobby.SetActive(false);
            room.SetActive(true);
            play.SetActive(false);
            UpdatePlayerList();
        });

        client.RegisterBroadcast<PlayerListUpdate>((msg, channel) =>
        {
            playerListText.text = string.Join("\n", msg.playerIds);
        });
    }

    private void OnClientConnectionStateChanged(ClientConnectionStateArgs args)
    {
        switch (args.ConnectionState)
        {
            case LocalConnectionState.Starting:
                feedbackText.text = "Conectando ao host...";
                break;
            case LocalConnectionState.Started:
                feedbackText.text = "Conectado ao host!";
                ip.SetActive(false);
                createRoom.SetActive(false);
                joinRoom.SetActive(true);
                break;
            case LocalConnectionState.Stopping:
                feedbackText.text = "Desconectando...";
                break;
            case LocalConnectionState.Stopped:
                feedbackText.text = "Desconectado do host!";
                break;
        }
    }

    private void UpdatePlayerList()
    {
        if (string.IsNullOrEmpty(currentRoomCode)) return;
        playerListText.text = LobbyManager.Instance.GetPlayerIds(currentRoomCode);
    }

    #endregion
}