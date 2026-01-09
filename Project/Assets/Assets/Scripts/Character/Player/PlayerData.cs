using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu (menuName = "Player/Starter Player")]
public class PlayerData : ScriptableObject, ICharacter
{
    #region Serialized Fields
    [Header("Identity")]
    [SerializeField] private string characterName;                              // Character Name
    [SerializeField] private int characterLevel;                                // Character Level
    [SerializeField] private GameObject characterPrefab;                        // Character Prefab

    [Header("Stats")]
    [SerializeField] private Stat characterHp;                                  // Character Hp
    [SerializeField] private Stat characterXp;                                  // Character Xp
    [SerializeField] private Stat characterMana;                                // Character Mana

    [Header("Mana Regeneration")]
    [SerializeField] private float regenDelay = 5f;                             // Delay before mana regeneration starts
    [SerializeField] private float regenTickRate = 0.1f;                        // Mana regeneration rate per second
    [SerializeField] private float regenAmount = 5f;                            // Amount of mana regenerated per tick

    [Header("Attributes")]
    [SerializeField] private float characterMovementSpeed;                      // Character Movement Speed
    [SerializeField] private float characterAttackSpeed;                        // Character Attack Speed
    [SerializeField] private float characterAttackPower;                        // Character Attack Power
    [SerializeField] private float characterDefense;                            // Character Defense
    [SerializeField] private List<Resistance> characterResistances;             // Character Resistances

    [Header("Spells and Inventory")]
    [SerializeField] private List<Spell> characterSpells;                       // Character learned Spells
    [SerializeField] private List<SpellEntry> characterEquippedSpells;          // Character equipped Spells
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

    // *----- Mana Regeneration -----*
    public float RegenDelay => regenDelay;
    public float RegenTickRate => regenTickRate;
    public float RegenAmount => regenAmount;

    // *----- Attributes -----*
    public float CharacterMovementSpeed => characterMovementSpeed * (1 + totalMovementSpeedBonus / 100f);
    public float CharacterAttackSpeed => characterAttackSpeed * (1 + totalAttackSpeedBonus / 100f);
    public float CharacterAttackPower => characterAttackPower;
    public float CharacterDefense => characterDefense;
    public List<Resistance> CharacterResistances => characterResistances;

    // *----- Spells and Inventory -----*
    public List<Spell> CharacterSpells => characterSpells;
    public List<SpellEntry> CharacterEquippedSpells => characterEquippedSpells;
    public List<ItemEntry> CharacterInventory => characterInventory;
    public List<EquipmentEntry> CharacterEquipment => characterEquipment;
    public List<EquipmentEntry> CharacterEquipedEquipment => characterEquipedEquipment;
    public int CharacterGold => characterGold;

    // *----- Internal runtime modifiers -----*
    private float totalAttackSpeedBonus;                            // Character attack speed modifiers
    private float totalMovementSpeedBonus;                          // Character movement speed modifiers
    #endregion

    #region Spell Methods
    public void InitializeSpells()
    {
        foreach (Spell spell in CharacterSpells)
        {
            spell.InitializeRuntimeData();
        }
    }
    
    public void InitializeEquippedSpells()
    {
        CharacterEquippedSpells.Clear();

        CharacterEquippedSpells.Add(new SpellEntry { spell = null, slot = 1, isSelected = false });
        CharacterEquippedSpells.Add(new SpellEntry { spell = null, slot = 2, isSelected = false });
        CharacterEquippedSpells.Add(new SpellEntry { spell = null, slot = 3, isSelected = false });
    }

    /// <summary>
    /// Call this method to add a Spell to the player spell book
    /// </summary>
    /// <param name="newSpell"></param>
    public void AddSpell(Spell spell)
    {
        // Firstly check if Spell is already there
        if (CharacterSpells.Exists(s => s.RuntimeSpellData.SpellName == spell.RuntimeSpellData.SpellName)) return;

        // Spell doesnst exist in the List
        this.CharacterSpells.Add(spell);
    }

