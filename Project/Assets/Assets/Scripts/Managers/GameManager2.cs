using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Gere o jogo multiplayer
/// </summary>
public class GameManager2 : MonoBehaviourPunCallbacks
{
    // Singleton
    public static GameManager2 Instance { get; private set; }

    [Header("Configurações")]
    [SerializeField] private string lobbySceneName = "Lobby";

    [Header("Referências")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject playerPrefab;
    // [SerializeField] private GameUI gameUI;


    public CustomPrefabPool customPrefabPool;



    private GameObject localPlayer;

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
        // Make sure Photon uses your custom prefab pool
        PhotonNetwork.PrefabPool = customPrefabPool;


        
        UnityEngine.Debug.Log("=== GAME MANAGER START ===");
        UnityEngine.Debug.Log($"PhotonNetwork.IsConnected: {PhotonNetwork.IsConnected}");
        UnityEngine.Debug.Log($"PhotonNetwork.InRoom: {PhotonNetwork.InRoom}");
        UnityEngine.Debug.Log($"PhotonNetwork.NickName: {PhotonNetwork.NickName}");

        //ESPERA UM POUCO ANTES DE VERIFICAR
        StartCoroutine(InitializeWithDelay());
    }

    /// <summary>
    /// Espera um pouco para garantir que o Photon está sincronizado
    /// </summary>
    private IEnumerator InitializeWithDelay()
    {
        UnityEngine.Debug.Log("Aguardar para a inicialização...");

        yield return null;

        //PROCURA AUTOMATICAMENTE
        // if (gameUI == null)
        // {
        //     gameUI = FindObjectOfType<GameUI>();

        //     if (gameUI != null)
        //     {
        //         UnityEngine.Debug.Log("GameUI encontrado automaticamente!");
        //     }
        //     else
        //     {
        //         UnityEngine.Debug.LogWarning("GameUI não encontrado");
        //     }
        // }

        UnityEngine.Debug.Log("GameManager inicializado!");


        UnityEngine.Debug.Log("A esperar pela sincronização do Photon...");

        // Espera 0.5 segundos
        yield return new WaitForSeconds(0.5f);

        UnityEngine.Debug.Log("=== APÓS DELAY ===");
        UnityEngine.Debug.Log($"PhotonNetwork.IsConnected: {PhotonNetwork.IsConnected}");
        UnityEngine.Debug.Log($"PhotonNetwork.InRoom: {PhotonNetwork.InRoom}");

        // VERIFICA SE ESTÁ LIGADO
        if (!PhotonNetwork.IsConnected)
        {
            UnityEngine.Debug.LogError("Não está ligado ao Photon! Voltando ao Lobby...");
            SceneManager.LoadScene(lobbySceneName);
            yield break;
        }

        //VERIFICA SE ESTÁ NUMA SALA
        if (!PhotonNetwork.InRoom)
        {
            UnityEngine.Debug.LogError("Não está numa sala! Voltar ao Lobby...");
            SceneManager.LoadScene(lobbySceneName);
            yield break;
        }

        UnityEngine.Debug.Log("GameManager inicializado com sucesso!");
        UnityEngine.Debug.Log($"Sala: {PhotonNetwork.CurrentRoom.Name}");
        UnityEngine.Debug.Log($"Jogadores: {PhotonNetwork.CurrentRoom.PlayerCount}");

        // VERIFICA SE O GAMEUI ESTÁ ATRIBUÍDO
        // if (gameUI == null)
        // {
        //     gameUI = FindObjectOfType<GameUI>();
        //     if (gameUI == null)
        //     {
        //         UnityEngine.Debug.LogError("GameUI não encontrado!");
        //     }
        //     else
        //     {
        //         UnityEngine.Debug.Log("GameUI encontrado automaticamente");
        //     }
        // }

        //SPAWN DO JOGADOR
        SpawnPlayer();
    }

    #endregion

    #region Spawn do Jogador

    /// <summary>
    /// Cria o jogador na rede
    /// </summary>
    private void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            UnityEngine.Debug.LogError("Player Prefab não está atribuído!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            UnityEngine.Debug.LogError("Spawn Points não estão configurados!");
            return;
        }

        // Escolhe um spawn point aleatório
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        Vector3 spawnPosition = spawnPoint.position;
        Quaternion spawnRotation = spawnPoint.rotation;

        // UnityEngine.Debug.Log($"A criar jogador em: {spawnPosition}");

        // Cria o jogador na rede
        localPlayer = PhotonNetwork.Instantiate(
            playerPrefab.name,
            spawnPosition,
            spawnRotation
        );

        // UnityEngine.Debug.Log($"Jogador criado: {localPlayer.name}");

        PhotonView playerPhotonView = localPlayer.GetComponent<PhotonView>();
        
        if (playerPhotonView.IsMine)
        {
            // localPlayer.SetActive(true);

            // Enable PlayerInput for the local player (owner)
            PlayerInput playerInput = localPlayer.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.enabled = true;  // Enable the PlayerInput component
                // UnityEngine.Debug.Log("PlayerInput enabled for local player.");
            }
            else
            {
                UnityEngine.Debug.LogError("PlayerInput component not found on player prefab!");
            }
        }

            
    }

    #endregion

    #region Callbacks do Photon

    // public override void OnPlayerEnteredRoom(Player newPlayer)
    // {
    //     UnityEngine.Debug.Log($"{newPlayer.NickName} entrou no jogo");

    //     if (gameUI != null)
    //         gameUI.UpdatePlayerList();
    // }

    // public override void OnPlayerLeftRoom(Player otherPlayer)
    // {
    //     UnityEngine.Debug.Log($"{otherPlayer.NickName} saiu do jogo");

    //     if (gameUI != null)
    //         gameUI.UpdatePlayerList();
    // }

    public override void OnLeftRoom()
    {
        UnityEngine.Debug.Log("Saiu da sala, voltar ao Lobby...");
        SceneManager.LoadScene(lobbySceneName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.Debug.LogWarning($"Desconectado: {cause}");
        SceneManager.LoadScene(lobbySceneName);
    }

    #endregion

    #region Métodos Públicos

    /// <summary>
    /// Sai do jogo e volta ao Lobby
    /// </summary>
    public void LeaveGame()
    {
        UnityEngine.Debug.Log("A sair do jogo...");
        PhotonNetwork.LeaveRoom();
    }

    #endregion
}