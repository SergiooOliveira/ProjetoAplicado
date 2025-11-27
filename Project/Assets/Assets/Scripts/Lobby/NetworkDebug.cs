using FishNet;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using UnityEngine;

public class NetworkDebug : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("[DEBUG] NetworkDebug started.");

        InstanceFinder.ClientManager.OnClientConnectionState += (args) =>
        {
            Debug.Log($"[CLIENT] State: {args.ConnectionState}");
        };

        InstanceFinder.ServerManager.OnServerConnectionState += (args) =>
        {
            Debug.Log($"[SERVER] State: {args.ConnectionState}");
        };

        InstanceFinder.ServerManager.OnRemoteConnectionState += (conn, args) =>
        {
            Debug.Log($"[SERVER] Client {conn.ClientId} state: {args.ConnectionState}");
        };

        InstanceFinder.ClientManager.RegisterBroadcast<JoinRoomResponse>((msg, channel) =>
        {
            Debug.Log($"[CLIENT] JoinRoomResponse received: {msg.success}");
        });
    }
}
