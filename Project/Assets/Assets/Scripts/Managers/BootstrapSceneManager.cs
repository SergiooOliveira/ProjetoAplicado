using UnityEngine;
using FishNet;
using FishNet.Managing.Scened;

public class BootstrapSceneManager : MonoBehaviour
{

    public static BootstrapSceneManager Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        LoadSceneLocal("StartMenu");
    }

    public void LoadSceneLocal(string sceneName)
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }

    public void UnloadSceneLocal(string sceneName)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!InstanceFinder.IsServerStarted)
            return;

        SceneLoadData sld = new SceneLoadData(sceneName);
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    public void UnloadScene(string sceneName)
    {
        if (!InstanceFinder.IsServerStarted)
            return;

        SceneUnloadData sld = new SceneUnloadData(sceneName);
        InstanceFinder.SceneManager.UnloadGlobalScenes(sld);
    }
}
