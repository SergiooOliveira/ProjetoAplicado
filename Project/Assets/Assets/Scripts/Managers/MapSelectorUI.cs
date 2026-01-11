using UnityEngine;
using UnityEngine.UI;

public class MapSelectorUI : MonoBehaviour
{
    #region Serialized Fields
    private bool isLoading = false;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        PlayerController localPlayer = FindLocalPlayer();
        if (localPlayer != null)
            localPlayer.gameObject.SetActive(false);

        var globalCam = Object.FindFirstObjectByType<GlobalCameraBootstrap>(FindObjectsInactive.Include);
        if (globalCam != null)
            globalCam.gameObject.SetActive(true);
    }
    #endregion

    #region Load Map
    public void OnMapSelect(string mapName)
    {
        if (isLoading) return;
        isLoading = true;

        BootstrapSceneManager sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();
        sm.UnloadSceneLocal("SelectMap");
        sm.LoadLoadingThenMap(mapName);
    }
    #endregion

    #region Helpers
    private PlayerController FindLocalPlayer()
    {
        PlayerController[] players = Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (var p in players)
        {
            if (p.IsOwner) // or p.CompareTag("Player") if multiplayer
                return p;
        }
        return null;
    }
    #endregion
}