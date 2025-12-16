using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    #region Serialized Fields

    [Header("Player Settings")]
    // public NetworkObject playerPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private int lastSpawnIndex = -1;

    #endregion

    #region Spawn Player

    /// <summary>
    /// Updates spawnPoints based on the loaded map's SpawnPointMarker.
    /// Must be called after the map scene is loaded.
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
    // public void SpawnPlayer(NetworkConnection conn)
    // {
    //     if (playerPrefab == null || spawnPoints.Length == 0)
    //         return;

    //     // Choose random spawn
    //     int spawnIndex = Random.Range(0, spawnPoints.Length);
    //     if (spawnPoints.Length > 1 && spawnIndex == lastSpawnIndex)
    //         spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
    //     lastSpawnIndex = spawnIndex;

    //     Transform spawnPoint = spawnPoints[spawnIndex];

    //     // Instantiates the player at spawn
    //     NetworkObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

    //     // Move to PersistentScene
    //     Scene persistentScene = SceneManager.GetSceneByName("PersistentScene");
    //     if (persistentScene.IsValid())
    //         SceneManager.MoveGameObjectToScene(playerInstance.gameObject, persistentScene);

    //     // Spawn on the server
    //     InstanceFinder.ServerManager.Spawn(playerInstance, conn);
    // }

    #endregion
}