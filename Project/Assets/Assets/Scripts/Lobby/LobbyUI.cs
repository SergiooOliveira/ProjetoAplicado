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

    #region Inicialização

    private void Start()
    {
        /*UnityEngine.Debug.Log("=== LOBBY UI START ===");

        //PROCURA O LOBBY MANAGER AUTOMATICAMENTE
        if (lobbyManager == null)
        {
            lobbyManager = FindObjectOfType<LobbyManager>();

            if (lobbyManager != null)
            {
                UnityEngine.Debug.Log("LobbyManager encontrado automaticamente!");
            }
            else
            {
                UnityEngine.Debug.LogError("LobbyManager NÃO ENCONTRADO!");
            }
        }

        SetupButtons();
        HideAllPanels();*/

   
        UnityEngine.Debug.Log("=== LOBBY UI START ===");

        // Procura o LobbyManager automaticamente
        if (lobbyManager == null)
        {
            lobbyManager = FindFirstObjectByType<LobbyManager>();

            if (lobbyManager != null)
            {
                UnityEngine.Debug.Log("✅ LobbyManager encontrado!");
            }
            else
            {
                UnityEngine.Debug.LogError("❌ LobbyManager NÃO ENCONTRADO!");
            }
        }

        // Configura os botões
        SetupButtons();

        // Mostra o painel de conexão
        ShowConnectionPanel();
   }



