using FishNet;
using FishNet.Managing.Scened;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy the duplicate instance
        }
    }



    public void MapSelect()
    {
       
        LoadScene("MapSelect");
        UnLoadScene("Map1_Part1");
    }



    public void Map2_cloud()
    {
        UnLoadScene("MapSelect");
        LoadScene("Map2_cloud");
    }



    public void LoadScene(string sceneName)
    {
        SceneLoadData sld = new SceneLoadData(sceneName);
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }



    public void UnLoadScene(string sceneName)
    {
        SceneUnloadData sud = new SceneUnloadData(sceneName);
        InstanceFinder.SceneManager.UnloadGlobalScenes(sud);
        // InstanceFinder.NetworkManager.SceneManager.UnloadGlobalScenes(sud);
    }
}
