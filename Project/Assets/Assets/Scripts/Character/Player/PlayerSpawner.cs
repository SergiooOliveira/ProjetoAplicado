using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

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
    /// Spawns a player for the specific connection
    /// </summary>
    public void SpawnPlayer(NetworkConnection conn)
    {
        if (playerPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("PlayerPrefab ou SpawnPoints não configurados!");
            return;
        }

        // Choose random spawn point (can change to sequential)
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Avoid repeating the same spawn consecutively
        if (spawnPoints.Length > 1 && spawnIndex == lastSpawnIndex)
        {
            spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
        }

        lastSpawnIndex = spawnIndex;
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Instantiates the player on the server
        NetworkObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        InstanceFinder.ServerManager.Spawn(playerInstance, conn);
        Debug.Log($"Player spawned para {conn.ClientId} em {spawnPoint.position}");
    }

    #endregion
}