public void Initialize(LobbyManager manager)
    {
        lobbyManager = manager;
        SetupButtons();
        HideAllPanels();
    }

    private void SetupButtons()
    {
        UnityEngine.Debug.Log("=== SETUP BUTTONS ===");

        // Connection Panel
        if (connectButton != null)
            connectButton.onClick.AddListener(OnConnectButtonClicked);

        // Lobby Panel
        if (createRoomButton != null)
            createRoomButton.onClick.AddListener(OnCreateRoomButtonClicked);

        if (joinRoomButton != null)
            joinRoomButton.onClick.AddListener(OnJoinRoomButtonClicked);

        if (refreshRoomsButton != null)
            refreshRoomsButton.onClick.AddListener(OnRefreshButtonClicked);

        // Room Panel
        if (startGameButton != null)
            startGameButton.onClick.AddListener(OnStartGameButtonClicked);

        if (leaveRoomButton != null)
            leaveRoomButton.onClick.AddListener(OnLeaveRoomButtonClicked);

        // Error Panel
        if (closeErrorButton != null)
            closeErrorButton.onClick.AddListener(OnCloseErrorButtonClicked);
    }

    #endregion

    #region Mostrar/Esconder Painéis

    private void HideAllPanels()
    {
        if (connectionPanel != null) connectionPanel.SetActive(false);
        if (lobbyPanel != null) lobbyPanel.SetActive(false);
        if (roomPanel != null) roomPanel.SetActive(false);
        if (loadingPanel != null) loadingPanel.SetActive(false);
        if (errorPanel != null) errorPanel.SetActive(false);
    }

    /// <summary>
    /// Mostra o menu principal (Lobby Panel)
    /// </summary>
    public void ShowMainMenu()
    {
        HideAllPanels();
        if (lobbyPanel != null)
        {
            lobbyPanel.SetActive(true);
            UnityEngine.Debug.Log("Mostra o menu principal (Lobby Panel)");
        }
    }

    public void ShowConnectionPanel()
    {
        HideAllPanels();
        if (connectionPanel != null)
        {
            connectionPanel.SetActive(true);
            UnityEngine.Debug.Log("Mostra o painel de conexão");
        }
    }

    public void ShowLobbyPanel()
    {
        HideAllPanels();
        if (lobbyPanel != null)
        {
            lobbyPanel.SetActive(true);
            UnityEngine.Debug.Log("Mostra o painel do lobby");
        }
    }

    public void ShowRoomPanel()
    {
        HideAllPanels();
        if (roomPanel != null)
        {
            roomPanel.SetActive(true);
            UnityEngine.Debug.Log("Mostra o painel da sala");
        }
    }

    public void ShowLoading(string message)
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
            if (loadingText != null)
                loadingText.text = message;
            UnityEngine.Debug.Log($"Loading: {message}");
        }
    }

    public void HideLoading()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
            UnityEngine.Debug.Log("Loading escondido");
        }
    }

    public void ShowError(string message)
    {
        if (errorPanel != null)
        {
            errorPanel.SetActive(true);
            if (errorMessageText != null)
                errorMessageText.text = message;
            UnityEngine.Debug.LogWarning($"Erro mostrado: {message}");
        }
    }

    private void HideError()
    {
        if (errorPanel != null)
        {
            errorPanel.SetActive(false);
        }
    }

    #endregion

    #region Callbacks de Botões - Connection Panel

    private void OnConnectButtonClicked()
    {
        UnityEngine.Debug.Log("=== BOTÃO LIGAR CLICADO ===");

        string playerName = playerNameInput != null ? playerNameInput.text : "";

        UnityEngine.Debug.Log($"playerNameInput é null? {playerNameInput == null}");
        UnityEngine.Debug.Log($"Nome do jogador: '{playerName}'");
        UnityEngine.Debug.Log($"Nome está vazio? {string.IsNullOrEmpty(playerName)}");

        if (string.IsNullOrEmpty(playerName))
        {
            UnityEngine.Debug.LogWarning("NOME VAZIO! A mostrar erro...");
            ShowError("Por favor, insira um nome!");
            return;
        }

        UnityEngine.Debug.Log($"Nome válido! A tentar ligar como: {playerName}");

        if (lobbyManager != null)
        {
            UnityEngine.Debug.Log("LobbyManager encontrado! A chamar ConnectToPhoton...");
            lobbyManager.ConnectToPhoton(playerName);
        }
        else
        {
            UnityEngine.Debug.LogError("LobbyManager é NULL!");
        }
    }


    #endregion

    #region Callbacks de Botões - Lobby Panel

    private void OnCreateRoomButtonClicked()
    {
        UnityEngine.Debug.Log("BOTÃO CLICADO!");

        string roomName = roomNameInput != null ? roomNameInput.text : "";

        if (string.IsNullOrEmpty(roomName))
        {
            ShowError("Por favor, insira um nome para a sala!");
            return;
        }

        UnityEngine.Debug.Log($"Criar sala: {roomName}");

        if (lobbyManager != null)
        {
            lobbyManager.CreateRoom(roomName);
        }
    }

    private void OnJoinRoomButtonClicked()
    {
        string roomName = joinRoomNameInput != null ? joinRoomNameInput.text : "";

        if (string.IsNullOrEmpty(roomName))
        {
            ShowError("Por favor, insira o nome da sala!");
            return;
        }

        UnityEngine.Debug.Log($"A entrar na sala: {roomName}");

        if (lobbyManager != null)
        {
            lobbyManager.JoinRoom(roomName);
        }
    }

    private void OnJoinRandomButtonClicked()
    {
        UnityEngine.Debug.Log("Procurar sala aleatória...");

        if (lobbyManager != null)
        {
            lobbyManager.JoinRandomRoom();
        }
    }

    private void OnRefreshButtonClicked()
    {
        UnityEngine.Debug.Log("Atualizar lista de salas...");

        if (lobbyManager != null)
        {
            lobbyManager.RefreshRoomList();
        }
    }

    #endregion

    #region Callbacks de Botões - Room Panel

    private void OnStartGameButtonClicked()
    {
        UnityEngine.Debug.Log("Botão de iniciar jogo clicado");

        if (lobbyManager != null)
        {
            lobbyManager.StartGame();
        }
    }

    private void OnLeaveRoomButtonClicked()
    {
        UnityEngine.Debug.Log("A sair da sala...");

        if (lobbyManager != null)
        {
            lobbyManager.LeaveRoom();
        }
    }

    #endregion

    #region Callbacks de Botões - Error Panel

    private void OnCloseErrorButtonClicked()
    {
        HideError();
    }

    #endregion

    #region Atualizar Lista de Salas

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        UnityEngine.Debug.Log($"UpdateRoomList chamado com {roomList.Count} salas");

        // Limpa lista antiga
        ClearRoomList();

        // Cria novos itens
        foreach (RoomInfo room in roomList)
        {
            // Ignora salas que foram removidas ou invisíveis
            if (room.RemovedFromList)
            {
                UnityEngine.Debug.Log($"Sala {room.Name} ignorada (removida)");
                continue;
            }

            if (!room.IsVisible)
            {
                UnityEngine.Debug.Log($"Sala {room.Name} ignorada (invisível)");
                continue;
            }

            UnityEngine.Debug.Log($"A adicionar sala: {room.Name} ({room.PlayerCount}/{room.MaxPlayers})");

            // Cria item da sala
            GameObject item = Instantiate(roomListItemPrefab, roomListContent);
            roomListItems.Add(item);

            // Configura o item
            RoomListItem roomItem = item.GetComponent<RoomListItem>();
            if (roomItem != null)
            {
                roomItem.Setup(room, OnJoinRoomFromList);
            }
            else
            {
                // Fallback se não tiver o componente RoomListItem
                TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();
                if (texts.Length >= 2)
                {
                    texts[0].text = room.Name;
                    texts[1].text = $"{room.PlayerCount}/{room.MaxPlayers}";
                }

                Button button = item.GetComponentInChildren<Button>();
                if (button != null)
                {
                    string roomName = room.Name;
                    button.onClick.AddListener(() => OnJoinRoomFromList(roomName));
                }
            }
        }

        UnityEngine.Debug.Log($"Lista de salas atualizada! Total visível: {roomListItems.Count}");
    }


    private void OnJoinRoomFromList(string roomName)
    {
        UnityEngine.Debug.Log($"Entrar na sala da lista: {roomName}");

        if (lobbyManager != null)
        {
            lobbyManager.JoinRoom(roomName);
        }
    }

    private void ClearRoomList()
    {
        foreach (GameObject item in roomListItems)
        {
            Destroy(item);
        }
        roomListItems.Clear();
    }

    #endregion

    #region Atualizar Informações da Sala

    public void UpdateRoomInfo(string roomName, int currentPlayers, int maxPlayers)
    {
        if (roomNameText != null)
        {
            roomNameText.text = $"Sala: {roomName}";
        }

        if (playerCountText != null)
        {
            playerCountText.text = $"Jogadores: {currentPlayers}/{maxPlayers}";
        }

        // Ativa/desativa botão de iniciar jogo (só o host pode)
        if (startGameButton != null)
        {
            bool isMasterClient = lobbyManager != null && lobbyManager.IsMasterClient();
            startGameButton.interactable = isMasterClient;
        }
    }

    public void UpdatePlayerList(List<Photon.Realtime.Player> players)
    {
        // Limpa lista antiga
        ClearPlayerList();

        // Cria novos itens
        foreach (Photon.Realtime.Player player in players)
        {
            GameObject item = Instantiate(playerListItemPrefab, playerListContent);
            playerListItems.Add(item);

            // Configura o item
            PlayerListItem playerItem = item.GetComponent<PlayerListItem>();
            if (playerItem != null)
            {
                bool isLocalPlayer = lobbyManager != null && player.NickName == lobbyManager.GetPlayerName();
                bool isMasterClient = player.IsMasterClient;
                playerItem.Setup(player.NickName, isLocalPlayer, isMasterClient);
            }
            else
            {
                // Fallback se não tiver o componente PlayerListItem
                TextMeshProUGUI text = item.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    string displayName = player.NickName;
                    if (player.IsMasterClient)
                        displayName += " [HOST]";
                    if (lobbyManager != null && player.NickName == lobbyManager.GetPlayerName())
                        displayName += " (Tu)";

                    text.text = displayName;
                }
            }
        }

        UnityEngine.Debug.Log($"Lista de jogadores atualizada: {playerListItems.Count} jogadores");
    }

    private void ClearPlayerList()
    {
        foreach (GameObject item in playerListItems)
        {
            Destroy(item);
        }
        playerListItems.Clear();
    }

    #endregion
}