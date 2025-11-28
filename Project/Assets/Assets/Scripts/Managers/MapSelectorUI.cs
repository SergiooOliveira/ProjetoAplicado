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

    #endregion

    #region Unity Methods

    private void Start()
    {
        buttonMap1.onClick.AddListener(() => OnMapSelect("Map1_Part1"));
        buttonMap2.onClick.AddListener(() => OnMapSelect("Map2_cloud"));
        buttonMap3.onClick.AddListener(() => OnMapSelect("Map3_ice"));
        buttonMap4.onClick.AddListener(() => OnMapSelect("Map4_light"));
    }

    #endregion

    #region Load Map

    private void OnMapSelect(string mapName)
    {
        if (!InstanceFinder.IsServerStarted)
        {
            Debug.LogWarning("Apenas o servidor pode trocar de mapa!");
            return;
        }

        BootstrapSceneManager sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();

        sm.UnloadScene("SelectMap");
        sm.LoadScene(mapName);
    }

    #endregion
}