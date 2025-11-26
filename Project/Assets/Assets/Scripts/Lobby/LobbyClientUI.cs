using FishNet;
using UnityEngine;
using TMPro;

public class LobbyClientUI : MonoBehaviour
{
    public TMP_Text roomCodeText;
    public TMP_InputField joinInput;

    public void CreateRoom()
    {
        InstanceFinder.ClientManager.Broadcast(new CreateRoomRequest());
    }

    public void JoinRoom()
    {
        InstanceFinder.ClientManager.Broadcast(new JoinRoomRequest { code = joinInput.text });
    }

    private void Start()
    {
        var client = InstanceFinder.ClientManager;

        client.RegisterBroadcast<CreateRoomResponse>(msg =>
        {
            roomCodeText.text = "Código da sala: " + msg.code;
        });

        client.RegisterBroadcast<JoinRoomResponse>(msg =>
        {
            if (msg.success)
                Debug.Log("Entrou na sala!");
            else
                Debug.Log("Código inválido!");
        });
    }
}