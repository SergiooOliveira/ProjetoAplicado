using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy / New Enemy")]
public class EnemyData : ScriptableObject, IEnemy
{
    #region Serizalized Fields
    [Header("Identity")]
    [SerializeField] private string enemyName;
    [SerializeField] private string enemyDescription;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private EnemySpawnLevel enemySpawnLevel;
    [SerializeField] private Spell enemySpell;
    [SerializeField] private List<Resistance> enemyResistances;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Stats")]
    [SerializeField] private Stat enemyHp;
    [SerializeField] private int enemyAttack;
    [SerializeField] private int enemyDefense;
    [SerializeField] private int enemyLevel;

    [Header("Drops")]
    [SerializeField] private List<Item> enemyDrops;
    #endregion

    #region Property implementation    
    public string EnemyName => enemyName;
    public string EnemyDescription => enemyDescription;
    public EnemyType EnemyType => enemyType;
    public EnemySpawnLevel EnemySpawnLevel => enemySpawnLevel;
    public Spell EnemySpell => enemySpell;
    public List<Resistance> EnemyResistances => enemyResistances;
    public GameObject EnemyPrefab => enemyPrefab;
    public Stat EnemyHp => enemyHp;
    public int EnemyAttack => enemyAttack;
    public int EnemyDefense => enemyDefense;
    public int EnemyLevel => enemyLevel;
    public List<Item> EnemyDrops => enemyDrops;
    #endregion

    // TODO: Need to create an Element Class
    public float GetResistance()
    {
        return 0;
    }

    /// <summary>
    /// Small override of ToString()
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string info = $"{EnemyName}: has {EnemyHp.Max} HP and {EnemyAttack} Damage";

        return info;
    }
}
