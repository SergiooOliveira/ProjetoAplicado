using FishNet;
using FishNet.Connection;
using FishNet.Object;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    #region Serialized Fields

    [Header("Player Settings")]
    public NetworkObject playerPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private int lastSpawnIndex = -1;

    #endregion

    #region Spawn Player

    /// <summary>
    /// Atualiza spawnPoints baseado nos SpawnPointMarker do mapa carregado.
    /// Deve ser chamado **ap�s a cena do mapa estar carregada**.
    /// </summary>
    public void CaptureSpawnPointsFromScene()
    {
        var markers = GameObject.FindObjectsByType<SpawnPointMarker>(FindObjectsSortMode.None);
        if (markers.Length > 0)
        {
            spawnPoints = markers.Select(m => m.transform).ToArray();
        }
    }

    /// <summary>
    /// Spawns a player for the specific connection
    /// </summary>
    public void SpawnPlayer(NetworkConnection conn)
    {
        if (playerPrefab == null || spawnPoints.Length == 0)
            return;


        var persistentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("PersistentScene");

        NetworkObject player = Instantiate(playerPrefab);

        // Move player to persistent scene
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(player.gameObject, persistentScene);

        // Instantiates the player on the server
        NetworkObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        Scene persistentScene = SceneManager.GetSceneByName("PersistentScene");
        if (persistentScene.IsValid())
        {
            SceneManager.MoveGameObjectToScene(playerInstance.gameObject, persistentScene);
        }

        InstanceFinder.ServerManager.Spawn(playerInstance, conn);
    }

    #endregion
}