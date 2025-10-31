using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData; // Data defining spawn count, distances, respawn time, prefab

    private List<EnemySpawnPoint> allSpawnPoints = new(); // All spawn points in the scene
    private List<EnemySpawnPoint> activeSpawnPoints = new(); // Spawn points currently used for spawning

    private Transform player; // Reference to the player

    private void Awake()
    {
        // Get all spawn points in the scene using the new Unity API
        allSpawnPoints = new List<EnemySpawnPoint>(
            FindObjectsByType<EnemySpawnPoint>(FindObjectsSortMode.None)
        );
    }

    private void Start()
    {
        // Start routine to find the player before spawning enemies
        StartCoroutine(FindPlayerRoutine());
    }

    private IEnumerator FindPlayerRoutine()
    {
        // Keep trying until we find the player
        while (player == null)
        {
            TryFindLocalPlayer();
            yield return new WaitForSeconds(0.5f);
        }

        // Once player is found, select active spawn points and start spawning routine
        SetupSpawnPoints();
        StartCoroutine(SpawnRoutine());
    }

    private void TryFindLocalPlayer()
    {
        // Multiplayer: search for local network-owned player
        var allNetObjs = FindObjectsByType<NetworkObject>(FindObjectsSortMode.None);

        foreach (var netObj in allNetObjs)
        {
            if (netObj != null && netObj.CompareTag("Player") && netObj.IsOwner)
            {
                player = netObj.transform;
                Debug.Log($"[Spawner] Local Network Player Found: {player.name}");
                return;
            }
        }

        // Singleplayer fallback: find object tagged as Player
        var spPlayer = GameObject.FindWithTag("Player");
        if (spPlayer != null)
        {
            player = spPlayer.transform;
            Debug.Log("[Spawner] Singleplayer Player Found.");
        }
    }

    private void SetupSpawnPoints()
    {
        activeSpawnPoints.Clear();

        // Clone the list to randomly select unique spawn points
        List<EnemySpawnPoint> temp = new(allSpawnPoints);

        // Ensure we don't select more than available
        int count = Mathf.Min(enemyData.SpawnCount, temp.Count);
        Debug.Log($"[Spawner] Selecting {count}/{temp.Count} spawn points randomly.");

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, temp.Count);
            activeSpawnPoints.Add(temp[index]);

            // Mark this spawn point as actively used
            temp[index].isActiveSpawnPoint = true;
            Debug.Log($"[Spawner] Active spawn: {temp[index].name}");

            temp.RemoveAt(index); // Remove from temp to avoid duplicates
        }
    }

    IEnumerator SpawnRoutine()
    {
        // Main spawning loop
        while (true)
        {
            foreach (var sp in activeSpawnPoints)
            {
                if (sp == null) continue;

                float dist = Vector3.Distance(player.position, sp.transform.position);

                // Spawn only if:
                // 1. No enemy is currently at this spawn
                // 2. Not already respawning
                // 3. Player is within spawn distance
                if (!sp.hasEnemy && !sp.isRespawning && dist <= enemyData.DistanceSpawn)
                {
                    SpawnEnemy(sp);
                }
            }

            // Check every 1 second
            yield return new WaitForSeconds(1f);
        }
    }

    private void SpawnEnemy(EnemySpawnPoint sp)
    {
        // Instantiate the enemy prefab at the spawn point
        GameObject enemyObj = Instantiate(enemyData.CharacterPrefab, sp.transform.position, Quaternion.identity);

        // Assign enemy to this spawn point
        sp.AssignEnemy(enemyObj);

        // Set references for the enemy to notify this spawner when it dies
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.spawnPoint = sp;
        enemy.spawner = this;
    }

    public void OnEnemyDeath(EnemySpawnPoint sp)
    {
        // Called by the enemy when it dies
        if (!sp.isRespawning)
        {
            Debug.Log($"Enemy died at {sp.name}. Respawn in {enemyData.RespawnTime}s");
            sp.isRespawning = true; // Block spawn until respawn completes
            StartCoroutine(RespawnEnemy(sp));
        }
    }

    IEnumerator RespawnEnemy(EnemySpawnPoint sp)
    {
        // Clear current enemy from the spawn point
        sp.ClearEnemy();

        // Wait for the defined respawn time
        yield return new WaitForSeconds(enemyData.RespawnTime);

        // Wait until the player leaves the respawn radius
        while (Vector3.Distance(player.position, sp.transform.position) < enemyData.RespawnDistance)
            yield return new WaitForSeconds(1f);

        // Release the spawn point and allow spawning again
        sp.hasEnemy = false;
        sp.isRespawning = false;

        // Spawn a new enemy
        SpawnEnemy(sp);
    }
}