    /// <summary>
    /// Call this method to equip a spell
    /// </summary>
    /// <param name="spell">Spell to add</param>
    public void EquipSpell(Spell spell)
    {
        bool alreadyEquipped = CharacterEquippedSpells.Any(s => s.spell != null && s.spell.RuntimeSpellData.SpellName == spell.RuntimeSpellData.SpellName);

        // Check if spell is already equipped
        if (alreadyEquipped)
        {
            Debug.Log($"{spell.RuntimeSpellData.SpellName} is already equipped");
            return;
        }

        bool hasAnyEquipped = CharacterEquippedSpells.Any(s => s.spell != null);

        // Find first empty slot
        for (int i = 0; i < 3; i++)
        {
            if (SlotIsEmpty(i))
            {
                SpellEntry entry = new SpellEntry
                {
                    slot = i,
                    spell = spell,
                    isSelected = !hasAnyEquipped
                };

                SetSlot(i, entry);

                return;
            }
        }
    }

    /// <summary>
    /// Call this method to remove a Spell from the player spell book
    /// </summary>
    /// <param name="slot">Spell slot in the player spell book</param>
    /// <param name="spell">Spell to remove</param>
    public void UnequipSpell(int slot)
    {
        SpellEntry entry = GetSlot(slot);

        if (entry.spell == null)
            return;

        bool wasSelected = entry.isSelected;

        entry.spell = null;
        entry.isSelected = false;
        SetSlot(slot, entry);

        if (wasSelected) SelectNextAvailableSpell();
    }

    /// <summary>
    /// Call this method to swap a spell to another spell
    /// </summary>
    /// <param name="slot">Spell slot in the player spell book</param>
    /// <param name="spellToRemove">Spell to remove</param>
    /// <param name="spellToAdd">Spell to add</param>
    public void SwapSpell(Spell spellToRemove, Spell spellToAdd)
    {
        //UnequipSpell(spellToRemove);
        //AddSpell(spellToAdd);
    }

    public void SwapActiveSpell(SpellEntry activeSpell, SpellEntry newActiveSpell)
    {
        if (activeSpell.spell == null)
        {
            Debug.Log("Active spell is null");
        }
        else if (newActiveSpell.spell == null)
        {
            Debug.Log("New active Spell is null");
        } else
        {
            //Debug.LogWarning($"Swaping {activeSpell.spell.RuntimeSpellData.SpellName} with {newActiveSpell.spell.RuntimeSpellData.SpellName}");            
            activeSpell.Deselect();
            newActiveSpell.Select();

            SetSlot(activeSpell.slot, activeSpell);
            SetSlot(newActiveSpell.slot, newActiveSpell);

            //foreach(SpellEntry entry in CharacterEquippedSpells)
            //{
            //    if (entry.spell  != null)
            //        Debug.Log($"<Color=green>{entry.spell.RuntimeSpellData.SpellName}: {entry.isSelected}</Color>");
            //}            
        }
    }

    public void ClearSpellList()
    {
        characterSpells = new List<Spell>();
    }

    #region Auxiliary methods
    public SpellEntry GetSlot(int slot)
    {
        //Debug.LogWarning($"CharacterEquippedSpells[{slot}].IsEmpty: {CharacterEquippedSpells[slot].IsEmpty}");
        return CharacterEquippedSpells[slot];
    }

    public void SetSlot(int slot, SpellEntry entry)
    {
        CharacterEquippedSpells[slot] = entry;
    }

    public bool SlotIsEmpty(int slot)
    {
        return CharacterEquippedSpells[slot].spell == null;
    }

    public int GetActiveSpellIndex()
    {
        SpellEntry activeSpell = CharacterEquippedSpells.Find(s => s.isSelected == true);

        if (activeSpell.spell != null) return activeSpell.slot;
        else return -1;
    }

    private void SelectNextAvailableSpell()
    {
        for (int i = 0; i < CharacterEquippedSpells.Count; i++)
        {
            SpellEntry entry = GetSlot(i);

            if (entry.spell != null)
            {
                entry.isSelected = true;
                SetSlot(i, entry);
                return;
            }
        }
    }

    public bool IsSpellEquipped(Spell spell)
    {
        return CharacterEquippedSpells
            .Exists(s => s.spell != null && s.spell.RuntimeSpellData.SpellName == spell.RuntimeSpellData.SpellName);
    }

