using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
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

    private void Start()
    {
        var client = InstanceFinder.ClientManager;

        client.OnClientConnectionState += OnClientConnectionStateChanged;
        InstanceFinder.ServerManager.OnServerConnectionState += OnServerStarted;

        RegisterClientMessages();

        // Se este cliente for host local (rodando server + client), passamos a conexão do client.
        // Em hosts, a Connection do ClientManager representa a conexão local do cliente.
        NetworkConnection myClientConn = InstanceFinder.ClientManager.Connection;
        if (myClientConn != null && LobbyManager.Instance.IsHost(myClientConn))
        {
            // Host cria a sala automaticamente — passamos a conexão do cliente (para identificação local)
            // Caso prefira a conexão server-side, CreateRoomUIForHost() também tenta recuperar server-side se receber null.
            CreateRoomUIForHost(myClientConn);
        }
    }

    private void OnServerStarted(ServerConnectionStateArgs args)
    {
        InstanceFinder.ServerManager.OnServerConnectionState += (args) =>
        {
            Debug.Log("Server Connection State: " + args.ConnectionState);
        };

        if (args.ConnectionState == LocalConnectionState.Started)
        {
            Debug.Log("Servidor iniciado. Procurando conexão do host...");

            // CORREÇÃO: pegar apenas os valores (NetworkConnection) da coleção Clients
            NetworkConnection hostConn = InstanceFinder.ServerManager.Clients.Values.FirstOrDefault();

            if (hostConn == null)
            {
                Debug.LogWarning("Nenhum host conectado ainda.");
                return;
            }

            CreateRoomUIForHost(hostConn);
        }
    }

    public void StartAsHost()
    {
        // Start server
        InstanceFinder.ServerManager.StartConnection();

        // Start LOCAL client using LOCALHOST (OBRIGATÓRIO)
        InstanceFinder.ClientManager.StartConnection("localhost", port);

        Debug.Log("Host iniciado!");
    }

    private string GetLocalIPAddress()
    {
        foreach (var ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
        {
            var ipProps = ni.GetIPProperties();

            foreach (var addr in ipProps.UnicastAddresses)
            {
                if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                    !addr.Address.ToString().StartsWith("169.254"))
                {
                    return addr.Address.ToString();
                }
            }
        }
        return "127.0.0.1";
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
            if (msg.success)
            {
                feedbackText.text = "Entrou na sala!";
                joinInput.text = "";
                UpdatePlayerList();
            }
            else
            {
                feedbackText.text = "Código inválido!";
            }
        });

        client.RegisterBroadcast<PlayerListUpdate>((msg, channel) =>
        {
            playerListText.text = string.Join("\n", msg.playerIds);
        });
    }

    public void CreateRoomAsHost()
    {
        // Pega a conexão do host automaticamente
        NetworkConnection hostConn = InstanceFinder.ClientManager.Connection;

        if (hostConn == null)
            hostConn = InstanceFinder.ServerManager.Clients.Values.FirstOrDefault();

        CreateRoomUIForHost(hostConn);
    }

    // Agora obrigamos a passagem de uma NetworkConnection.
    // Também tornamos a função tolerante a null (tenta resolver automaticamente).
    private void CreateRoomUIForHost(NetworkConnection hostConn)
    {
        // Se caller passou null, tentamos recuperar a conexão server-side (caso este processo seja host/server)
        if (hostConn == null)
        {
            hostConn = InstanceFinder.ServerManager.Clients.Values.FirstOrDefault();
        }

        if (hostConn == null)
        {
            Debug.LogWarning("Não foi possível obter a conexão do host para criar a sala.");
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

    public void ConnectToHost()
    {
        string hostIP = ipInput.text.Trim();
        if (string.IsNullOrEmpty(hostIP))
        {
            feedbackText.text = "Insira o IP do host!";
            return;
        }

        feedbackText.text = "A conectar ao host...";
        Debug.Log($"A conectar ao host: {hostIP}");
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

    private void UpdatePlayerList()
    {
        if (string.IsNullOrEmpty(currentRoomCode)) return;

        playerListText.text = LobbyManager.Instance.GetPlayerIds(currentRoomCode);
    }
}