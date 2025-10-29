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
    [SerializeField] private List<Resistance> characterResistances;

    [Header("Equipables and Inventory")]
    [SerializeField] private List<Spell> characterEquipedSpells;
    [SerializeField] private List<Item> characterInventory;
    [SerializeField] private List<Equipment> characterEquipedItems;
    [SerializeField] private int characterGold;
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
    public List<Resistance> CharacterResistances => characterResistances;

    // *----- Equipables and Inventory -----*
    public List<Spell> CharacterEquipedSpells => characterEquipedSpells;
    public List<Item> CharacterInventory => characterInventory;
    public List<Equipment> CharacterEquipedItems => characterEquipedItems;
    public int CharacterGold => characterGold;
    #endregion

    #region Spell Methods (not supported)
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

    #region Inventory Methods (not supported)
    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void AddItem(Item item, int quantity)
    {
        throw new NotSupportedException("This class does not support adding items");
    }

    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void RemoveItem(int slot, Item item)
    {
        throw new NotSupportedException("This class does not support removing items");
    }

    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void SellItem(int slot, Item item)
    {
        throw new NotSupportedException("This class does not support selling items");
    }

    public void AddGold(int amount)
    {
        throw new NotSupportedException("This class does not support adding gold");
    }
    #endregion

    #region Equipment Methods
    /// <summary>
    /// Call this method to add the bonus hp from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusHp(int amount)
    {        
        if (amount <= 0) return;
        
        characterHp.IncreaseMaxCurrent(amount);
    }

    /// <summary>
    /// Call this method to add the bonus attack from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusAttack(int amount)
    {
        if (amount <= 0) return;

        characterAttackPower += amount;
    }

    /// <summary>
    /// Call this method to add the bonus attack speed from the equipment to the enemy data
    /// <para>The scaling is multiplicative</para>
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusAttackSpeed(float amount)
    {
        if (amount <= 0) return;

        characterAttackSpeed *= 1 + (amount / 100f);
    }

    /// <summary>
    /// Call this method to add the bonus defense from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusDefense(int amount)
    {
        if (amount <= 0) return;

        characterDefense += amount;
    }

    /// <summary>
    /// Call this method to add the bonus mana from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusMana(int amount)
    {
        if (amount <= 0) return;

        characterMana.IncreaseMaxCurrent(amount);
    }

    /// <summary>
    /// Call this method to add the bonus movement speed from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusMovementSpeed(float amount)
    {
        if (amount <= 0) return;

        characterMovementSpeed *= 1 + (amount / 100f);
    }
    #endregion

    #region Resistance Methods
    /// <summary>
    /// Call this method to add resistances to an enemy
    /// </summary>
    /// <param name="resistance">Resistance to add</param>
    public void AddResistance(Resistance resistance)
    {
        Resistance foundResistance = characterResistances.Find(cr => cr.SpellAfinity == resistance.SpellAfinity);

        // TODO: Define if resistance is flat or a %
        if (foundResistance == null)
        {
            // No resistance in the List
            // Add resistance
            characterResistances.Add(resistance);
        }
        else
        {
            // Resistance already in the list
            // Add amount
            foundResistance.AddAmount(resistance.Amount);
        }
    }
    #endregion

    #region Override
    /// <summary>
    /// Small override of ToString()
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string info = $"{CharacterName}: has {CharacterHp.Max} HP, {CharacterAttackPower} Attack, {CharacterDefense} Defense and {CharacterAttackSpeed:F2} Damage";

        return info;
    }
    #endregion
}
