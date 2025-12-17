using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

/// <summary>
/// Representa um item na lista de salas
/// </summary>
public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private Button joinButton;

    private string roomName;
    private System.Action<string> onJoinCallback;

    public void Setup(RoomInfo roomInfo, System.Action<string> joinCallback)
    {
        roomName = roomInfo.Name;
        onJoinCallback = joinCallback;

        // Atualiza textos
        if (roomNameText != null)
            roomNameText.text = roomInfo.Name;

        if (playerCountText != null)
            playerCountText.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";

        // Configura bot�o
        if (joinButton != null)
        {
            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(OnJoinButtonClicked);

            // Desativa se a sala estiver cheia
            joinButton.interactable = roomInfo.PlayerCount < roomInfo.MaxPlayers;
        }
    }

    private void OnJoinButtonClicked()
    {
        onJoinCallback?.Invoke(roomName);
    }
}
