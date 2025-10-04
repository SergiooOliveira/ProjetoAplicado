using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy / New Enemy")]
public class EnemyData : ScriptableObject, IEnemy
{
    #region Serizalized Fields
    [SerializeField] private string enemyName;
    [SerializeField] private string enemyDescription;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private EnemySpawnLevel enemySpawnLevel;
    [SerializeField] private Spell enemySpell;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Stat enemyHp = new Stat(100);
    [SerializeField] private int enemyAttack;
    [SerializeField] private int enemyDefense;
    [SerializeField] private int enemyLevel;
    #endregion

    #region Property implementation
    public string EnemyName => enemyName;
    public string EnemyDescription => enemyDescription;
    public EnemyType EnemyType => enemyType;
    public EnemySpawnLevel EnemySpawnLevel => enemySpawnLevel;
    public Spell EnemySpell => enemySpell;
    public GameObject EnemyPrefab => enemyPrefab;
    public Stat EnemyHp => enemyHp;
    public int EnemyAttack => enemyAttack;
    public int EnemyDefense => enemyDefense;
    public int EnemyLevel => enemyLevel;
    #endregion

    public override string ToString()
    {
        string info = $"{EnemyName}: has {EnemyHp.Max} HP and {EnemyAttack} Damage";

        return info;
    }
}