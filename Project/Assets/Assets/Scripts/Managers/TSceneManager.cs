using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Evita ambiguidade: usa o nome completo quando precisares do SceneManager da Unity
// (não há alias usando-directive aqui para manter claro)
public class TSceneManager : MonoBehaviour
{
    #region Serialized Fields

    public static TSceneManager Instance;

    [Header("Player Spawner")]
    public PlayerSpawner spawner;
    private static bool startMenuLoadedOnce = false;

    private HashSet<int> loadedClients = new HashSet<int>();
    private int expectedClients = 0;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Regista apenas os eventos do FishNet SceneManager necessários
        if (InstanceFinder.SceneManager != null)
            InstanceFinder.SceneManager.OnLoadEnd += OnScenesLoaded;
    }

    private void Start()
    {
        if (spawner == null)
            spawner = GameObject.FindFirstObjectByType<PlayerSpawner>();

        if (spawner == null)
            Debug.LogError("[TSceneManager] PlayerSpawner não encontrado na cena!");

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
        yield return null;

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
            Debug.LogWarning("[TSceneManager] Apenas o servidor pode carregar cenas!");
            return;
        }

        // Unload all active scenes except PersistentScene using FishNet
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene != "PersistentScene")
        {
            // Use UnloadGlobalScenes (FishNet) — do not call Unity's UnloadSceneAsync here
            InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData(currentScene));
        }

        // Load the new scene as GLOBAL (so clients will also load it)
        SceneLoadData sld = new SceneLoadData(sceneToLoad)
        {
            // ReplaceOption.All ensures old scenes are removed if needed (adjust if you want different behavior)
            ReplaceScenes = ReplaceOption.All
        };

        Debug.Log($"[TSceneManager] Carregando cena global: {sceneToLoad}");
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    #endregion

    #region Load Scenes

    /// <summary>
    /// Called when FishNet finishes loading scenes. On the server this fires after clients loaded scenes too.
    /// </summary>
    private void OnScenesLoaded(SceneLoadEndEventArgs args)
    {
        // Only run server side logic here (spawning should be server-side).
        if (!InstanceFinder.IsServerStarted)
            return;

        if (args.LoadedScenes == null || args.LoadedScenes.Count() == 0)
            return;

        // NOTE: LoadedScenes can contain multiple scenes; take the first meaningful one.
        string sceneName = args.LoadedScenes[0].name;
        Debug.Log($"[TSceneManager] OnScenesLoaded fired. Scene: {sceneName}");

        // Do not spawn players for menus
        if (sceneName == "Loading" || sceneName == "StartMenu" || sceneName == "Lobby")
            return;

        // Set active scene on the server for proper Instantiate behaviour
        var newScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        if (newScene.IsValid())
        {
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(newScene);
            Debug.Log($"[TSceneManager] ActiveScene set to: {sceneName}");
        }

        Debug.Log($"[TSceneManager] Mapa carregado: {sceneName}");

        // Update spawn points (scene objects must be present now)
        //SpawnPointMarker[] markers = GameObject.FindObjectsByType<SpawnPointMarker>(FindObjectsSortMode.None);
        //spawner.spawnPoints = markers.Select(s => s.transform).ToArray();

        // SPAWN: spawn players now that FishNet has finished loading the scenes (clients are synchronized)
        foreach (NetworkConnection conn in InstanceFinder.ServerManager.Clients.Values)
        {
            // If there's already a first object, optionally despawn it to avoid duplicates.
            // If you want to preserve existing FirstObject for hot-join or reconnect, adjust logic.
            //if (conn.FirstObject != null)
            //{
            //    Debug.Log($"[TSceneManager] Despawn FirstObject for Conn {conn.ClientId}");
            //    InstanceFinder.ServerManager.Objects.Despawn(conn.FirstObject);
            //}

            // Spawn the player for that connection
            //spawner.SpawnPlayer(conn);
        }

        // Remove Loading scene via FishNet API (do not call Unity unload directly)
        if (UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading").isLoaded)
        {
            Debug.Log("[TSceneManager] Unloading Loading scene via FishNet");
            InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Loading"));
        }
    }

    #endregion

    #region Load Scenes Using Loading

    public void LoadLoadingThenMap(string targetMap)
    {
        StartCoroutine(LoadLoadingThenMapRoutine(targetMap));
    }

    private IEnumerator LoadLoadingThenMapRoutine(string targetMap)
    {
        if (!InstanceFinder.IsServerStarted)
        {
            Debug.LogWarning("[TSceneManager] LoadLoadingThenMapRoutine called but server not started.");
            yield break;
        }

        // 1) Unload old menu scenes (if any) using FishNet
        string[] scenesToUnload = { "StartMenu", "Lobby" };
        foreach (var sceneName in scenesToUnload)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                Debug.Log($"[TSceneManager] Unloading old scene: {sceneName}");
                InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData(sceneName));
            }
        }

        // 2) Load Loading as a global scene for everyone
        Debug.Log("[TSceneManager] Loading 'Loading' scene (global)...");
        SceneLoadData sldLoading = new SceneLoadData("Loading")
        {
            ReplaceScenes = ReplaceOption.None
        };
        InstanceFinder.SceneManager.LoadGlobalScenes(sldLoading);

        // Wait until Unity reports the Loading scene as loaded
        yield return new WaitUntil(() =>
            UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading").isLoaded
        );

        // 3) Load the target map as a global scene and replace previous scenes
        Debug.Log($"[TSceneManager] Loading target map '{targetMap}' (global)...");
        SceneLoadData sldMap = new SceneLoadData(targetMap)
        {
            ReplaceScenes = ReplaceOption.All
        };
        InstanceFinder.SceneManager.LoadGlobalScenes(sldMap);

        // Wait until Unity reports the target map is loaded
        yield return new WaitUntil(() =>
            UnityEngine.SceneManagement.SceneManager.GetSceneByName(targetMap).isLoaded
        );

        // 4) Unload Loading via FishNet (do not call Unity unload)
        if (UnityEngine.SceneManagement.SceneManager.GetSceneByName("Loading").isLoaded)
        {
            Debug.Log("[TSceneManager] Unloading 'Loading' (via FishNet)...");
            InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Loading"));
        }

        Debug.Log($"[TSceneManager] LoadLoadingThenMapRoutine finished for {targetMap}");
    }

    #endregion

    #region Spawn Helpers (kept empty here but can add logic)

    // Note: spawning logic is inside OnScenesLoaded so no additional coroutine is required.

    #endregion
}