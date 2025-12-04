using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerNetwork : NetworkBehaviour
{
    public override void OnStartNetwork()
    {
        // Garante que o cliente move sempre o player para a PersistentScene
        StartCoroutine(MoveToPersistentWhenReady());
    }

    private IEnumerator MoveToPersistentWhenReady()
    {
        Scene persistentScene;

        // Aguarda até a PersistentScene estar carregada no cliente
        do
        {
            persistentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("PersistentScene");
            yield return null;
        }
        while (!persistentScene.isLoaded);

        // Move o player para a PersistentScene
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, persistentScene);
        Debug.Log("[Client] Player movido para PersistentScene");
    }
}
