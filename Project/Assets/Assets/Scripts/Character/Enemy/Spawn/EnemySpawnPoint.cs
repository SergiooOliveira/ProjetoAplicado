using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    #region Fields

    public bool hasEnemy = false;               // Check if it is spawned
    public GameObject currentEnemy;             // Tag the Enemy
    public bool isRespawning = false;           // Prevent double respawn
    public bool isActiveSpawnPoint = false;     // Spawn is activated

    [HideInInspector] public EnemyData enemyTypeData;

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
