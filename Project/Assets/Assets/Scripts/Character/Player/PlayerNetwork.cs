using FishNet.Object;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerNetwork : NetworkBehaviour
{
    public override void OnStartNetwork()
    {
        // Ensures that the client always moves the player to the PersistentScene
        StartCoroutine(MoveToPersistentWhenReady());
    }

    private IEnumerator MoveToPersistentWhenReady()
    {
        Scene persistentScene;

        // Wait until the PersistentScene is loaded on the client
        do
        {
            persistentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("PersistentScene");
            yield return null;
        }
        while (!persistentScene.isLoaded);

        // Move player to PersistentScene
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, persistentScene);
    }
}