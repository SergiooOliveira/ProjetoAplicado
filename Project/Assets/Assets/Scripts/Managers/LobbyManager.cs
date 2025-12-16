using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Gere toda a lógica do Lobby com Photon
/// </summary>
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Configurações")]
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private byte maxPlayersPerRoom = 4;

    [Header("UI")]
    [SerializeField] private LobbyUI lobbyUI;

    // Singleton
    public static LobbyManager Instance { get; private set; }

    // Cache de salas disponíveis
    private List<RoomInfo> availableRooms = new List<RoomInfo>();

    #region Inicialização

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UnityEngine.Debug.Log("=== LOBBY MANAGER START ===");

        //LIGAR AO PHOTON AUTOMATICAMENTE
        /*if (!PhotonNetwork.IsConnected)
        {
            UnityEngine.Debug.Log("A ligar ao Photon...");
            lobbyUI.ShowLoading("A ligar ao servidor...");
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            UnityEngine.Debug.Log("Já está ligado ao Photon");
            lobbyUI.ShowMainMenu();
        }*/

        // Atualiza a UI
        UpdateUI();
    }

    #endregion

    #region Métodos Privados de UI

    /// <summary>
    /// Atualiza toda a UI do Lobby
    /// </summary>
    private void UpdateUI()
    {
        if (PhotonNetwork.InRoom)
        {
            // UpdateRoomInfo();
        }
    }

    #endregion

    
}