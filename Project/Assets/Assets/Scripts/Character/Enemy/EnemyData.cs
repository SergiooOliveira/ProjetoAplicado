using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy / New Enemy")]
public class EnemyData : ScriptableObject, IEnemy
{
    #region Serialized Fields
    [Header("Identity")]
    [SerializeField] private string characterName;
    [SerializeField] private int characterLevel;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private string characterDescription;
    [SerializeField] private EnemyType characterType;
    [SerializeField] private EnemySpawnLevel characterSpawnLevel;

    [Header("Stats")]
    [SerializeField] private Stat characterHp;
    [SerializeField] private Stat characterXp;
    [SerializeField] private Stat characterMana;

    [Header("Attributes")]
    [SerializeField] private float characterMovementSpeed;
    [SerializeField] private float characterAttackSpeed;
    [SerializeField] private int characterAttackPower;
    [SerializeField] private int characterDefense;
    [SerializeField] private List<Resistance> enemyResistances;

    [Header("Equipables and Inventory")]
    [SerializeField] private List<Spell> characterEquipedSpells;
    [SerializeField] private List<Item> characterInventory;
    [SerializeField] private List<Item> characterEquipedItems;
    #endregion

    #region Property implementation
    // *----- Identity -----*  
    public string CharacterName => characterName;
    public int CharacterLevel => characterLevel;
    public GameObject CharacterPrefab => characterPrefab;
    public string CharacterDescription => characterDescription;
    public EnemyType CharacterType => characterType;
    public EnemySpawnLevel CharacterSpawnLevel => characterSpawnLevel;

    // *----- Stats -----*
    public Stat CharacterHp => characterHp;
    public Stat CharacterXp => characterXp;
    public Stat CharacterMana => characterMana;

    // *----- Attributes -----*
    public float CharacterMovementSpeed => characterMovementSpeed;
    public float CharacterAttackSpeed => characterAttackSpeed;
    public int CharacterAttackPower => characterAttackPower;
    public int CharacterDefense => characterDefense;
    public List<Resistance> CharacterResistances => CharacterResistances;

    // *----- Equipables and Inventory -----*
    public List<Spell> CharacterEquipedSpells => characterEquipedSpells;
    public List<Item> CharacterInventory => characterInventory;
    public List<Item> CharacterEquipedItems => characterEquipedItems;
    #endregion

    #region Methods (not supported)
    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void AddSpell(Spell spell)
    {
        throw new NotSupportedException ("This class does not support adding spells");
    }

    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void RemoveSpell(int slot, Spell spell)
    {
        throw new NotSupportedException("This class does not support removing spells");
    }

    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void SwapSpell(int slot, Spell spellToRemove, Spell spellToAdd)
    {
        throw new NotSupportedException("This class does not support swapping spells");
    }
    #endregion

    /// <summary>
    /// Small override of ToString()
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string info = $"{CharacterName}: has {CharacterHp.Max} HP and {CharacterAttackSpeed} Damage";

        return info;
    }
}
