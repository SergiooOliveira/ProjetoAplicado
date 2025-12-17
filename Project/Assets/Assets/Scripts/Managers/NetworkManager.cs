using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Gere a ligação a base com o Photon (persiste entre cenas)
/// </summary>
public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Singleton
    public static NetworkManager Instance { get; private set; }

    [Header("Configurações Photon")]
    [SerializeField] private string gameVersion = "1.0";

    #region Inicialização

    private void Awake()
    {
        //SINGLETON - Garante que só existe 1 instância
        if (Instance != null && Instance != this)
        {
            UnityEngine.Debug.LogWarning("NetworkManager duplicado! Destruir...");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //CONFIGURAÇÕES CRÍTICAS DO PHOTON
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;

        UnityEngine.Debug.Log("NetworkManager inicializado e persistente");
        UnityEngine.Debug.Log($"AutomaticallySyncScene: {PhotonNetwork.AutomaticallySyncScene}");
    }

    private void Start()
    {
        UnityEngine.Debug.Log($"Photon App Version: {PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion}");
        UnityEngine.Debug.Log($"Game Version: {gameVersion}");
        UnityEngine.Debug.Log($"Ligado: {PhotonNetwork.IsConnected}");
        UnityEngine.Debug.Log($"Na sala: {PhotonNetwork.InRoom}");
    }

    #endregion

    #region Callbacks do Photon

    public override void OnConnectedToMaster()
    {
        UnityEngine.Debug.Log("NetworkManager: ligado ao Master Server");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.Debug.LogWarning($"NetworkManager: Desligado - {cause}");
    }

    public override void OnJoinedLobby()
    {
        UnityEngine.Debug.Log("NetworkManager: Entrou no Lobby");
    }

    public override void OnLeftLobby()
    {
        UnityEngine.Debug.Log("NetworkManager: Saiu do Lobby");
    }

    public override void OnJoinedRoom()
    {
        UnityEngine.Debug.Log($"NetworkManager: Entrou na sala '{PhotonNetwork.CurrentRoom.Name}'");
        UnityEngine.Debug.Log($"Jogadores: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}");
    }

    public override void OnLeftRoom()
    {
        UnityEngine.Debug.Log("NetworkManager: Saiu da sala");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UnityEngine.Debug.Log($"NetworkManager: {newPlayer.NickName} entrou na sala");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UnityEngine.Debug.Log($"NetworkManager: {otherPlayer.NickName} saiu da sala");
    }

    #endregion

    #region Métodos Públicos

    /// <summary>
    /// Conecta ao Photon
    /// </summary>
    public void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            UnityEngine.Debug.Log("NetworkManager: A ligar ao Photon...");
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            UnityEngine.Debug.Log("NetworkManager: Já está ligado");
        }
    }

    /// <summary>
    /// Desconecta do Photon
    /// </summary>
    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            UnityEngine.Debug.Log("NetworkManager: A desconectar...");
            PhotonNetwork.Disconnect();
        }
    }

    /// <summary>
    /// Verifica se está ligado
    /// </summary>
    public bool IsConnected()
    {
        return PhotonNetwork.IsConnected;
    }

    /// <summary>
    /// Verifica se está numa sala
    /// </summary>
    public bool IsInRoom()
    {
        return PhotonNetwork.InRoom;
    }

    /// <summary>
    /// Obtém o nome do jogador
    /// </summary>
    public string GetPlayerName()
    {
        return PhotonNetwork.NickName;
    }

    #endregion

    #region Debug

    private void Update()
    {
        // MOSTRA O ESTADO A CADA 5 SEGUNDOS
        if (Time.frameCount % 300 == 0) // ~5 segundos a 60 FPS
        {
            // UnityEngine.Debug.Log($"[NetworkManager] ligado: {PhotonNetwork.IsConnected} | Na sala: {PhotonNetwork.InRoom}");
        }
    }

    #endregion
}