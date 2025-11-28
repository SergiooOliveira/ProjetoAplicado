using FishNet;
using FishNet.Example;
using FishNet.Managing.Scened;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class MenuManager : MonoBehaviour
{
    #region Fields

    private bool isLoading = false;

    [Header("HUD")]
    public NetworkHudCanvases hud;

    #endregion

    #region Methods

    public void PlayButton()
    {
        if (isLoading)
            return;

        isLoading = true;

        //TSceneManager sm = GameObject.FindFirstObjectByType<TSceneManager>();

        //// First load the Loading screen
        //sm.LoadLoadingThenMap("Map1_Part1");

        BootstrapSceneManager sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();
        sm.UnloadScene("StartMenu");
        sm.LoadScene("Map1_Part1");
    }

    public void Multiplayer()
    {
        if (isLoading)
            return;

        isLoading = true;

        //TSceneManager sm = GameObject.FindFirstObjectByType<TSceneManager>();
        //sm.LoadMapByName("Lobby");
        BootstrapSceneManager sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();

        // First load the Loading screen
        sm.UnloadScene("StartMenu");
        sm.LoadScene("Lobby");
    }

    public void PlayMultiplayer()
    {
        if (isLoading) return;
        isLoading = true;

        BootstrapSceneManager sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();

        // First load the Loading screen
        sm.UnloadScene("Lobby");
        sm.LoadScene("Map1_Part1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}

//using FishNet;
//using FishNet.Example;
//using FishNet.Managing.Scened;
//using System.Collections;
//using UnityEngine;

//public class MenuManager : MonoBehaviour
//{
//    private bool isLoading = false;
//    [Header("HUD")]
//    public NetworkHudCanvases hud;

//    public void PlayButton()
//    {
//        if (isLoading) return;
//        isLoading = true;

//        BootstrapSceneManager sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();
//        sm.UnloadScene("StartMenu", () =>
//        {
//            sm.LoadScene("Map1_Part1");
//        });
//    }

//    public void Multiplayer()
//    {
//        if (isLoading) return;
//        isLoading = true;

//        BootstrapSceneManager sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();
//        sm.UnloadScene("StartMenu", () =>
//        {
//            sm.LoadScene("Lobby");
//        });
//    }

//    public void PlayMultiplayer()
//    {
//        if (isLoading) return;
//        isLoading = true;

//        if (!InstanceFinder.IsServerStarted) return;

//        BootstrapSceneManager sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();

//        // Use o BootstrapSceneManager para executar a Coroutine segura
//        sm.UnloadScene("Lobby", () =>
//        {
//            sm.StartCoroutine(sm.LoadMapWithDelay(10f, "Map1_Part1"));
//        });
//    }

//    public IEnumerator LoadMapWithDelay(float delay, string mapName)
//    {
//        yield return new WaitForSeconds(delay);

//        SceneLoadData sld = new SceneLoadData(mapName);
//        InstanceFinder.SceneManager.LoadGlobalScenes(sld);

//        Debug.Log($"[{nameof(BootstrapSceneManager)}] {mapName} carregada após {delay} segundos.");
//    }

//    public void QuitGame()
//    {
//        Application.Quit();
//    }
//}
