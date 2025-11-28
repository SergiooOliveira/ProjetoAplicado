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

        BootstrapSceneManager sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();

        // First load the Loading screen
        sm.UnloadSceneLocal("StartMenu");
        sm.LoadSceneLocal("Lobby");
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