using FishNet;
using FishNet.Transporting;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Fields

    private bool isLoading = false;

    #endregion

    #region SinglePlayer

    public void PlaySingleplayer()
    {
        if (isLoading) return;
        isLoading = true;

        var client = InstanceFinder.ClientManager;
        var server = InstanceFinder.ServerManager;

        // Start server
        server.StartConnection();

        // Start client (local)
        client.OnClientConnectionState += OnClientConnectionState;
        client.StartConnection("localhost", 7777);
    }

    private void OnClientConnectionState(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            var sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();

            // Chama a coroutine de loading + mapa
            sm.UnloadSceneLocal("StartMenu");
            sm.LoadLoadingThenMap("Map1_Part1");

            // Remove o listener
            InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
        }
    }

    #endregion

    #region MultiPlayer

    public void Multiplayer()
    {
        if (isLoading) return;
        isLoading = true;

        BootstrapSceneManager sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();

        sm.UnloadSceneLocal("StartMenu");
        sm.LoadSceneLocal("Lobby");
    }

    public void PlayMultiplayer()
    {
        if (isLoading) return;
        isLoading = true;

        var sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();

        sm.UnloadScene("Lobby");
        sm.LoadLoadingThenMap("Map1_Part1");
    }

    #endregion

    #region Quit

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}