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
        if (!InstanceFinder.IsServerStarted) return;

        // Unload Lobby
        InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData("Lobby"));

        // Load Map1_Part1
        //SceneLoadData sld = new SceneLoadData("Map1_Part1");
        //InstanceFinder.SceneManager.LoadGlobalScenes(sld);

        StartCoroutine(LoadMapWithDelay(10f, "Map1_Part1"));
    }

    private IEnumerator LoadMapWithDelay(float delay, string mapName)
    {
        // Aguarda o tempo especificado
        yield return new WaitForSeconds(delay);

        // Carrega a nova cena
        SceneLoadData sld = new SceneLoadData(mapName);
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);

        Debug.Log($"{mapName} foi carregada após {delay} segundos.");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}