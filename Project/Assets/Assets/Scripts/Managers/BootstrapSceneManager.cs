using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class BootstrapSceneManager : MonoBehaviour
{

    public static BootstrapSceneManager Instance { get; private set; }

    [Header("Player Spawner")]
    public PlayerSpawner spawner;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        LoadSceneLocal("StartMenu");

        if (spawner == null)
        {
            spawner = GameObject.FindFirstObjectByType<PlayerSpawner>();
            if (spawner == null)
                Debug.LogError("PlayerSpawner não encontrado na cena!");
        }

        InstanceFinder.SceneManager.OnLoadEnd += OnScenesLoaded;
    }

    //private void OnSceneLoadEnd(SceneLoadEndEventArgs args)
    //{
    //    foreach (var scene in args.LoadedScenes)
    //    {
    //        Ignora menu/ loading
    //        if (scene.name == "StartMenu" || scene.name == "Loading") continue;

    //        Tornar a nova cena ativa
    //        SceneManager.SetActiveScene(scene);

    //        Spawn players só no host
    //        if (InstanceFinder.IsServerStarted)
    //        {
    //            spawner.CaptureSpawnPointsFromScene();

    //            foreach (var conn in InstanceFinder.ServerManager.Clients.Values)
    //                spawner.SpawnPlayer(conn);

    //            StartCoroutine(RemoveLoadingAfterPlayersSpawned());
    //        }

    //        foreach (var conn in InstanceFinder.ClientManager.Clients.Values)
    //        {
    //            NetworkObject player = conn.FirstObject;
    //            if (player != null)
    //            {
    //                Scene persistentScene = SceneManager.GetSceneByName("PersistentScene");
    //                if (persistentScene.IsValid())
    //                    SceneManager.MoveGameObjectToScene(player.gameObject, persistentScene);
    //            }
    //        }

    //        // Remove Loading localmente no cliente
    //        var loadingScene = SceneManager.GetSceneByName("Loading");
    //        if (loadingScene.isLoaded)
    //        {
    //            // Unload localmente
    //            SceneManager.UnloadSceneAsync("Loading");
    //            // Se for multiplayer, também remove globalmente
    //            if (InstanceFinder.IsServerStarted)
    //                InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Loading"));

    //            Debug.Log("[BootstrapSceneManager] Loading removida após spawn dos players.");
    //        }
    //    }
    //}

    public void LoadSceneLocal(string sceneName)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public void UnloadSceneLocal(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!InstanceFinder.IsServerStarted)
            return;

        SceneLoadData sld = new SceneLoadData(sceneName);
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    public void UnloadScene(string sceneName)
    {
        if (!InstanceFinder.IsServerStarted)
            return;

        SceneUnloadData sld = new SceneUnloadData(sceneName);
        InstanceFinder.SceneManager.UnloadGlobalScenes(sld);
    }

    //public void LoadLoadingThenMap(string targetMap)
    //{
    //    if (!InstanceFinder.IsServerStarted) return;

    //    StartCoroutine(LoadLoadingThenMapRoutine(targetMap));
    //}

    //private IEnumerator LoadLoadingThenMapRoutine(string targetMap)
    //{
    //    // 1. Carrega Loading globalmente
    //    SceneLoadData loadLoading = new SceneLoadData("Loading");
    //    InstanceFinder.SceneManager.LoadGlobalScenes(loadLoading);

    //    yield return new WaitUntil(() => SceneManager.GetSceneByName("Loading").isLoaded);

    //    // 2. Carrega mapa globalmente
    //    SceneLoadData loadMap = new SceneLoadData(targetMap);
    //    InstanceFinder.SceneManager.LoadGlobalScenes(loadMap);

    //    yield return new WaitUntil(() => SceneManager.GetSceneByName(targetMap).isLoaded);
    //}

    //private IEnumerator RemoveLoadingAfterPlayersSpawned()
    //{
    //    // Espera que todos os clients tenham instanciado seus players
    //    yield return new WaitUntil(() =>
    //    {
    //        // Todos os clientes devem ter pelo menos 1 NetworkObject (player)
    //        return InstanceFinder.ClientManager.Clients.Values.All(c => c.FirstObject != null);
    //    });

    //    // Move players para PersistentScene (garante que host e clients fiquem na PersistentScene)
    //    foreach (var conn in InstanceFinder.ClientManager.Clients.Values)
    //    {
    //        NetworkObject player = conn.FirstObject;
    //        if (player != null)
    //        {
    //            Scene persistentScene = SceneManager.GetSceneByName("PersistentScene");
    //            if (persistentScene.IsValid())
    //                SceneManager.MoveGameObjectToScene(player.gameObject, persistentScene);
    //        }
    //    }


    //    InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Loading"));
    //    SceneManager.UnloadSceneAsync("Loading");

    //    // Remove Loading localmente
    //    var loadingScene = SceneManager.GetSceneByName("Loading");
    //    if (loadingScene.isLoaded)
    //    {
    //        // Unload localmente
    //        SceneManager.UnloadSceneAsync("Loading");
    //        // Se for multiplayer, também remove globalmente
    //        if (InstanceFinder.IsServerStarted)
    //            InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Loading"));

    //        Debug.Log("[BootstrapSceneManager] Loading removida após spawn dos players.");
    //    }
    //}


    private void OnScenesLoaded(SceneLoadEndEventArgs args)
    {
        if (!InstanceFinder.IsServerStarted)
            return;

        if (args.LoadedScenes == null || args.LoadedScenes.Count() == 0)
            return;

        string sceneName = args.LoadedScenes[0].name;

        if (sceneName == "Loading")
            return; // does nothing

        if (sceneName == "StartMenu")
            return; // menu does not spawn players

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


    #region Load Scenes Using Loading

    public void LoadLoadingThenMap(string targetMap)
    {
        StartCoroutine(LoadLoadingThenMapRoutine(targetMap));
    }

    private IEnumerator LoadLoadingThenMapRoutine(string targetMap)
    {
        // 1. Unload StartMenu if it is loaded
        var startMenuScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("StartMenu");
        if (startMenuScene.isLoaded)
        {
            InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("StartMenu")); // ideal / correct but it doesn't work
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("StartMenu"); // TODO: You shouldn't use this but it's the only way that works

            // espera descarregar
            yield return new WaitUntil(() => !UnityEngine.SceneManagement.SceneManager.GetSceneByName("StartMenu").isLoaded);
        }

        // 2. Load LOADING scene
        SceneLoadData loadLoading = new SceneLoadData("Loading");
        InstanceFinder.SceneManager.LoadGlobalScenes(loadLoading);

        yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading").isLoaded);

        // 3. Load the actual map
        SceneLoadData loadMap = new SceneLoadData(targetMap);
        InstanceFinder.SceneManager.LoadGlobalScenes(loadMap);
    }

    #endregion

    #region Spawn Players

    

    private IEnumerator FinishLoadAndSpawn()
    {
        yield return null;

        // Update map spawnpoints
        spawner.spawnPoints = GameObject.FindObjectsByType<SpawnPointMarker>(FindObjectsSortMode.None)
                                         .Select(s => s.transform)
                                         .ToArray();

        foreach (NetworkConnection conn in InstanceFinder.ServerManager.Clients.Values)
        {
            spawner.CaptureSpawnPointsFromScene();

            // Create the player only if it doesn't already exist
            spawner.SpawnPlayer(conn);
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
