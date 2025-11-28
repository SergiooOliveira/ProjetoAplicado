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

        // Apenas servidor inicia o carregamento global
        if (!InstanceFinder.IsServer) return;

        // Unload Lobby
        InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Lobby"));

        // Load Map1_Part1
        //SceneLoadData sld = new SceneLoadData("Map1_Part1");
        //InstanceFinder.SceneManager.LoadGlobalScenes(sld);

        //// Espera carregar a cena para inicializar HUD
        //StartCoroutine(WaitForSceneLoad("Map1_Part1"));
    }

    private IEnumerator WaitForSceneLoad(string sceneName)
    {
        // Espera até a cena estar carregada
        while (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded)
            yield return null;

        hud = FindObjectOfType<NetworkHudCanvases>();

        // Cena carregada, inicializa HUD ou objetos de rede
        if (hud != null)
        {
            hud.OnClick_Server(); // ou OnClick_Client
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}