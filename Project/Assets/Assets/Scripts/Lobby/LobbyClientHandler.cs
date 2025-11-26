using FishNet;
using FishNet.Managing.Client;
using FishNet.Transporting;
using UnityEngine;

public class LobbyClientHandler : MonoBehaviour
{
    private void Start()
    {
        var client = InstanceFinder.ClientManager;

        client.RegisterBroadcast<CreateRoomResponse>(OnCreateRoomResponse);
        client.RegisterBroadcast<JoinRoomResponse>(OnJoinRoomResponse);
    }

    private void OnCreateRoomResponse(CreateRoomResponse msg, Channel channel)
    {
        Debug.Log("[CLIENT] Sala criada com código: " + msg.code);
        // TODO -> chama UI para mostrar código
    }

    private void OnJoinRoomResponse(JoinRoomResponse msg, Channel channel)
    {
        Debug.Log("[CLIENT] Join resultado: " + msg.success);
        // TODO -> UI atualiza
    }
}
