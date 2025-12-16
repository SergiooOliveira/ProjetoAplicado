using UnityEngine;
using TMPro;

/// <summary>
/// Representa um item na lista de jogadores
/// </summary>
public class PlayerListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;

    public void Setup(string playerName, bool isLocalPlayer, bool isMasterClient)
    {
        if (playerNameText != null)
        {
            string displayName = playerName;

            if (isMasterClient)
                displayName += " [HOST]";

            if (isLocalPlayer)
                displayName += " (Tu)";

            playerNameText.text = displayName;
        }
    }
}