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
        if (conn.FirstObject != null)
        {
            Debug.Log("Player já existe — não será respawnado.");
            return;
        }

        var persistentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("PersistentScene");

        NetworkObject player = Instantiate(playerPrefab);

        // Move player to persistent scene
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(player.gameObject, persistentScene);

        InstanceFinder.ServerManager.Spawn(player, conn);
    }

    #endregion
}