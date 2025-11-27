using UnityEngine;

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

        TSceneManager sm = GameObject.FindFirstObjectByType<TSceneManager>();

        // First load the Loading screen
        sm.LoadLoadingThenMap("Map1_Part1");
    }

    public void Multiplayer()
    {
        if (isLoading)
            return;

        isLoading = true;

        TSceneManager sm = GameObject.FindFirstObjectByType<TSceneManager>();

        // First load the Loading screen
        sm.LoadMapByName("Lobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}