using UnityEngine;
using FishNet;
using FishNet.Managing.Scened;
using System.Collections;
using System;

public class BootstrapSceneManager : MonoBehaviour
{

    private static bool startMenuLoadedOnce = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {

        // Only loads the StartMenu once
        if (!startMenuLoadedOnce)
        {
            startMenuLoadedOnce = true;
            StartCoroutine(WaitServerAndMaybeLoadStartMenu());
        }
    }

    private IEnumerator WaitServerAndMaybeLoadStartMenu()
    {
        yield return null;

        // Wait until the Server is started (avoid using IsServer at startup)
        //Debug.Log("[TSceneManager] Aguardando servidor iniciar...");
        //yield return new WaitUntil(() => InstanceFinder.IsServerStarted);

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
            LoadScene("StartMenu");
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

    //public void UnloadScene(string sceneName, Action callbackAfterUnload = null)
    //{
    //    if (!InstanceFinder.IsServerStarted)
    //        return;

    //    // Chama o unload no servidor
    //    SceneUnloadData sud = new SceneUnloadData(sceneName);
    //    InstanceFinder.SceneManager.UnloadGlobalScenes(sud);
    //    Debug.Log($"[BootstrapSceneManager] UnloadScene chamado: {sceneName}");

    //    // Inicia coroutine para aguardar a cena realmente descarregar
    //    StartCoroutine(WaitForUnload(sceneName, callbackAfterUnload));
    //}

    //private IEnumerator WaitForUnload(string sceneName, Action callback)
    //{
    //    // Espera até que a cena seja descarregada do servidor
    //    yield return new WaitUntil(() =>
    //        !UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded
    //    );

    //    Debug.Log($"[BootstrapSceneManager] Unload de {sceneName} concluído!");
    //    callback?.Invoke();
    //}

    //public IEnumerator LoadMapWithDelay(float delay, string mapName)
    //{
    //    yield return new WaitForSeconds(delay);

    //    SceneLoadData sld = new SceneLoadData(mapName);
    //    InstanceFinder.SceneManager.LoadGlobalScenes(sld);

    //    Debug.Log($"[{nameof(BootstrapSceneManager)}] {mapName} carregada após {delay} segundos.");
    //}

}
