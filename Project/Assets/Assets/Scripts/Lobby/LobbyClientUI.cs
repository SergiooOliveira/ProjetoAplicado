using FishNet;
using UnityEngine;
using TMPro;

public class LobbyClientUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text roomCodeText;
    public TMP_InputField joinInput;
    public TMP_Text feedbackText;

    private bool registered = false;

    private void Awake()
    {
        // Evita dupla inscrição de callbacks ao carregar a UI mais de uma vez
        if (!registered)
        {
            RegisterClientMessages();
            registered = true;
        }
    }

    private void Start()
    {
        // Se for host
        if (LobbyManager.Instance.IsHost(InstanceFinder.ClientManager.Connection))
        {
            CreateRoom(); // cria sala automaticamente
        }
    }

    private void RegisterClientMessages()
    {
        var client = InstanceFinder.ClientManager;

        // Resposta ao criar sala
        client.RegisterBroadcast<CreateRoomResponse>((msg, channel) =>
        {
            roomCodeText.text = "Código da sala: " + msg.code;
            feedbackText.text = "Sala criada!";
        });

        // Resposta ao entrar na sala
        client.RegisterBroadcast<JoinRoomResponse>((msg, channel) =>
        {
            if (msg.success)
            {
                feedbackText.text = "Entrou na sala!";
                joinInput.text = "";
            }
            else
            {
                feedbackText.text = "Código inválido!";
            }
        });
    }

    public void CreateRoom()
    {
        feedbackText.text = "A criar sala...";
        InstanceFinder.ClientManager.Broadcast(new CreateRoomRequest());
    }

    public void JoinRoom()
    {
        if (string.IsNullOrWhiteSpace(joinInput.text))
        {
            feedbackText.text = "Insira um código!";
            return;
        }

        feedbackText.text = "A entrar...";
        InstanceFinder.ClientManager.Broadcast(new JoinRoomRequest { code = joinInput.text });
    }
}