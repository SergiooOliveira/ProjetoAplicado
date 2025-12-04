using FishNet;
using FishNet.Connection;
using FishNet.Example;
using FishNet.Transporting;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class LobbyClientUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text roomCodeText;
    public TMP_InputField joinInput;
    public TMP_Text feedbackText;
    public TMP_Text playerListText;
    public GameObject lobby;
    public GameObject room;

    [Header("IP Settings")]
    public TMP_InputField ipInput;
    public ushort port = 7777;
    public HostDiscovery hostDiscovery;
    public ClientDiscovery discovery;

    private string currentRoomCode;
    private string pendingJoinCode;

    private void Awake()
    {
        var client = InstanceFinder.ClientManager;
        var server = InstanceFinder.ServerManager;

        client.OnClientConnectionState += OnClientConnectionStateChanged;

        // Notificação de desconexão de qualquer client (no host)
        server.OnRemoteConnectionState += (conn, args) =>
        {
            if (args.ConnectionState == RemoteConnectionState.Stopped)
            {
                LobbyManager.Instance.RemovePlayer(conn);
            }
        };

        RegisterClientMessages();
    }

    void Start()
    {
        joinInput.onValueChanged.AddListener(OnCodeTyping);
    }

    public void StartAsHost()
    {
        // 1. Start server
        InstanceFinder.ServerManager.StartConnection();

        // 2. Start LOCAL client usando localhost
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
            UpdatePlayerList();
        });

        client.RegisterBroadcast<PlayerListUpdate>((msg, channel) =>
        {
            playerListText.text = string.Join("\n", msg.playerIds);
        });
    }

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

    private void OnClientConnectionStateChanged(ClientConnectionStateArgs args)
    {
        switch (args.ConnectionState)
        {
            case LocalConnectionState.Starting:
                feedbackText.text = "Conectando ao host...";
                break;
            case LocalConnectionState.Started:
                feedbackText.text = "Conectado ao host!";
                Debug.Log("Cliente conectado com sucesso!");

                // se temos um código pendente, envia JoinRoom automaticamente
                if (!string.IsNullOrEmpty(pendingJoinCode))
                {
                    InstanceFinder.ClientManager.Broadcast(
                        new JoinRoomRequest { code = pendingJoinCode }
                    );

                    pendingJoinCode = null;
                }
                break;
            case LocalConnectionState.Stopping:
                feedbackText.text = "Desconectando...";
                break;
            case LocalConnectionState.Stopped:
                feedbackText.text = "Desconectado do host!";
                Debug.Log("Cliente desconectado!");
                break;
        }
    }

    void OnCodeTyping(string text)
    {
        if (text.Length == 6)
        {
            JoinRoom();
        }
    }

    //public void JoinRoom()
    //{
    //    currentRoomCode = joinInput.text.Trim();
    //    feedbackText.text = "A entrar...";
    //    InstanceFinder.ClientManager.Broadcast(new JoinRoomRequest { code = currentRoomCode });
    //}

    public void JoinRoom()
    {
        string code = joinInput.text.Trim();

        feedbackText.text = "Procurando sala na LAN...";
        StartCoroutine(WaitForRoomAndJoin(code));
    }

    private IEnumerator WaitForRoomAndJoin(string code)
    {
        float timeout = 5f;
        float timer = 0f;

        while (timer < timeout)
        {
            if (discovery.TryGetRoom(code, out string ip, out int foundPort))
            {
                feedbackText.text = "A conectar ao host...";
                pendingJoinCode = code;
                currentRoomCode = code;

                Debug.Log($"[LobbyClientUI] IP do host encontrado: {ip} / Porta: {foundPort}");
                InstanceFinder.ClientManager.StartConnection(ip, (ushort)foundPort);
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        feedbackText.text = "Nenhuma sala LAN encontrada com esse código!";
        Debug.Log($"[LobbyClientUI] Sala NÃO encontrada após {timeout} segundos: {code}");
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

        hostDiscovery.StartBroadcasting(code);

        PlayerListUpdate updateMsg = new PlayerListUpdate
        {
            playerIds = LobbyManager.Instance.GetRoom(code)
                .players
                .Select(p => $"Player {p.ClientId}")
                .ToList()
        };
        playerListText.text = string.Join("\n", updateMsg.playerIds);
    }

    private void UpdatePlayerList()
    {
        if (string.IsNullOrEmpty(currentRoomCode)) return;
        playerListText.text = LobbyManager.Instance.GetPlayerIds(currentRoomCode);
    }
}