    public int GetSpellSlot(Spell spell)
    {
        return CharacterEquippedSpells
            .FindIndex(s => s.spell != null && s.spell.RuntimeSpellData.SpellName == spell.RuntimeSpellData.SpellName);
    }
    #endregion
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
            // Item already exists
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
    /// Call this method to remove an item from the player inventory
    /// </summary>
    /// <param name="slot">Slot in the inventory</param>
    /// <param name="item">Item to remove</param>
    public ItemEntry RemoveItem(ItemEntry entry, int amount)
    {
        int index = characterInventory.FindIndex(i => i.item.RunTimeItemData.ItemName == entry.item.RunTimeItemData.ItemName);

        ItemEntry item = characterInventory[index];

        item.RemoveQuantity(amount);

        if (item.quantity <= 0) characterInventory.RemoveAt(index);
        else characterInventory[index] = item;

        return item;
    }

    public void RemoveItem(Item item, int amount)
    {
        int index = characterInventory.FindIndex(i => i.item.RunTimeItemData.ItemName == item.RunTimeItemData.ItemName);

        ItemEntry entry = characterInventory[index];
        entry.RemoveQuantity(amount);

        if (entry.quantity <= 0) characterInventory.RemoveAt(index);
        else characterInventory[index] = entry;
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
            //EquipmentData
            equipment.equipment.Initialize();

            EquipmentEntry newEntry = new EquipmentEntry
            {
                equipment = equipment.equipment,
                quantity = 1,
                isEquipped = false
            };

            characterEquipment.Add(newEntry);
        }
    }

    /// <summary>
    /// Call this method to remove an Equipment
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="equipment">Equipment to remove</param>
    public EquipmentEntry RemoveEquip(EquipmentEntry entry)
    {
        int index = characterEquipment.FindIndex(e => e.equipment.RunTimeEquipmentData.ItemName == entry.equipment.RunTimeEquipmentData.ItemName);

        EquipmentEntry equipment = characterEquipment[index];

        equipment.RemoveQuantity(1);

        if (equipment.quantity <= 0) characterEquipment.RemoveAt(index);
        else characterEquipment[index] = equipment;

        return equipment;
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
        // Get index and EquipmentEntry
        int index = CharacterEquipment.FindIndex(e => e.equipment.RunTimeEquipmentData.ItemName == equipment.equipment.RunTimeEquipmentData.ItemName);
        EquipmentEntry eq = CharacterEquipment[index];
        eq.Equip();

        //Debug.Log($"Equipping {eq.equipment.RunTimeEquipmentData.ItemName}");
        
        CharacterEquipedEquipment.Add(eq);
        AddEquipmentStats(eq.equipment.RunTimeEquipmentData);

        CharacterEquipment[index] = eq;
    }

    /// <summary>
    /// Call this method to Unequip a new Equipment
    /// </summary>
    /// <param name="equipment">Equipment to remove</param>
    public void UnequipEquipment(EquipmentEntry equipment)
    {
        // Get index and EquipmentEntry
        int index = CharacterEquipment.FindIndex(e => e.equipment.RunTimeEquipmentData.ItemName == equipment.equipment.RunTimeEquipmentData.ItemName);
        EquipmentEntry eq = CharacterEquipment[index];

        // Unequip
        eq.Unequip();

        // Remove from EquipedEquipment List and remove Stats
        CharacterEquipedEquipment.RemoveAll(e => e.equipment.RunTimeEquipmentData.ItemName == eq.equipment.RunTimeEquipmentData.ItemName);
        RemoveEquipmentStats(eq.equipment.RunTimeEquipmentData);

        // Reasign EquipmentEntry to List
        CharacterEquipment[index] = eq;
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
            //Debug.Log($"Adding {entry.equipment.RunTimeEquipmentData.ItemName}");
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
    public void AddBonusAttack(float amount)
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

        totalAttackSpeedBonus += amount;
    }

    /// <summary>
    /// Call this method to add the bonus defense from the equipment to the enemy data
    /// </summary>
    /// <param name="amount">Quantity to add</param>
    public void AddBonusDefense(float amount)
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

        totalMovementSpeedBonus += amount;        
    }
    #endregion
    #endregion

    #region XP Methods
    public void LevelUP()
    {
        characterLevel++;
    }
    #endregion
}