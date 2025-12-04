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

        InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoadEnd;
    }

    private void OnSceneLoadEnd(SceneLoadEndEventArgs args)
    {
        foreach (var scene in args.LoadedScenes)
        {
            // Ignora menu/loading
            if (scene.name == "StartMenu" || scene.name == "Loading") continue;

            // Tornar a nova cena ativa
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);

            // Spawn players só no host
            if (InstanceFinder.IsServerStarted)
            {
                spawner.CaptureSpawnPointsFromScene();

                foreach (var conn in InstanceFinder.ServerManager.Clients.Values)
                    spawner.SpawnPlayer(conn);
            }

            foreach (var conn in InstanceFinder.ClientManager.Clients.Values)
            {
                NetworkObject player = conn.FirstObject;
                if (player != null)
                {
                    Scene persistentScene = SceneManager.GetSceneByName("PersistentScene");
                    if (persistentScene.IsValid())
                        SceneManager.MoveGameObjectToScene(player.gameObject, persistentScene);
                }
            }

            // Remove Loading localmente no cliente
            var loadingScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading");
            if (loadingScene.isLoaded)
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Loading");
        }
    }

    public void LoadSceneLocal(string sceneName)
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }

    public void UnloadSceneLocal(string sceneName)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
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
    //    StartCoroutine(LoadLoadingThenMapRoutine(targetMap));
    //}

    //private IEnumerator LoadLoadingThenMapRoutine(string targetMap)
    //{
    //    // 1. Carrega a cena de loading
    //    LoadScene("Loading");

    //    // Espera até o loading estar carregado
    //    yield return new WaitUntil(() =>
    //        UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading").isLoaded);

    //    // 2. Carrega a cena do mapa
    //    LoadScene(targetMap);

    //    // Espera a cena do mapa ser carregada
    //    yield return new WaitUntil(() =>
    //        UnityEngine.SceneManagement.SceneManager.GetSceneByName(targetMap).isLoaded);

    //    // 3. Captura os spawn points
    //    spawner.CaptureSpawnPointsFromScene();

    //    // 4. Spawn dos players
    //    foreach (var conn in InstanceFinder.ServerManager.Clients.Values)
    //    {
    //        spawner.SpawnPlayer(conn);
    //    }

    //    // 5. Remove a tela de loading
    //    //UnloadLoading();
    //    InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Loading"));
    //    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Loading");
    //    UnloadScene("Loading");
    //    UnloadSceneLocal("Loading");
    //}

    //private void UnloadLoading()
    //{
    //    var loadingScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading");
    //    if (loadingScene.isLoaded)
    //    {
    //        // Descarrega globalmente para todos
    //        InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Loading"));
    //        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Loading");
    //        Debug.Log("[BootstrapSceneManager] Loading removida pelo servidor.");
    //    }
    //}

    public void LoadLoadingThenMap(string targetMap)
    {
        if (!InstanceFinder.IsServerStarted)
            return; // Apenas host inicia o mapa

        StartCoroutine(LoadLoadingThenMapRoutine(targetMap));
    }

    private IEnumerator LoadLoadingThenMapRoutine(string targetMap)
    {
        // 1. Carrega Loading globalmente
        SceneLoadData loadLoading = new SceneLoadData("Loading");
        InstanceFinder.SceneManager.LoadGlobalScenes(loadLoading);

        yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading").isLoaded);

        // 2. Carrega mapa globalmente
        SceneLoadData loadMap = new SceneLoadData(targetMap);
        InstanceFinder.SceneManager.LoadGlobalScenes(loadMap);

        yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetSceneByName(targetMap).isLoaded);

        Debug.Log("[BootstrapSceneManager] Map loaded, spawn dos players será executado via OnLoadEnd.");
    }

}
