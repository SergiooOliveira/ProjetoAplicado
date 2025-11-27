using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
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

    [Header("IP Settings")]
    public TMP_InputField ipInput;
    public ushort port = 7777;

    private string currentRoomCode;

    private void Awake()
    {
        // Garantir que listeners estão no main thread
        var client = InstanceFinder.ClientManager;

        client.OnClientConnectionState += OnClientConnectionStateChanged;
        InstanceFinder.ServerManager.OnServerConnectionState += OnServerStarted;

        InstanceFinder.ServerManager.OnServerConnectionState += (args) =>
        {
            Debug.Log($"[SERVER] State aa: {args.ConnectionState}");
        };

        InstanceFinder.ClientManager.OnClientConnectionState += (args) =>
        {
            Debug.Log($"[CLIENT] State bb: {args.ConnectionState}");
        };

        RegisterClientMessages();
    }

    private void Start()
    {
        // Se este cliente for host local (rodando server + client), passamos a conexão do client.
        StartCoroutine(CheckLocalHostConnection());
    }

    private IEnumerator CheckLocalHostConnection()
    {
        yield return null; // Espera a scene carregar

        NetworkConnection myClientConn = InstanceFinder.ClientManager.Connection;
        if (myClientConn != null && LobbyManager.Instance.IsHost(myClientConn))
        {
            CreateRoomUIForHost(myClientConn);
        }
    }

    private void OnServerStarted(ServerConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            StartCoroutine(HandleHostConnection());
        }
    }

    private IEnumerator HandleHostConnection()
    {
        yield return null; // Espera a scene carregar
        Debug.Log("Servidor iniciado. Procurando conexão do host...");

        NetworkConnection hostConn = InstanceFinder.ServerManager.Clients.Values.FirstOrDefault();
        if (hostConn != null)
        {
            CreateRoomUIForHost(hostConn);
        }
        else
        {
            Debug.LogWarning("Nenhum host conectado ainda.");
        }
    }

    public void StartAsHost()
    {
        // Start server
        InstanceFinder.ServerManager.StartConnection();

        // Start LOCAL client usando localhost
        StartCoroutine(StartLocalClient());
    }

    private IEnumerator StartLocalClient()
    {
        yield return null; // Espera a scene carregar

        InstanceFinder.ClientManager.StartConnection("localhost", port);
        Debug.Log("Host iniciado!");
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

        StartCoroutine(ConnectClientCoroutine(hostIP));
    }

    private IEnumerator ConnectClientCoroutine(string hostIP)
    {
        yield return null; // Garante main thread e scene carregada

        feedbackText.text = "A conectar ao host...";
        Debug.Log($"A conectar ao host: {hostIP}");

        var tugboat = InstanceFinder.TransportManager.Transport as FishNet.Transporting.Tugboat.Tugboat;
        //tugboat.SetClientAddress(hostIP);

        // Corrigido: passar hostIP e port
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

    public void JoinRoom()
    {
        if (string.IsNullOrWhiteSpace(joinInput.text))
        {
            feedbackText.text = "Insira um código!";
            return;
        }

        currentRoomCode = joinInput.text.Trim();
        feedbackText.text = "A entrar...";
        InstanceFinder.ClientManager.Broadcast(new JoinRoomRequest { code = currentRoomCode });
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

    private void UpdatePlayerList()
    {
        if (string.IsNullOrEmpty(currentRoomCode)) return;
        playerListText.text = LobbyManager.Instance.GetPlayerIds(currentRoomCode);
    }
}