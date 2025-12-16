using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class BootstrapSceneManager : MonoBehaviour
{
    #region Fields

    [Header("Player Spawner")]
    public PlayerSpawner spawner;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        LoadSceneLocal("StartMenu");

        // InstanceFinder.SceneManager.OnLoadEnd += OnScenesLoaded;
    }

    #endregion

    #region Load / Unload Scenes Withou Host / Client

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

    #endregion

    #region Load / Unload With Host / Client

    // public void LoadScene(string sceneName)
    // {
    //     if (!InstanceFinder.IsServerStarted)
    //         return;

    //     SceneLoadData sld = new SceneLoadData(sceneName);
    //     InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    // }

    // public void UnloadScene(string sceneName)
    // {
    //     if (!InstanceFinder.IsServerStarted)
    //         return;

    //     SceneUnloadData sld = new SceneUnloadData(sceneName);
    //     InstanceFinder.SceneManager.UnloadGlobalScenes(sld);
    // }

    #endregion

    #region On Scene Load

    // private void OnScenesLoaded(SceneLoadEndEventArgs args)
    // {
    //     if (!InstanceFinder.IsServerStarted)
    //         return;

    //     if (args.LoadedScenes == null || args.LoadedScenes.Count() == 0)
    //         return;

    //     string sceneName = args.LoadedScenes[0].name;

    //     if (sceneName == "StartMenu" || sceneName == "Loading")
    //         return; // does nothing

    //     var newScene = SceneManager.GetSceneByName(sceneName);
    //     if (newScene.IsValid())
    //     {
    //         SceneManager.SetActiveScene(newScene);
    //     }

    //     // Update spawn points
    //     SpawnPointMarker[] markers = GameObject.FindObjectsByType<SpawnPointMarker>(FindObjectsSortMode.None);
    //     spawner.spawnPoints = markers.Select(s => s.transform).ToArray();

    //     // Spawn players
    //     StartCoroutine(FinishLoadAndSpawn());
    // }

    #endregion

    #region Load Scenes Using Loading

    // public void LoadLoadingThenMap(string targetMap)
    // {
    //     StartCoroutine(LoadLoadingThenMapRoutine(targetMap));
    // }

    // private IEnumerator LoadLoadingThenMapRoutine(string targetMap)
    // {
    //     // 1. Load LOADING scene
    //     SceneLoadData loadLoading = new SceneLoadData("Loading");
    //     InstanceFinder.SceneManager.LoadGlobalScenes(loadLoading);

    //     yield return new WaitUntil(() => SceneManager.GetSceneByName("Loading").isLoaded);

    //     // 2. Load the actual map
    //     SceneLoadData loadMap = new SceneLoadData(targetMap);
    //     InstanceFinder.SceneManager.LoadGlobalScenes(loadMap);
    // }

    #endregion

    #region Spawn Players

    private IEnumerator FinishLoadAndSpawn()
    {
        yield return null;

        // Update map spawnpoints
        spawner.spawnPoints = GameObject.FindObjectsByType<SpawnPointMarker>(FindObjectsSortMode.None)
                                         .Select(s => s.transform)
                                         .ToArray();

        // foreach (NetworkConnection conn in InstanceFinder.ServerManager.Clients.Values)
        // {
        //     spawner.CaptureSpawnPointsFromScene();
        //     spawner.SpawnPlayer(conn);
        // }

        // Remove LOADING
        yield return new WaitForSeconds(0.1f);
        UnloadLoading();
    }

    private void UnloadLoading()
    {
        var loadingScene = SceneManager.GetSceneByName("Loading");

        // if (loadingScene.isLoaded)
        // {
        //     InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Loading"));
        //     SceneManager.UnloadSceneAsync("Loading");
        // }
    }

    #endregion
}