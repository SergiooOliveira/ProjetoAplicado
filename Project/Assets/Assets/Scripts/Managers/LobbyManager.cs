using FishNet;
using FishNet.Connection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    private Dictionary<string, LobbyRoom> rooms = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string CreateRoom(NetworkConnection hostConn)
    {
        string code = GenerateCode(6);
        rooms[code] = new LobbyRoom
        {
            host = hostConn,
            players = new List<NetworkConnection> { hostConn }
        };

        // Envia atualização da lista logo que a sala é criada
        SendPlayerListUpdate(code);
        return code;
    }

    public bool TryJoinRoom(string code, NetworkConnection conn)
    {
        if (!rooms.ContainsKey(code))
            return false;

        rooms[code].players.Add(conn);

        // Envia a lista de players atualizada para todos
        SendPlayerListUpdate(code);
        return true;
    }

    public void RemovePlayer(NetworkConnection conn)
    {
        foreach (var room in rooms.Values)
        {
            if (room.players.Contains(conn))
            {
                room.players.Remove(conn);
                SendPlayerListUpdate(room.Code); // atualiza lista para todos
                break;
            }
        }
    }

    public LobbyRoom GetRoom(string code)
    {
        rooms.TryGetValue(code, out LobbyRoom room);
        return room;
    }

    public bool IsHost(NetworkConnection conn)
    {
        foreach (var room in rooms.Values)
        {
            if (room.host == conn)
                return true;
        }
        return false;
    }

    private void SendPlayerListUpdate(string code)
    {
        if (!rooms.ContainsKey(code)) return;

        PlayerListUpdate updateMsg = new PlayerListUpdate
        {
            playerIds = rooms[code].players.Select(p => $"Player {p.ClientId}").ToList()
        };

        foreach (var playerConn in rooms[code].players)
        {
            InstanceFinder.ServerManager.Broadcast(playerConn, updateMsg);
        }
    }

    public string GetPlayerIds(string code)
    {
        if (!rooms.ContainsKey(code)) return "";
        return string.Join("\n", rooms[code].players.Select(p => $"Player {p.ClientId}"));
    }

    private string GenerateCode(int len)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string code = "";
        for (int i = 0; i < len; i++)
            code += chars[Random.Range(0, chars.Length)];
        return code;
    }
}

public class LobbyRoom
{
    public string Code;
    public NetworkConnection host;
    public List<NetworkConnection> players;
}
