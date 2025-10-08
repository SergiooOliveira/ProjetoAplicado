using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private List<Spell> characterEquipedSpells;    // Character Equiped Spells
    [SerializeField] private List<Item> characterInventory;         // Character Inventory (Also, drop table for enemies)
    [SerializeField] private List<Item> characterEquipedItems;      // Character EquipedItems (Also, drop table for enemies)
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
    public List<Item> CharacterInventory => characterInventory;
    public List<Item> CharacterEquipedItems => characterEquipedItems;
    #endregion

    #region Methods
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
        Debug.Log("Added: " + spell.name);
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
    #endregion
}