using FishNet;
using FishNet.Connection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    #region Fields

    public static LobbyManager Instance;

    private Dictionary<string, LobbyRoom> rooms = new();

    #endregion

    #region Unity Callbacks

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

    #endregion

    #region Room

    public string CreateRoom(NetworkConnection hostConn)
    {
        string code = GenerateCode(6);
        rooms[code] = new LobbyRoom
        {
            host = hostConn,
            players = new List<NetworkConnection> { hostConn }
        };

        // Send list update as soon as the room is created
        SendPlayerListUpdate(code);
        return code;
    }

    private string GenerateCode(int len)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string code = "";
        for (int i = 0; i < len; i++)
            code += chars[Random.Range(0, chars.Length)];
        return code;
    }

    public bool TryJoinRoom(string code, NetworkConnection conn)
    {
        if (!rooms.ContainsKey(code))
            return false;

        rooms[code].players.Add(conn);

        // Send the updated player list to everyone
        SendPlayerListUpdate(code);
        return true;
    }

    public LobbyRoom GetRoom(string code)
    {
        rooms.TryGetValue(code, out LobbyRoom room);
        return room;
    }

    #endregion

    #region Player List

    public string GetPlayerIds(string code)
    {
        if (!rooms.ContainsKey(code)) return "";
        return string.Join("\n", rooms[code].players.Select(p => $"Player {p.ClientId}"));
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

    public void RemovePlayer(NetworkConnection conn)
    {
        foreach (var room in rooms.Values)
        {
            if (room.players.Contains(conn))
            {
                room.players.Remove(conn);
                SendPlayerListUpdate(room.Code); // update list for everyone
                break;
            }
        }
    }

    #endregion
}