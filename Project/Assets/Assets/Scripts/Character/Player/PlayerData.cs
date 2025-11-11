using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu (menuName = "Player/Starter Player")]
public class PlayerData : ScriptableObject, ICharacter
{
    #region Serialized Fields
    [Header("Identity")]
    [SerializeField] private string characterName;                  // Character Name
    [SerializeField] private int characterLevel;                    // Character Level
    [SerializeField] private GameObject characterPrefab;            // Character Prefab

    [Header("Stats")]
    [SerializeField] private Stat characterHp;                      // Character Hp
    [SerializeField] private Stat characterXp;                      // Character Xp
    [SerializeField] private Stat characterMana;                    // Character Mana

    [Header("Attributes")]
    [SerializeField] private float characterMovementSpeed;          // Character Movement Speed
    [SerializeField] private float characterAttackSpeed;            // Character Attack Speed
    [SerializeField] private int characterAttackPower;              // Character Attack Power
    [SerializeField] private int characterDefense;                  // Character Defense
    [SerializeField] private List<Resistance> characterResistances; // Character Resistances

    [Header("Equipables and Inventory")]
    [SerializeField] private List<Spell> characterEquipedSpells;                // Character Equiped Spells
    [SerializeField] private List<ItemEntry> characterInventory;                // Character Inventory (Also, drop table for enemies)
    [SerializeField] private List<EquipmentEntry> characterEquipment;           // Character Equipment (Also, drop table for enemies)
    [SerializeField] private List<EquipmentEntry> characterEquipedEquipment;    // Character Equiped Equipment
    [SerializeField] private int characterGold;                                 // Character Gold
    #endregion

    #region Property implementation
    // *----- Identity -----*
    public string CharacterName => characterName;
    public int CharacterLevel => characterLevel;
    public GameObject CharacterPrefab => characterPrefab;

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
    public List<ItemEntry> CharacterInventory => characterInventory;
    public List<EquipmentEntry> CharacterEquipment => characterEquipment;
    public List<EquipmentEntry> CharacterEquipedEquipment => characterEquipedEquipment;
    public int CharacterGold => characterGold;
    #endregion

    #region Spell Methods
    /// <summary>
    /// Call this method to add a Spell to the player spell book
    /// TODO: Change this to add an int with the slot to place said spell
    /// </summary>
    /// <param name="newSpell"></param>
    public virtual void AddSpell(Spell spell)
    {
        // Firstly check if Spell is already there
        if (CharacterEquipedSpells.Exists(spell => spell.name == spell.name)) return;

        // TODO: Check if the limit is already passed, limit it to 3, and keep the order even if it's deleted. Maybe needs to change to array

        this.CharacterEquipedSpells.Add(spell);

        // If it's the first spell set as selected and save it as an object
        if (CharacterEquipedSpells.Count == 1)
        {
            CharacterEquipedSpells[0].Select();
            SpellManager.Instance.selectedSpell = spell;
        }
        // Debug.Log("Added: " + spell.name);
    }

    /// <summary>
    /// Call this method to remove a Spell from the player spell book
    /// </summary>
    /// <param name="slot">Spell slot in the player spell book</param>
    /// <param name="spell">Spell to remove</param>
    public void RemoveSpell(int slot, Spell spell)
    {

    }

    /// <summary>
    /// Call this method to swap a spell to another spell
    /// </summary>
    /// <param name="slot">Spell slot in the player spell book</param>
    /// <param name="spellToRemove">Spell to remove</param>
    /// <param name="spellToAdd">Spell to add</param>
    public void SwapSpell(int slot, Spell spellToRemove, Spell spellToAdd)
    {
        RemoveSpell(slot, spellToRemove);
        AddSpell(spellToAdd);
    }

    public void ClearSpellList()
    {
        characterEquipedSpells = new List<Spell>();
    }
    #endregion

    #region Inventory Methods
    #region Items
    /// <summary>
    /// Call this method to add an item to the player inventory
    /// </summary>
    /// <param name="entry">Item to add</param>
    /// <param name="amount">Amount</param>
    public void AddItem(ItemEntry entry, int amount)
    {
        // Try to find the item in the List
        int index = characterInventory.FindIndex(i => i.item.RunTimeItemData.ItemName == entry.item.RunTimeItemData.ItemName);

        if (index >= 0)
        {
            // Item alreaddy exists
            ItemEntry existing = characterInventory[index];

            existing.quantity += amount;

            characterInventory[index] = existing;
        }
        else
        {
            // Item not in inventory
            // Instantiate a new Item to avoid conflicts
            Item newItem = ScriptableObject.Instantiate(entry.item);

            newItem.Initialize();

            ItemEntry newEntry = new ItemEntry
            {
                item = newItem,
                quantity = amount
            };

            characterInventory.Add(newEntry);
        }
    }

    /// <summary>
    /// Call this method to sell an item from the player inventory
    /// </summary>
    /// <param name="slot">Slot in the inventory</param>
    /// <param name="item">Item to sell</param>
    public void SellItem(int slot, Item item)
    {
        // slot is useless (?)
    }

