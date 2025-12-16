using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private EnemyData enemyData; // Data defining spawn count, distances, respawn time, prefab

    #endregion

    #region Private Fields

    private List<EnemySpawnPoint> allSpawnPoints = new(); // All spawn points in the scene
    private List<EnemySpawnPoint> activeSpawnPoints = new(); // Spawn points currently used for spawning
    private Transform player; // Reference to the player

    [Header("Boss Spawn")]
    [Tooltip("Boss dies permanently when false -> Save On Disk")]
    [SerializeField] private bool debugMode = true;

    private HashSet<string> sessionDeadBosses = new();

    #endregion

    #region Unity Methods

    private void Awake()
    {
        allSpawnPoints = new List<EnemySpawnPoint>(GetComponentsInChildren<EnemySpawnPoint>());

        if (IsBossDead())
        {
            Debug.Log($"[Spawner] Boss {enemyData.CharacterName} já morto detectado no Awake. Desativando objeto.");
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(FindPlayerRoutine());
    }

    #endregion

    #region Player Finding

    private IEnumerator FindPlayerRoutine()
    {
        while (player == null)
        {
            TryFindLocalPlayer();
            yield return new WaitForSeconds(0.5f);
        }

        if (IsBossDead())
        {
            Debug.Log($"[Spawner] Boss {enemyData.CharacterName} esta morto permanentemente. Spawner desativado.");
            yield break;
        }

        SetupSpawnPoints();
        StartCoroutine(SpawnRoutine());
    }

    private void TryFindLocalPlayer()
    {
        // Multiplayer
        // foreach (var netObj in FindObjectsByType<NetworkObject>(FindObjectsSortMode.None))
        // {
        //     if (netObj != null && netObj.CompareTag("Player") && netObj.IsOwner)
        //     {
        //         player = netObj.transform;
        //         return;
        //     }
        // }

        // Singleplayer fallback
        var spPlayer = GameObject.FindWithTag("Player");
        if (spPlayer != null)
        {
            player = spPlayer.transform;
        }
    }

    #endregion

    #region Spawn Points Setup

    private void SetupSpawnPoints()
    {
        activeSpawnPoints.Clear();

        foreach (var sp in allSpawnPoints)
        {
            if (sp.isActiveSpawnPoint)
            {
                activeSpawnPoints.Add(sp);
            }
        }

        var temp = new List<EnemySpawnPoint>(allSpawnPoints);
        int count = Mathf.Min(enemyData.SpawnCount, temp.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, temp.Count);
            var sp = temp[index];
            activeSpawnPoints.Add(sp);
            sp.isActiveSpawnPoint = true;
            temp.RemoveAt(index);
        }
    }

    #endregion

    #region Spawning

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            foreach (var sp in activeSpawnPoints)
            {

                if (enemyData.CharacterCategory == EnemyCategory.Boss)
                {
                    string bossKey = $"{enemyData.CharacterSpawnLevel}_{enemyData.CharacterName}";
                    if (sessionDeadBosses.Contains(bossKey))
                        continue; // no longer spawns in the session
                }

                if (sp == null || !sp.isActiveSpawnPoint) continue;

                float dist = Vector3.Distance(player.position, sp.transform.position);

                if (!sp.hasEnemy && !sp.isRespawning && dist <= enemyData.DistanceSpawn)
                {
                    SpawnEnemy(sp);
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void SpawnEnemy(EnemySpawnPoint sp)
    {
        if (IsBossDead()) return;

        if (sp.onlySpawnOnce && sp.hasSpawnedOnce)
            return;

        GameObject enemyObj = Instantiate(enemyData.CharacterPrefab, sp.transform.position, Quaternion.identity);
        sp.AssignEnemy(enemyObj);

        var enemy = enemyObj.GetComponent<Enemy>();
        enemy.spawnPoint = sp;
        enemy.spawner = this;

        if (sp.onlySpawnOnce)
            sp.hasSpawnedOnce = true;
    }

    #endregion

    #region Enemy Death Handling

    public void OnEnemyDeath(EnemySpawnPoint sp, Enemy enemy)
    {
        if (enemy == null) return;

        if (enemy.RunTimeData.CharacterCategory == EnemyCategory.Boss)
        {
            SaveBossDeath(enemy);
            sp.isRespawning = false;
            sp.hasEnemy = true;
            sp.isActiveSpawnPoint = false;
            return;
        }

        if (!sp.isRespawning)
        {
            sp.isRespawning = true;
            StartCoroutine(RespawnEnemy(sp));
        }
    }

    private IEnumerator RespawnEnemy(EnemySpawnPoint sp)
    {
        if (sp.onlySpawnOnce) yield break;

        sp.ClearEnemy();
        yield return new WaitForSeconds(enemyData.RespawnTime);

        while (Vector3.Distance(player.position, sp.transform.position) < enemyData.RespawnDistance)
            yield return new WaitForSeconds(1f);

        if (IsBossDead()) yield break;

        sp.hasEnemy = false;
        sp.isRespawning = false;
        SpawnEnemy(sp);
    }

    #endregion

    #region Utilities

    private bool IsBossDead()
    {
        if (enemyData.CharacterCategory != EnemyCategory.Boss)
            return false; // normal enemies never use debugMode

        string bossKey = $"{enemyData.CharacterSpawnLevel}_{enemyData.CharacterName}";

        if (debugMode)
            return sessionDeadBosses.Contains(bossKey); // only in session
        else
            return PlayerPrefs.GetInt($"BossDead_{bossKey}", 0) == 1; // save on disk
    }

    private void SaveBossDeath(Enemy enemy)
    {
        if (enemy.RunTimeData.CharacterCategory != EnemyCategory.Boss)
            return; // normal enemies never use debugMode

        string bossKey = $"{enemy.RunTimeData.CharacterSpawnLevel}_{enemy.RunTimeData.CharacterName}";
        string bossName = enemy.RunTimeData.CharacterName; // code to change map

        if (debugMode)
        {
            // Just blocks in session
            sessionDeadBosses.Add(bossKey);
            Debug.Log($"[DEBUG] Boss {enemy.RunTimeData.CharacterName} morto (somente na sessão).");

            TeleportToSelectMap();
        }
        else
        {
            // Write permanently
            string saveKey = $"BossDead_{bossKey}";
            PlayerPrefs.SetInt(saveKey, 1);
            PlayerPrefs.Save();
            Debug.Log($"[Spawner] Boss {enemy.RunTimeData.CharacterName} morto permanentemente e salvo em PlayerPrefs.");
        }
    }

    private void TeleportToSelectMap()
    {
        BootstrapSceneManager sm = GameObject.FindFirstObjectByType<BootstrapSceneManager>();
        // Call the method to load the SelectMap scene
        // sm.LoadScene("SelectMap");
    }

    #endregion
}