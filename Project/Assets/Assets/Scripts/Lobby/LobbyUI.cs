using Photon.Realtime;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interface do Lobby
/// </summary>
public class LobbyUI : MonoBehaviour
{
    [Header("Paineis")]
    [SerializeField] private GameObject connectionPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject errorPanel;

    [Header("Connection Panel")]
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private Button connectButton;

    [Header("Lobby Panel - Botões")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button refreshRoomsButton;

    [Header("Lobby Panel - Inputs")]
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private TMP_InputField joinRoomNameInput;

    [Header("Lobby Panel - Lista de Salas")]
    [SerializeField] private Transform roomListContent;
    [SerializeField] private GameObject roomListItemPrefab;

    [Header("Room Panel")]
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private Transform playerListContent;
    [SerializeField] private GameObject playerListItemPrefab;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button leaveRoomButton;

    [Header("Error Panel")]
    [SerializeField] private TextMeshProUGUI errorMessageText;
    [SerializeField] private Button closeErrorButton;

    [Header("Loading")]
    [SerializeField] private TextMeshProUGUI loadingText;

    private LobbyManager lobbyManager;
    private List<GameObject> roomListItems = new List<GameObject>();
    private List<GameObject> playerListItems = new List<GameObject>();

 
    
}