    /// <summary>
    /// Call this method to remove an item from the player inventory
    /// </summary>
    /// <param name="slot">Slot in the inventory</param>
    /// <param name="item">Item to remove</param>
    public void RemoveItem(int slot, Item item)
    {
        // slot is useless (?)
    }
    #endregion

    #region Equipment
    /// <summary>
    /// Call this method to add an Equipment to Player inventory
    /// </summary>
    /// <param name="equipment">Equipment to add</param>
    public void AddEquip(EquipmentEntry equipment)
    {
        int index = characterEquipment.FindIndex(e => e.equipment.RunTimeEquipmentData.ItemName == equipment.equipment.RunTimeEquipmentData.ItemName);

        if (index >= 0)
        {
            // Equipment found
            EquipmentEntry existing = characterEquipment[index];
            existing.quantity++;
            characterEquipment[index] = existing;
        }
        else
        {
            // Equipment not found
            Equipment eq = ScriptableObject.Instantiate(equipment.equipment);
            eq.Initialize();

            EquipmentEntry newEntry = new EquipmentEntry
            {
                equipment = eq,
                quantity = 1,
                isEquipped = false
            };

            characterEquipment.Add(newEntry);
        }
    }

    /// <summary>
    /// Call this method to sell an Equipment
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="equipment">Equipment to sell</param>
    public void SellEquip(int slot, Equipment equipment)
    {
        // slot is useless (?)
    }

    /// <summary>
    /// Call this method to remove an Equipment
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="equipment">Equipment to remove</param>
    public void RemoveEquip(int slot, Equipment equipment)
    {
        // slot is useless (?)
    }
    #endregion
    public void AddGold(int amount)
    {
        //Debug.Log($"Adding {amount} gold to player");
        characterGold += amount;
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
        CharacterEquipedEquipment.Add(equipment);
        AddEquipmentStats(equipment.equipment.RunTimeEquipmentData);
    }

    /// <summary>
    /// Call this method to Unequip a new Equipment
    /// </summary>
    /// <param name="equipment">Equipment to remove</param>
    public void UnequipEquipment(EquipmentEntry equipment)
    {
        equipment.Unequip();
        CharacterEquipedEquipment.RemoveAll(e => e.equipment.RunTimeEquipmentData.ItemName == equipment.equipment.RunTimeEquipmentData.ItemName);
        RemoveEquipmentStats(equipment.equipment.RunTimeEquipmentData);
    }

    /// <summary>
    /// Call this method to Swap 2 equipments
    /// </summary>
    /// <param name="equipmentToAdd">Equipment to add</param>
    /// <param name="equipmentToRemove">Equipment to remove</param>
    public void SwapEquipment(EquipmentEntry equipmentToAdd, EquipmentEntry equipmentToRemove)
    {        
        UnequipEquipment(equipmentToRemove);
        EquipEquipment(equipmentToAdd);
    }
    #endregion

    #region Stat Methods
    /// <summary>
    /// Call this method to give all the equiped equipment stats
    /// </summary>
    public void EquipmentStats()
    {
        foreach (EquipmentEntry entry in CharacterEquipedEquipment)
        {
            Debug.Log($"Adding {entry.equipment.RunTimeEquipmentData.ItemName}");
            AddEquipmentStats(entry.equipment.RunTimeEquipmentData);
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

        // Giving equipment resistances from the Player
        foreach (Resistance resistance in equipment.ItemResistanceBonus)
        {
            Resistance playerResistance = characterResistances.Find(pr => pr.SpellAfinity == resistance.SpellAfinity);

            if (playerResistance == null)
                characterResistances.Add(new Resistance(resistance));
            else
                playerResistance.AddAmount(resistance.Amount);
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

        // Removing equipment resistances from the Player
        foreach (Resistance resistance in equipment.ItemResistanceBonus)
        {
            Resistance playerResistance = characterResistances.Find(pr => pr.SpellAfinity == resistance.SpellAfinity);

            playerResistance.AddAmount(-resistance.Amount);
        }
    }

    #region Additions
    /// <summary>
    /// Call this method to add the bonus hp from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusHp(int amount)
    {
        if (amount == 0)
        {
            return;
        }

        characterHp.IncreaseMaxCurrent(amount);
    }

    /// <summary>
    /// Call this method to add the bonus attack from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusAttack(int amount)
    {
        if (amount == 0) return;

        characterAttackPower += amount;
    }

    /// <summary>
    /// Call this method to add the bonus attack speed from the equipment to the enemy data
    /// <para>The scaling is multiplicative</para>
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusAttackSpeed(float amount)
    {
        if (amount == 0) return;

        characterAttackSpeed *= 1 + (amount / 100f);
    }

    /// <summary>
    /// Call this method to add the bonus defense from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusDefense(int amount)
    {
        if (amount == 0) return;

        characterDefense += amount;
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

        characterMovementSpeed *= 1 + (amount / 100f);
    }
    #endregion
    #endregion
}