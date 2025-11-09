using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

#region Fields / Enums

public enum EnemyType { Ground, Flying, Floating }

public enum EnemySpawnLevel { Level1, Level2, Level3, Level4 }

public enum AttackType { Melee, Ranged }

public enum EnemyCategory { Warrior, Necromancer, CyclopeBat, Boss }

#endregion

public interface IEnemy : ICharacter
{
    #region Properties
    // *----- Identity -----*
    string CharacterDescription { get; }
    EnemyType CharacterType { get; }
    EnemySpawnLevel CharacterSpawnLevel { get; }
    EnemyCategory CharacterCategory { get; }
    #endregion
}