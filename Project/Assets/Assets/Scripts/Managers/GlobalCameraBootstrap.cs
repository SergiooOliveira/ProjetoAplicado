using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalCameraBootstrap : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void MoveToPersistentScene()
    {
        Scene persistentScene = SceneManager.GetSceneByName("PersistentScene");

        if (persistentScene.IsValid())
        {
            SceneManager.MoveGameObjectToScene(gameObject, persistentScene);
        }
    }
}
