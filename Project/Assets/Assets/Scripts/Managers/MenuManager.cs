using FishNet;
using FishNet.Managing.Scened;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class MenuManager : MonoBehaviour
{
    #region Fields

    private bool isLoading = false;

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

        // Unload Lobby e load Map1_Part1
        InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Lobby"));

        var sld = new SceneLoadData("Map1_Part1");
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}