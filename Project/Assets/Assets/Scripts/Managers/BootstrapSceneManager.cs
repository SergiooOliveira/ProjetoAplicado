using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class BootstrapSceneManager : MonoBehaviour
{
    #region Fields
    [Header("Player Spawner")]
    [SerializeField] private PlayerSpawner spawner;
    private string currentMap;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        InstanceFinder.SceneManager.OnLoadEnd += OnScenesLoaded;
    }

    private void OnDisable()
    {
        if (InstanceFinder.SceneManager != null)
        {
            InstanceFinder.SceneManager.OnLoadEnd -= OnScenesLoaded;
        }
    }

    private void Start()
    {
        LoadSceneLocal("StartMenu");

        StartCoroutine(WaitAndMoveGlobalCamera());
    }

    private IEnumerator WaitAndMoveGlobalCamera()
    {
        yield return new WaitUntil(() =>
            SceneManager.GetSceneByName("StartMenu").isLoaded
        );

        var globalCam = FindAnyObjectByType<GlobalCameraBootstrap>();
        if (globalCam != null)
        {
            globalCam.MoveToPersistentScene();
        }
    }
    #endregion

    #region Loacl Scenes (Menu, Lobby)
    public void LoadSceneLocal(string sceneName)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadSceneLocal(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.UnloadSceneAsync(sceneName);
    }
    #endregion

    #region Global Scenes (Maps)
    public void LoadLoadingThenMap(string targetMap)
    {
        currentMap = targetMap;
        StartCoroutine(LoadLoadingThenMapRoutine(targetMap));
    }

    private IEnumerator LoadLoadingThenMapRoutine(string targetMap)
    {
        // Load Loading scene
        InstanceFinder.SceneManager.LoadGlobalScenes(new SceneLoadData("Loading"));
        yield return new WaitUntil(() => SceneManager.GetSceneByName("Loading").isLoaded);

        // Load target map
        InstanceFinder.SceneManager.LoadGlobalScenes(new SceneLoadData(targetMap));
    }
    #endregion

    #region Scene Loaded Callback
    private void OnScenesLoaded(SceneLoadEndEventArgs args)
    {
        if (!InstanceFinder.IsServerStarted)
            return;

        if (args.LoadedScenes == null || args.LoadedScenes.Count() == 0)
            return;

        string sceneName = args.LoadedScenes[0].name;

        if (sceneName == "StartMenu" || sceneName == "Loading")
            return;

        Scene mapScene = SceneManager.GetSceneByName(sceneName);
        if (mapScene.IsValid())
            SceneManager.SetActiveScene(mapScene);

        // Handle players securely
        StartCoroutine(HandlePlayersAfterSceneLoad());
    }
    #endregion

    #region Player Spawn / Respawn
    private IEnumerator HandlePlayersAfterSceneLoad()
    {
        yield return null;

        // Update map spawn points
        spawner.CaptureSpawnPointsFromScene();

        foreach (NetworkConnection conn in InstanceFinder.ServerManager.Clients.Values)
        {
            // If the player already exists, direct spawn/respawn
            if (conn.FirstObject != null)
            {
                SpawnOrRespawnPlayer(conn);
            }
            else
            {
                // Subscribe event to safe spawn when client finishes loading
                conn.OnLoadedStartScenes += (c, success) =>
                {
                    if (!success) return;
                    SpawnOrRespawnPlayer(c);
                };
            }
        }

        UnloadSceneLocal("Loading");
    }

    private void SpawnOrRespawnPlayer(NetworkConnection conn)
    {
        if (conn.FirstObject == null)
        {
            // Initial spawn
            spawner.SpawnPlayer(conn);
        }
        else
        {
            // Persistent player
            var player = conn.FirstObject;

            // Position at map spawn
            if (spawner.spawnPoints.Length > 0)
            {
                var spawn = spawner.spawnPoints[Random.Range(0, spawner.spawnPoints.Length)];
                player.transform.position = spawn.position;
                player.transform.rotation = spawn.rotation;
            }

            var playerComponent = player.GetComponent<Player>();
            if (playerComponent != null && playerComponent.IsDead)
            {
                playerComponent.Revive();
            }

            // Disable Global Camera
            var globalCam = Object.FindFirstObjectByType<GlobalCameraBootstrap>(FindObjectsInactive.Include);
            if (globalCam != null)
                globalCam.gameObject.SetActive(false);

            // Reactivate Player (if it was in the menu)
            if (!player.gameObject.activeSelf)
                player.gameObject.SetActive(true);
        }
    }
    #endregion

    #region Reload Map
    public void ReloadCurrentMap()
    {
        UnloadSceneLocal(currentMap);
        LoadLoadingThenMap(currentMap);
    }
    #endregion
}