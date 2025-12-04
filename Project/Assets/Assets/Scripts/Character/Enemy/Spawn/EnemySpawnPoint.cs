using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    #region Fields

    [HideInInspector] public EnemyData enemyTypeData;

    [HideInInspector] public bool hasEnemy = false;               // Check if it is spawned
    [HideInInspector] public GameObject currentEnemy;             // Tag the Enemy
    [HideInInspector] public bool isRespawning = false;           // Prevent double respawn
    [HideInInspector] public bool hasSpawnedOnce = false;         // Mark if it has already been generated

    [Header("Spawn Rules")]
    public bool isActiveSpawnPoint = false;     // Spawn is activated
    public bool onlySpawnOnce = false;          // Only spawn once

    #endregion

    #region Enemy Assignment

    // Assign Enemy
    // Parameters
    public void AssignEnemy(GameObject enemy)
    {
        currentEnemy = enemy;   // Tag the Enemy
        hasEnemy = true;        // Spawn Enemy True
        isRespawning = false;   // The enemy has returned, reset
    }

    #endregion

    #region Enemy Clearing

    // Clear Enemy
    public void ClearEnemy()
    {
        currentEnemy = null;

        // DO NOT release spawn here!
        // hasEnemy will only be released after respawn!
    }

    // Release spawn after actual respawn
    public void ReleaseSpawnPoint()
    {
        hasEnemy = false;
    }

    #endregion
}