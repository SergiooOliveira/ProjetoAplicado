using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TSceneManager : MonoBehaviour
{
    #region Serialized Fields

    public static TSceneManager Instance;

    [Header("Player Spawner")]
    public PlayerSpawner spawner;
    private static bool startMenuLoadedOnce = false;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InstanceFinder.SceneManager.OnLoadEnd += OnScenesLoaded;
    }

    private void Start()
    {
        if (spawner == null)
            spawner = GameObject.FindFirstObjectByType<PlayerSpawner>();

        if (spawner == null)
            Debug.LogError("PlayerSpawner não encontrado na cena!");

        // Only loads the StartMenu once
        if (!startMenuLoadedOnce)
        {
            startMenuLoadedOnce = true;
            StartCoroutine(WaitServerAndMaybeLoadStartMenu());
        }
    }

    private void OnDestroy()
    {
        if (InstanceFinder.SceneManager != null)
            InstanceFinder.SceneManager.OnLoadEnd -= OnScenesLoaded;
    }

    #endregion

    #region Load StartMenu

    private IEnumerator WaitServerAndMaybeLoadStartMenu()
    {
        // Wait until the Server is started (avoid using IsServer at startup)
        Debug.Log("[TSceneManager] Aguardando servidor iniciar...");
        yield return new WaitUntil(() => InstanceFinder.IsServerStarted);


        // If StartMenu is already loaded, it does nothing
        if (UnityEngine.SceneManagement.SceneManager.GetSceneByName("StartMenu").isLoaded)
        {
            Debug.Log("[TSceneManager] StartMenu já está carregada - não irá recarregar.");
            yield break;
        }

        // Checks if there is already a map scene loaded (in addition to PersistentScene and StartMenu)
        bool anyMapLoaded = false;
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            var s = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (s.name != "PersistentScene" && s.name != "StartMenu")
            {
                anyMapLoaded = true;
                break;
            }
        }

        if (!anyMapLoaded)
        {
            LoadMapByName("StartMenu");
        }
    }

    public void LoadMapByName(string sceneToLoad)
    {
        if (!InstanceFinder.IsServerStarted)
        {
            Debug.LogWarning("Apenas o servidor pode carregar cenas!");
            return;
        }

        // Unload all active scenes except PersistentScene
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene != "PersistentScene")
        {
            InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData(currentScene)); // ideal / correct but it doesn't work
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene); // TODO: you shouldn't use this but it's the only way that works
        }

        // Load the new scene
        SceneLoadData sld = new SceneLoadData(sceneToLoad);
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    #endregion

    #region Load Scenes

    private void OnScenesLoaded(SceneLoadEndEventArgs args)
    {
        if (!InstanceFinder.IsServerStarted)
            return;

        if (args.LoadedScenes == null || args.LoadedScenes.Count() == 0)
            return;

        string sceneName = args.LoadedScenes[0].name;

        // Não spawna no Loading/Lobby/StartMenu
        if (sceneName == "Loading" || sceneName == "StartMenu" || sceneName == "Lobby")
            return; // does nothing

        var newScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        if (newScene.IsValid())
        {
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(newScene);
            Debug.Log("[TSceneManager] ActiveScene set to: " + sceneName);
        }

        Debug.Log("[TSceneManager] Mapa carregado: " + sceneName);

        // Update spawn points
        SpawnPointMarker[] markers = GameObject.FindObjectsByType<SpawnPointMarker>(FindObjectsSortMode.None);
        spawner.spawnPoints = markers.Select(s => s.transform).ToArray();

        // Spawn players
        StartCoroutine(FinishLoadAndSpawn());
    }

    #endregion

    #region Load Scenes Using Loading

    public void LoadLoadingThenMap(string targetMap)
    {
        StartCoroutine(LoadLoadingThenMapRoutine(targetMap));
    }

    private IEnumerator LoadLoadingThenMapRoutine(string targetMap)
    {
        // 1. Descarrega StartMenu e Lobby, se estiverem carregadas
        string[] scenesToUnload = { "StartMenu", "Lobby" };
        foreach (var sceneName in scenesToUnload)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
            if (scene.isLoaded)
            {
                AsyncOperation unloadOp = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
                // Espera o unload completar
                yield return new WaitUntil(() => unloadOp.isDone);
            }
        }

        // 2. Carrega a cena de Loading
        SceneLoadData loadLoading = new SceneLoadData("Loading");
        //InstanceFinder.SceneManager.LoadGlobalScenes(loadLoading);
        InstanceFinder.SceneManager.LoadConnectionScenes(loadLoading);
        yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading").isLoaded);

        // 3. Carrega o mapa final
        SceneLoadData loadMap = new SceneLoadData(targetMap);
        //InstanceFinder.SceneManager.LoadGlobalScenes(loadMap);
        InstanceFinder.SceneManager.LoadConnectionScenes(loadMap);
        yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetSceneByName(targetMap).isLoaded);
    }

    #endregion

    #region Spawn Players

    private void MovePlayerToSpawnPoint(NetworkConnection conn)
    {
        var player = conn.FirstObject;
        if (player == null)
        {
            Debug.LogError("MovePlayerToSpawnPoint: player não encontrado!");
            return;
        }

        // If there are no spawnpoints on the map, nothing changes
        if (spawner.spawnPoints == null || spawner.spawnPoints.Length == 0)
        {
            Debug.LogWarning("Nenhum spawnpoint encontrado neste mapa!");
            return;
        }

        // For now let's choose a random spawn (same as PlayerSpawner)
        int index = Random.Range(0, spawner.spawnPoints.Length);
        Transform spawn = spawner.spawnPoints[index];

        player.transform.position = spawn.position;
        player.transform.rotation = spawn.rotation;

        Debug.Log($"Player movido para spawnpoint {index}: {spawn.position}");
    }

    private IEnumerator FinishLoadAndSpawn()
    {
        yield return null;

        // Update map spawnpoints
        spawner.spawnPoints = GameObject.FindObjectsByType<SpawnPointMarker>(FindObjectsSortMode.None)
                                         .Select(s => s.transform)
                                         .ToArray();

        foreach (NetworkConnection conn in InstanceFinder.ServerManager.Clients.Values)
        {
            // Create the player only if it doesn't already exist
            spawner.SpawnPlayer(conn);

            // Move player to map spawn
            MovePlayerToSpawnPoint(conn);
        }

        // Remove LOADING
        yield return new WaitForSeconds(0.1f);
        UnloadLoading();
    }

    private void UnloadLoading()
    {
        var loadingScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading");

        if (loadingScene.isLoaded)
        {
            InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Loading"));
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Loading");
            Debug.Log("[TSceneManager] Loading removida.");
        }
    }

    #endregion
}