using System;
using System.Collections.Generic;
using UnityEngine;
using static ICharacter;

[CreateAssetMenu(menuName = "Enemy / New Enemy")]
public class EnemyData : ScriptableObject, IEnemy
{
    #region Serialized Fields
    [Header("Identity")]
    [Tooltip("Name of the Enemy")]                              [SerializeField] private string characterName;
    [Tooltip("Level of the Enemy")]                             [SerializeField] private int characterLevel;
    [Tooltip("Enemy Prefab")]                                   [SerializeField] private GameObject characterPrefab;
    [Tooltip("Description of the Enemy")]                       [SerializeField] private string characterDescription;
    [Tooltip("Type of the Enemy")]                              [SerializeField] private EnemyType characterType;
    [Tooltip("Where does the Enemy spawn")]                     [SerializeField] private EnemySpawnLevel characterSpawnLevel;

    [Header("Stats")]
    [Tooltip("Enemy HP")]                                       [SerializeField] private Stat characterHp;
    [Tooltip("Amount of XP the enemy will drop")]               [SerializeField] private Stat characterXp;
    [Tooltip("Amount of mana the enemy has to use spells")]     [SerializeField] private Stat characterMana;

    [Header("Attributes")]
    [Tooltip("What speed does the enemy move")]                 [SerializeField] private float characterMovementSpeed;
    [Tooltip("What speed does the enemy attack")]               [SerializeField] private float characterAttackSpeed;
    [Tooltip("What is the enemy damage")]                       [SerializeField] private int characterAttackPower;
    [Tooltip("What is the enemy defense")]                      [SerializeField] private int characterDefense;
    [Tooltip("Which resistances does the enemy has")]           [SerializeField] private List<Resistance> characterResistances;

    [Header("Equipables and Inventory")]
    [Tooltip("What spells does the Enemy know")]                [SerializeField] private List<Spell> characterEquipedSpells;
    [Tooltip("What items does the Enemy drops")]                [SerializeField] private List<InventoryItem> characterInventory;
    [Tooltip("What equipments does the Enemy drops")]           [SerializeField] private List<EquipmentEntry> characterEquipedItems;
    [Tooltip("Amount of gold the enemy will drop")]             [SerializeField] private int characterGold;
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
    public List<InventoryItem> CharacterInventory => characterInventory;
    public List<EquipmentEntry> CharacterEquipItems => characterEquipedItems;
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
    public void AddItem(InventoryItem entry, int quantity)
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

    /// <summary>
    /// Do not use this method
    /// </summary>    
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void AddEquip(EquipmentEntry equipment)
    {
        throw new NotSupportedException("This class does not support adding equipment");
    }

    /// <summary>
    /// Do not use this method
    /// </summary>    
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void RemoveEquip(int slot, Equipment equipment)
    {
        throw new NotSupportedException("This class does not support removing equipment");
    }

    /// <summary>
    /// Do not use this method
    /// </summary>    
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void SellEquip(int slot, Equipment equipment)
    {
        throw new NotSupportedException("This class does not support selling equipment");
    }

    /// <summary>
    /// Do not use this method
    /// </summary>    
    /// <exception cref="NotSupportedException">Not supported</exception>
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
        string info = $"{CharacterName}: has {CharacterHp.Current} HP, {CharacterAttackPower} Attack, {CharacterDefense} Defense and {CharacterAttackSpeed:F2} Damage";

        return info;
    }
    #endregion
}
