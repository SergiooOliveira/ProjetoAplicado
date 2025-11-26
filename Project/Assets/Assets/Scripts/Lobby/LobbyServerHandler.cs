using FishNet;
using FishNet.Managing.Server;
using FishNet.Connection;
using FishNet.Transporting;
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
        conn.Broadcast(new JoinRoomResponse { success = ok });
    }
}