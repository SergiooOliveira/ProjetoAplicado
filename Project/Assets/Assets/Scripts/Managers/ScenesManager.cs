using FishNet;
using FishNet.Managing.Scened;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    // public static ScenesManager Instance { get; private set; }
    public static ScenesManager Instance;

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

        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() != null)
        {
            Destroy(gameObject); // Destroy duplicate EventSystem
        }

    }



    // public void MapSelect()
    // {
       
    //     // LoadScene("MapSelect");
    //      UnityEngine.SceneManagement.SceneManager.LoadScene("MapSelect");
    //     UnLoadScene("Map1_Part1");
    // }



    // public void Map2_cloud()
    // {
    //     // UnLoadScene("MapSelect");
    //     // LoadScene("Map2_cloud");
    //     UnityEngine.SceneManagement.SceneManager.LoadScene("Map2_cloud");
    // }

    public void MapSelect()
    {
        // Load the "MapSelect" scene using FishNet
        LoadScene("MapSelect");
        
        // Unload the "Map1_Part1" scene using FishNet
        UnLoadScene("Map1_Part1");
    }

    public void Map2_cloud()
    {
        // Load the "Map2_cloud" scene using FishNet
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
