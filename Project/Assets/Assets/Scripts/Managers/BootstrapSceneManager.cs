using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

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
        {
            Debug.LogWarning("Apenas o host pode iniciar o mapa.");
            return;
        }

        StartCoroutine(LoadLoadingThenMapRoutine(targetMap));
    }

    private IEnumerator LoadLoadingThenMapRoutine(string targetMap)
    {
        // 1. Carrega a cena de Loading globalmente
        SceneLoadData loadLoading = new SceneLoadData("Loading");
        InstanceFinder.SceneManager.LoadGlobalScenes(loadLoading);

        // Espera até que a Loading esteja realmente carregada
        yield return new WaitUntil(() =>
            UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading").isLoaded);

        // 2. Carrega a cena do mapa globalmente
        SceneLoadData loadMap = new SceneLoadData(targetMap);
        InstanceFinder.SceneManager.LoadGlobalScenes(loadMap);

        // Espera até que o mapa esteja carregado
        yield return new WaitUntil(() =>
            UnityEngine.SceneManagement.SceneManager.GetSceneByName(targetMap).isLoaded);

        // 3. Captura os spawn points no mapa
        spawner.CaptureSpawnPointsFromScene();

        // 4. Spawn de todos os players (host e clientes)
        foreach (var conn in InstanceFinder.ServerManager.Clients.Values)
        {
            spawner.SpawnPlayer(conn);
        }

        // Pequena espera para garantir que clients tenham processado o spawn
        yield return new WaitForEndOfFrame();

        // 5. Remove a tela de Loading globalmente
        SceneUnloadData unloadLoading = new SceneUnloadData("Loading");
        InstanceFinder.SceneManager.UnloadGlobalScenes(unloadLoading);

        // Localmente também garante que a Loading seja removida no servidor
        var loadingScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading");
        if (loadingScene.isLoaded)
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Loading");

        Debug.Log("[BootstrapSceneManager] Loading removida após spawn dos players.");
    }

}
