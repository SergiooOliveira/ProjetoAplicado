using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;  // Reference to your player prefab
    public Transform spawnPoint;     // Reference to the spawn point

    void Start()
    {
        // Instantiate the player at the spawn point
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (playerPrefab != null && spawnPoint != null)
        {
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Player prefab or spawn point is not assigned.");
        }
    }
}
