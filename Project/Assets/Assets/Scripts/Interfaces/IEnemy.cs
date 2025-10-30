using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Ground, Flying }

public enum EnemySpawnLevel { Level1, Level2, Level3, Level4 }

public interface IEnemy : ICharacter
{
    #region Properties
    // *----- Identity -----*
    string CharacterDescription { get; }
    EnemyType CharacterType { get; }
    EnemySpawnLevel CharacterSpawnLevel { get; }
    #endregion
}
