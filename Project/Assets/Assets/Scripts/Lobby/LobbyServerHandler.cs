using FishNet;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Transporting;
using System.Linq;
using UnityEngine;

public class LobbyServerHandler : MonoBehaviour
{
    private void Start()
    {
        var server = InstanceFinder.ServerManager;

        server.RegisterBroadcast<CreateRoomRequest>(OnCreateRoomRequested);
        server.RegisterBroadcast<JoinRoomRequest>(OnJoinRoomRequested);
    }

    private void OnCreateRoomRequested(NetworkConnection conn, CreateRoomRequest msg, Channel channel)
    {
        string code = LobbyManager.Instance.CreateRoom(conn);
        conn.Broadcast(new CreateRoomResponse { code = code });
    }

    private void OnJoinRoomRequested(NetworkConnection conn, JoinRoomRequest msg, Channel channel)
    {
        bool ok = LobbyManager.Instance.TryJoinRoom(msg.code, conn);

        // envia JoinRoomResponse para quem entrou
        conn.Broadcast(new JoinRoomResponse { success = ok }); // <- envia para o cliente específico

        // envia PlayerListUpdate para todos os players da sala
        LobbyRoom room = LobbyManager.Instance.GetRoom(msg.code);
        if (room != null)
        {
            PlayerListUpdate updateMsg = new PlayerListUpdate
            {
                playerIds = room.players.Select(p => $"Player {p.ClientId}").ToList()
            };

            foreach (var p in room.players)
            {
                p.Broadcast(updateMsg); // <- envia para cada client da sala
            }
        }
    }
}