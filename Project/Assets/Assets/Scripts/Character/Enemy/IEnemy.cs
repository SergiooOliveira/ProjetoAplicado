using UnityEngine;

public enum EnemyType { Ground, Flying }

public enum EnemySpawnLevel { Level1, Level2, Level3, Level4 }

// TODO: Maybe do an interface for all Characters
public interface IEnemy
{
    #region Properties
    // *----- Identity -----*
    string EnemyName { get; }
    string EnemyDescription { get; }
    EnemyType EnemyType { get; }
    EnemySpawnLevel EnemySpawnLevel { get; }
    Spell EnemySpell { get; }
    GameObject EnemyPrefab { get; }

    // *----- Stats -----*
    Stat EnemyHp { get; }
    int EnemyAttack { get; }
    int EnemyDefense { get; }
    int EnemyLevel { get; }
    #endregion

    #region Methods
    void Move();
    void Attack();
    void UseSpell();
    #endregion
}