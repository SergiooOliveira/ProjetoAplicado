using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager: MonoBehaviour
{
    public static EnemyManager Instance;

    [HideInInspector] public List<EnemyData> allEnemies;
    [HideInInspector] public List<EnemyData> activeEnemies;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        // Get all the Scriptable Objects of Enemies
        allEnemies = new List<EnemyData>(Resources.LoadAll<EnemyData>("Enemies"));

        GetAllLevelEnemies(1);
    }

    /// <summary>
    /// Call this method when the player triggers the next level
    /// </summary>
    public void SpawnEnemies(int level)
    {
        GetAllLevelEnemies(level);
    }

    /// <summary>
    /// Call this method to get all the enemies from a level
    /// </summary>
    /// <param name="level">Level of an enemy</param>
    /// <returns>List of all enemies</returns>
    private void GetAllLevelEnemies(int level)
    {
        List<EnemyData> allLevelEnemies = allEnemies.FindAll(enemy => enemy.EnemyLevel == level);

        foreach (EnemyData enemy in allLevelEnemies)
            activeEnemies.Add(enemy);
    }

    /// <summary>
    /// Call this method to remove and destroy an Enemy
    /// </summary>
    /// <param name="enemy">Enemy object</param>
    public void RemoveEnemy(Enemy enemy)
    {
        activeEnemies.Remove(enemy.enemyData);
        Destroy(enemy.gameObject);
    }
}