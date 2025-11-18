using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;

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

    private GameObject teleportPoints;
    private GameObject startMap2; // Reference to the "StartMap2" object
    private GameObject startMap3; // Reference to the "StartMap3" object
    private GameObject startMap4; // Reference to the "StartMap4" object

    #region Unity Methods

    private void Awake()
    {
        allSpawnPoints = new List<EnemySpawnPoint>(GetComponentsInChildren<EnemySpawnPoint>());

        if (IsBossDead())
        {
            Debug.Log($"[Spawner] Boss {enemyData.CharacterName} j� morto detectado no Awake. Desativando objeto.");
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(FindPlayerRoutine());

        teleportPoints = GameObject.Find("TeleportPoints");
        startMap2 = teleportPoints.transform.Find("StartMap2").gameObject;
        startMap3 = teleportPoints.transform.Find("StartMap3").gameObject;
        startMap4 = teleportPoints.transform.Find("StartMap4").gameObject;
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
            Debug.Log($"[Spawner] Boss {enemyData.CharacterName} est� morto permanentemente. Spawner desativado.");
            yield break;
        }

        SetupSpawnPoints();
        StartCoroutine(SpawnRoutine());
    }

    private void TryFindLocalPlayer()
    {
        // Multiplayer
        foreach (var netObj in FindObjectsByType<NetworkObject>(FindObjectsSortMode.None))
        {
            if (netObj != null && netObj.CompareTag("Player") && netObj.IsOwner)
            {
                player = netObj.transform;
                Debug.Log($"[Spawner] Local Network Player Found: {player.name}");
                return;
            }
        }

        // Singleplayer fallback
        var spPlayer = GameObject.FindWithTag("Player");
        if (spPlayer != null)
        {
            player = spPlayer.transform;
            Debug.Log("[Spawner] Singleplayer Player Found.");
        }
    }

    #endregion

    #region Spawn Points Setup

    private void SetupSpawnPoints()
    {
        activeSpawnPoints.Clear();

        var temp = new List<EnemySpawnPoint>(allSpawnPoints);
        int count = Mathf.Min(enemyData.SpawnCount, temp.Count);
        Debug.Log($"[Spawner] Selecting {count}/{temp.Count} spawn points randomly.");

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, temp.Count);
            var sp = temp[index];
            activeSpawnPoints.Add(sp);
            sp.isActiveSpawnPoint = true;
            Debug.Log($"[Spawner] Active spawn: {sp.name}");
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
                        continue; // n�o spawna mais na sess�o
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

        GameObject enemyObj = Instantiate(enemyData.CharacterPrefab, sp.transform.position, Quaternion.identity);
        sp.AssignEnemy(enemyObj);

        var enemy = enemyObj.GetComponent<Enemy>();
        enemy.spawnPoint = sp;
        enemy.spawner = this;

        Debug.Log($"{enemy.RunTimeData}");
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
            Debug.Log($"Enemy died at {sp.name}. Respawn in {enemyData.RespawnTime}s");
            sp.isRespawning = true;
            StartCoroutine(RespawnEnemy(sp));
        }
    }

    private IEnumerator RespawnEnemy(EnemySpawnPoint sp)
    {
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
        string bossName = enemy.RunTimeData.CharacterName; // codigo para o mudar de mapa

        if (debugMode)
        {
            // Just blocks in session
            sessionDeadBosses.Add(bossKey);
            Debug.Log($"[DEBUG] Boss {enemy.RunTimeData.CharacterName} morto (somente na sess�o).");
        
            // codigo para o mudar de mapa
            switch (bossName)
            {
                case "Bringer Of Death":
                    TeleportToMap2();
                    break;

                case "Skyrath":
                    TeleportToMap3();
                    break;
                
                case "Bringer Of Undeath":
                    TeleportToMap4();
                    break;

                case "Bringer Of God":
                    // TeleportToMap5();
                    break;

                default:
                    Debug.Log("Boss não encontrado");
                    break;
            }
            
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

    #endregion

    void TeleportToMap2()
    {
        // Debug.Log(player.position);
        // Set the player's position to (201, -63, 0)
        player.transform.position = startMap2.transform.position;
    }

    void TeleportToMap3()
    {
        // Debug.Log(player.position);
        // Set the player's position to (201, -63, 0)
        player.transform.position = startMap3.transform.position;
    }

    void TeleportToMap4()
    {
        // Debug.Log(player.position);
        // Set the player's position to (201, -63, 0)
        player.transform.position = startMap4.transform.position;
    }


}