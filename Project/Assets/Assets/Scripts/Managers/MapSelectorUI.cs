using UnityEngine;
using UnityEngine.UI;
using FishNet;

public class MapSelectorUI : MonoBehaviour
{
    #region Serialized Fields

    [Header("Map Buttons")]
    public Button buttonMap1;
    public Button buttonMap2;
    public Button buttonMap3;
    public Button buttonMap4;
    public Button buttonMap5;

    private bool isLoading = false;

    #endregion

    #region Unity Methods

    private void Start()
    {

        PlayerController localPlayer = FindLocalPlayer();
        if (localPlayer != null) localPlayer.gameObject.SetActive(false);

        buttonMap1.onClick.AddListener(() => OnMapSelect("Map1_Part1"));
        buttonMap2.onClick.AddListener(() => OnMapSelect("Map2_cloud"));
        buttonMap3.onClick.AddListener(() => OnMapSelect("Map3_ice"));
        buttonMap4.onClick.AddListener(() => OnMapSelect("Map4_light"));
    }

    #endregion

    #region Load Map

    private void OnMapSelect(string mapName)
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