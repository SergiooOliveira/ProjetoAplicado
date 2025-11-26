using FishNet.Connection;
using System.Collections.Generic;
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
        return code;
    }

    public bool TryJoinRoom(string code, NetworkConnection conn)
    {
        if (!rooms.ContainsKey(code))
            return false;

        rooms[code].players.Add(conn);
        return true;
    }

    public LobbyRoom GetRoom(string code)
    {
        if (rooms.TryGetValue(code, out LobbyRoom room))
            return room;
        return null;
    }

    private string GenerateCode(int len)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string code = "";
        for (int i = 0; i < len; i++)
            code += chars[Random.Range(0, chars.Length)];
        return code;
    }

    public string GetPlayerIds(string code)
    {
        if (!rooms.ContainsKey(code)) return "";
        string ids = "";
        foreach (var p in rooms[code].players)
            ids += $"Player {p.ClientId}\n";
        return ids;
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
}

public class LobbyRoom
{
    public NetworkConnection host;
    public List<NetworkConnection> players;
}