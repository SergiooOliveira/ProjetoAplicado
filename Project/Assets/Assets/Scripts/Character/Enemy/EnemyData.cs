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
    [Tooltip("Enemy Category")]                                 [SerializeField] private EnemyCategory characterCategory;
    
    [Header("Attacks")]
    [Tooltip("Enemy attacks")]                                  [SerializeField] private List<EnemyAttack> attacks;
    
    [Header("Spawn Settings")]
    [Tooltip("Max enemies alive")]                              [SerializeField] private int spawnCount;
    [Tooltip("Player must be inside this radius to spawn")]     [SerializeField] private float distanceSpawn;
    [Tooltip("Time to respawn after death")]                    [SerializeField] private float respawnTime;
    [Tooltip("Player must be outside this radius to respawn")]  [SerializeField] private float respawnDistance;

    [Header("Stats")]
    [Tooltip("Enemy HP")]                                       [SerializeField] private Stat characterHp;
    [Tooltip("Amount of XP the enemy will drop")]               [SerializeField] private Stat characterXp;
    [Tooltip("Amount of mana the enemy has to use spells")]     [SerializeField] private Stat characterMana;

    [Header("Attributes")]
    [Tooltip("What speed does the enemy move")]                 [SerializeField] private float characterMovementSpeed;
    [Tooltip("What speed does the enemy attack")]               [SerializeField] private float characterAttackSpeed;
    [Tooltip("What is the enemy damage")]                       [SerializeField] private float characterAttackPower;
    [Tooltip("What is the enemy defense")]                      [SerializeField] private float characterDefense;
    [Tooltip("Which resistances does the enemy has")]           [SerializeField] private List<Resistance> characterResistances;

    [Header("Equipables and Inventory")]
    [Tooltip("What spells does the Enemy know")]                [SerializeField] private List<Spell> characterEquipedSpells;
    [Tooltip("What items does the Enemy drops")]                [SerializeField] private List<ItemEntry> characterInventory;
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
    public EnemyCategory CharacterCategory => characterCategory;

    // *----- Attacks -----*  
    public List<EnemyAttack> Attacks => attacks;

    // *----- Spawn Settings -----*
    public int SpawnCount => spawnCount;
    public float DistanceSpawn => distanceSpawn;
    public float RespawnTime => respawnTime;
    public float RespawnDistance => respawnDistance;

    // *----- Stats -----*
    public Stat CharacterHp => characterHp;
    public Stat CharacterXp => characterXp;
    public Stat CharacterMana => characterMana;

    // *----- Attributes -----*
    public float CharacterMovementSpeed => characterMovementSpeed * (1 + totalMovementSpeedBonus / 100f);
    public float CharacterAttackSpeed => characterAttackSpeed * (1 + totalAttackSpeedBonus / 100f);
    public float CharacterAttackPower => characterAttackPower * (1 + totalAttackPower / 100f);
    public float CharacterDefense => characterDefense * (1 + totalDefense / 100f);
    public List<Resistance> CharacterResistances => characterResistances;

    // *----- Equipables and Inventory -----*
    public List<Spell> CharacterSpells => characterEquipedSpells;
    public List<ItemEntry> CharacterInventory => characterInventory;
    public List<EquipmentEntry> CharacterEquipment => characterEquipedItems;
    public int CharacterGold => characterGold;

    // *----- Effects applied -----*
    

    // *----- Internal runtime modifiers -----*
    private float totalAttackSpeedBonus;                                // Character attack speed modifiers
    private float totalMovementSpeedBonus;                              // Character movement speed modifiers
    private float totalAttackPower;                                     // Character attack power modifiers
    private float totalDefense;                                         // Character defense modifiers
    #endregion

    #region Spell Methods (not supported)
    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void AddSpell(Spell spell)
    {
        throw new NotSupportedException("This class does not support adding spells");
    }

    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void EquipSpell(Spell spell)
    {
        throw new NotSupportedException("This class does not support equipping spells");
    }

    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void UnequipSpell(int slot)
    {
        throw new NotSupportedException("This class does not support removing spells");
    }

    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void SwapSpell(Spell spellToRemove, Spell spellToAdd)
    {
        throw new NotSupportedException("This class does not support swapping spells");
    }
    #endregion

    #region Inventory Methods (not supported)
    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public void AddItem(ItemEntry entry, int quantity)
    {
        throw new NotSupportedException("This class does not support adding items");
    }

    /// <summary>
    /// Do not use this method
    /// </summary>
    /// <exception cref="NotSupportedException">Not supported</exception>
    public ItemEntry RemoveItem(ItemEntry entry, int quantity)
    {
        throw new NotSupportedException("This class does not support removing items");
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
    public EquipmentEntry RemoveEquip(EquipmentEntry equipment)
    {
        throw new NotSupportedException("This class does not support removing equipment");
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
    /// Call this method to Equip a new Equipment
    /// </summary>
    /// <param name="equipment">Equipment to add</param>
    public void EquipEquipment(EquipmentEntry equipment)
    {
        equipment.Equip();

        // Give all the stats
        AddEquipmentStats(equipment.equipment.RunTimeEquipmentData);
    }

    /// <summary>
    /// Call this method to Unequip a new Equipment
    /// </summary>
    /// <param name="equipment">Equipment to remove</param>
    public void UnequipEquipment(EquipmentEntry equipment)
    {
        equipment.Unequip();

        RemoveEquipmentStats(equipment.equipment.RunTimeEquipmentData);
    }

    /// <summary>
    /// Call this method to Swap 2 equipments
    /// </summary>
    /// <param name="equipmentToAdd">Equipment to add</param>
    /// <param name="equipmentToRemove">Equipment to remove</param>
    public void SwapEquipment(EquipmentEntry equipmentToAdd, EquipmentEntry equipmentToRemove)
    {
        // Remove Equipment and stats
        UnequipEquipment(equipmentToRemove);

        // Equip new Equipment and stats
        EquipEquipment(equipmentToAdd);
    }
    #endregion

    #region Stat Methods
    /// <summary>
    /// Call this method to give all the equipment stats
    /// </summary>
    public void EquipmentStats()
    {
        foreach (EquipmentEntry entry in characterEquipedItems)
        {            
            if (entry.isEquipped)
            {
                AddEquipmentStats(entry.equipment.RunTimeEquipmentData);
            }
        }
    }

    /// <summary>
    /// Call this method when a new equipment is equiped
    /// </summary>
    /// <param name="equipment">Equipment to add</param>
    public void AddEquipmentStats(EquipmentData equipment)
    {
        AddBonusHp(equipment.ItemHpBonus);
        AddBonusAttack(equipment.ItemAttackBonus);
        AddBonusAttackSpeed(equipment.ItemAttackSpeedBonus);
        AddBonusDefense(equipment.ItemDefenseBonus);
        AddBonusMana(equipment.ItemManaBonus);
        AddBonusMovementSpeed(equipment.ItemMovementSpeedBonus);

        foreach (Resistance resistance in equipment.ItemResistanceBonus)
        {
            AddResistance(resistance);
        }
    }

    /// <summary>
    /// Call this method to unequip an equipment
    /// </summary>
    /// <param name="equipment">Equipment to remove</param>
    public void RemoveEquipmentStats(EquipmentData equipment)
    {
        AddBonusHp(-equipment.ItemHpBonus);
        AddBonusAttack(-equipment.ItemAttackBonus);
        AddBonusAttackSpeed(-equipment.ItemAttackSpeedBonus);
        AddBonusDefense(-equipment.ItemDefenseBonus);
        AddBonusMana(-equipment.ItemManaBonus);
        AddBonusMovementSpeed(-equipment.ItemMovementSpeedBonus);

        // TODO: Resistances needs a way to remove them
        foreach (Resistance resistance in equipment.ItemResistanceBonus)
        {
            AddResistance(resistance);
        }
    }

    #region Additions
    /// <summary>
    /// Call this method to add the bonus hp from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusHp(int amount)
    {
        if (amount == 0) return;

        characterHp.IncreaseMaxCurrent(amount);
    }

    /// <summary>
    /// Call this method to add the bonus attack from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusAttack(float amount)
    {
        if (amount == 0) return;

        totalAttackPower += amount;
    }

    /// <summary>
    /// Call this method to add the bonus attack speed from the equipment to the enemy data
    /// <para>The scaling is multiplicative</para>
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusAttackSpeed(float amount)
    {
        if (amount == 0) return;

        totalAttackSpeedBonus += amount;
    }

    /// <summary>
    /// Call this method to add the bonus defense from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusDefense(float amount)
    {
        if (amount == 0) return;

        totalDefense += amount;
    }

    /// <summary>
    /// Call this method to add the bonus mana from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusMana(int amount)
    {
        if (amount == 0) return;

        characterMana.IncreaseMaxCurrent(amount);
    }

    /// <summary>
    /// Call this method to add the bonus movement speed from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusMovementSpeed(float amount)
    {
        if (amount == 0) return;

        totalMovementSpeedBonus += amount;
    }
    #endregion
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

        if (CharacterEquipment.Count > 0)
        {
            info += " and has equipped: ";
            foreach (EquipmentEntry entry in CharacterEquipment)
            {
                info += entry.equipment.RunTimeEquipmentData.ItemName + " ";
                // See the resistances

                info += " which has ";

                foreach(Resistance resistance in entry.equipment.RunTimeEquipmentData.ItemResistanceBonus)
                {
                    info += resistance.SpellAfinity.ToString() + ": " + resistance.Amount.ToString() + " ";
                }
            }            
        }

        return info;
    }
    #endregion
}