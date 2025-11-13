using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StatManagerUI : MonoBehaviour
{
    #region Variables
    private PlayerData player;

    #region Armor
    [Header("Armor")]
    [Header("Helmet")]
    [SerializeField] private Image img_helmetSprite;
    [SerializeField] private TMP_Text tb_helmetName;
    [SerializeField] private TMP_Text tb_helmetSetName;
    [SerializeField] private TMP_Text tb_helmetStat;
    [SerializeField] private TMP_Text tb_helmetResist;

    [Header("Chestplate")]
    [SerializeField] private Image img_chestplateSprite;
    [SerializeField] private TMP_Text tb_chestplateName;
    [SerializeField] private TMP_Text tb_chestplateSetName;
    [SerializeField] private TMP_Text tb_chestplateStat;
    [SerializeField] private TMP_Text tb_chestplateResist;

    [Header("Leggings")]
    [SerializeField] private Image img_leggingsSprite;
    [SerializeField] private TMP_Text tb_leggingsName;
    [SerializeField] private TMP_Text tb_leggingsSetName;
    [SerializeField] private TMP_Text tb_leggingsStat;
    [SerializeField] private TMP_Text tb_leggingsResist;
    #endregion

    #region Accessories/Weapon
    [Space(20)]
    [Header("Accessories/Weapon")]
    [Header("Amulet")]
    [SerializeField] private Image img_amuletSprite;
    [SerializeField] private TMP_Text tb_amuletName;
    [SerializeField] private TMP_Text tb_amuletStat;

    [Header("Ring")]
    [SerializeField] private Image img_ringSprite;
    [SerializeField] private TMP_Text tb_ringName;
    [SerializeField] private TMP_Text tb_ringStat;

    [Header("Weapon")]
    [SerializeField] private Image img_weaponSprite;
    [SerializeField] private TMP_Text tb_weaponName;
    [SerializeField] private TMP_Text tb_weaponStat;
    #endregion

    #region Stats
    [Space(20)]
    [Header("Simple Stats")]
    [SerializeField] private TMP_Text hp;
    [SerializeField] private TMP_Text attack;
    [SerializeField] private TMP_Text attackSpeed;
    [SerializeField] private TMP_Text defense;
    [SerializeField] private TMP_Text movementSpeed;

    [Header("Resistances Stats")]
    [SerializeField] private TMP_Text fireResistance;
    [SerializeField] private TMP_Text iceResistance;
    [SerializeField] private TMP_Text windResistance;
    [SerializeField] private TMP_Text lightResistance;
    [SerializeField] private TMP_Text darkResistance;
    #endregion

    #region Const
    private const string fireSet = "Emberheart Battlemage Set";
    private const string iceSet = "Glacial Sage Set";
    private const string windSet = "Tempest Dancer Set";
    private const string lightSet = "Radiant Seraph Set";
    private const string darkSet = "Void Reaper Set";    
    #endregion
    #endregion

    private void Awake()
    {
        player = GetComponentInParent<Player>().RunTimePlayerData;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    #region Updaters
    public void UpdateUI()
    {
        UpdateSimpleStats();
        UpdateResistanceStats();
        UpdateEquipedEquipment();
    }

    /// <summary>
    /// Call this method to update the simple stats of the player
    /// </summary>
    private void UpdateSimpleStats()
    {
        hp.text = player.CharacterHp.Current.ToString() + "/" + player.CharacterHp.Max.ToString();
        attack.text = player.CharacterAttackPower.ToString();
        attackSpeed.text = player.CharacterAttackSpeed.ToString();
        defense.text = player.CharacterDefense.ToString();
        movementSpeed.text = player.CharacterMovementSpeed.ToString();
    }

    /// <summary>
    /// Call this method to update the resistance stats of the player
    /// </summary>
    private void UpdateResistanceStats()
    {
        foreach (Resistance resistance in player.CharacterResistances)
        {
            switch (resistance.SpellAfinity)
            {
                case SpellAffinity.Fire:
                    fireResistance.text = resistance.Amount.ToString() + " %";
                    break;
                case SpellAffinity.Ice:
                    iceResistance.text = resistance.Amount.ToString() + " %";
                    break;
                case SpellAffinity.Wind:
                    windResistance.text = resistance.Amount.ToString() + " %";
                    break;
                case SpellAffinity.Light:
                    lightResistance.text = resistance.Amount.ToString() + " %";
                    break;
                case SpellAffinity.Dark:
                    darkResistance.text = resistance.Amount.ToString() + " %";
                    break;
            }
        }
    }

    /// <summary>
    /// Call this method to update the Equipment section
    /// </summary>
    public void UpdateEquipedEquipment()
    {
        /*
         * Update by order of the variables
         * Helmet - Chestplate - Leggings - Amulet - Ring - Weapon
         * Maybe create some sub-methods to handdle it better
         */
        foreach (EquipmentEntry entry in player.CharacterEquipedEquipment)
        {
            switch (entry.equipment.RunTimeEquipmentData.ItemSlot)
            {
                case EquipmentSlot.Helmet:
                    HelmetObject(entry);
                    break;
                case EquipmentSlot.Chestplate:
                    ChestplateObject(entry);
                    break;
                case EquipmentSlot.Leggings:
                    LeggingsObject(entry);
                    break;
                case EquipmentSlot.Amulet:
                    AmuletObject(entry);
                    break;
                case EquipmentSlot.Ring:
                    RingObject(entry);
                    break;
                case EquipmentSlot.Weapon:
                    WeaponObject(entry);
                    break;
            }
        }
    }
    #endregion

    #region Equipment auxiliary methods
    /// <summary>
    /// Call this method to update the helmet UI
    /// </summary>
    /// <param name="e">Helmet</param>
    private void HelmetObject(EquipmentEntry e)
    {
        EquipmentData entry = e.equipment.RunTimeEquipmentData;
        
        img_helmetSprite.sprite = entry.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_helmetName.text = entry.ItemName;
        tb_helmetSetName.text = GetItemSet(entry.ItemSet);
        tb_helmetStat.text = GetItemStats(entry);
        tb_helmetResist.text = GetItemResistances(entry);
    }

    /// <summary>
    /// Call this method to update the chestplate UI
    /// </summary>
    /// <param name="e">Chestplate</param>
    private void ChestplateObject(EquipmentEntry e)
    {
        EquipmentData entry = e.equipment.RunTimeEquipmentData;

        img_chestplateSprite.sprite = entry.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_chestplateName.text = entry.ItemName;
        tb_chestplateSetName.text = GetItemSet(entry.ItemSet);
        tb_chestplateStat.text = GetItemStats(entry);
        tb_chestplateResist.text = GetItemResistances(entry);
    }

    /// <summary>
    /// Call this method to update the leggings UI
    /// </summary>
    /// <param name="e">Leggings</param>
    private void LeggingsObject(EquipmentEntry e)
    {
        EquipmentData entry = e.equipment.RunTimeEquipmentData;

        img_leggingsSprite.sprite = entry.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_leggingsName.text = entry.ItemName;
        tb_leggingsSetName.text = GetItemSet(entry.ItemSet);
        tb_leggingsStat.text = GetItemStats(entry);
        tb_leggingsResist.text = GetItemResistances(entry);
    }

    /// <summary>
    /// Call this method to update the amulet UI
    /// </summary>
    /// <param name="e">Amulet</param>
    private void AmuletObject(EquipmentEntry e)
    {
        EquipmentData entry = e.equipment.RunTimeEquipmentData;

        img_amuletSprite.sprite = entry.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_amuletName.text = entry.ItemName;
        tb_amuletStat.text = GetItemStats(entry) + GetItemResistances(entry) + GetItemDamageAffinity(entry);
    }

    /// <summary>
    /// Call this method to update the ring UI
    /// </summary>
    /// <param name="e">Ring</param>
    private void RingObject(EquipmentEntry e)
    {
        EquipmentData entry = e.equipment.RunTimeEquipmentData;

        img_ringSprite.sprite = entry.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_ringName.text = entry.ItemName;
        tb_ringStat.text = GetItemStats(entry) + GetItemResistances(entry) + GetItemDamageAffinity(entry);
    }

    /// <summary>
    /// Call this method to update the weapon UI
    /// </summary>
    /// <param name="e">Weapon</param>
    private void WeaponObject(EquipmentEntry e)
    {
        EquipmentData entry = e.equipment.RunTimeEquipmentData;

        img_weaponSprite.sprite = entry.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_weaponName.text = entry.ItemName;
        tb_weaponStat.text = GetItemStats(entry) + GetItemResistances(entry) + GetItemDamageAffinity(entry);
    }
    #endregion

    #region Auxiliary methods
    /// <summary>
    /// Call this method to get the item set of item
    /// </summary>
    /// <param name="equipment">Equipment to get the set</param>
    /// <returns>Set name in string</returns>
    private string GetItemSet(EquipmentSet equipment)
    {
        switch (equipment)
        {
            case EquipmentSet.Fire:     return fireSet;
            case EquipmentSet.Ice:      return iceSet;
            case EquipmentSet.Wind:     return windSet;
            case EquipmentSet.Light:    return lightSet;
            case EquipmentSet.Dark:     return darkSet;
            default: return "";
        }
    }

    /// <summary>
    /// Call this method to get the item stats
    /// </summary>
    /// <param name="equipment">Equipment to get the stats</param>
    /// <returns>All item stats in string</returns>
    private string GetItemStats(EquipmentData equipment)
    {
        string stat = "";

        if (equipment.ItemHpBonus != 0)
            stat += $"{(equipment.ItemHpBonus > 0 ? "+ " : "- ")}{Mathf.Abs(equipment.ItemHpBonus)} Hp\n";

        if (equipment.ItemAttackBonus != 0)
            stat += $"{(equipment.ItemAttackBonus > 0 ? "+ " : "- ")}{Mathf.Abs(equipment.ItemAttackBonus)} Attack\n";

        if (equipment.ItemAttackSpeedBonus != 0)
            stat += $"{(equipment.ItemAttackSpeedBonus > 0 ? "+ " : "- ")}{Mathf.Abs(equipment.ItemAttackSpeedBonus):F2} Attack Speed\n";

        if (equipment.ItemDefenseBonus != 0)
            stat += $"{(equipment.ItemDefenseBonus > 0 ? "+ " : "- ")}{Mathf.Abs(equipment.ItemDefenseBonus)} Defense\n";

        if (equipment.ItemManaBonus != 0)
            stat += $"{(equipment.ItemManaBonus > 0 ? "+ " : "- ")}{Mathf.Abs(equipment.ItemManaBonus)} Mana\n";

        if (equipment.ItemMovementSpeedBonus != 0)
            stat += $"{(equipment.ItemMovementSpeedBonus > 0 ? "+ " : "- ")}{Mathf.Abs(equipment.ItemMovementSpeedBonus):F2} Movement Speed\n";

        // Also add the item damage afinities
        stat += GetItemDamageAffinity(equipment);

        return stat;
    }

    /// <summary>
    /// Call this method to get the damage affinities of an equipment
    /// </summary>
    /// <param name="equipment">Equipment to get the affinities</param>
    /// <returns>String with all affinities</returns>
    private string GetItemDamageAffinity(EquipmentData equipment)
    {
        string affinities = "";

        // Go through all the affinities and add them to the string
        foreach (Resistance r in equipment.ItemDamageAffinity)
        {
            affinities += $"{(r.Amount > 0 ? "+ " : "- ")}{Mathf.Abs(r.Amount)} {r.SpellAfinity} Damage\n";
        }

        return affinities;
    }

    /// <summary>
    /// Call this method to get the item resistances
    /// </summary>
    /// <param name="equipment">Equipment to get the resistances</param>
    /// <returns>All item resistances in string</returns>
    private string GetItemResistances(EquipmentData equipment)
    {
        string resistances = string.Empty;

        foreach (Resistance r in equipment.ItemResistanceBonus)
        {
            resistances += $"{(r.Amount > 0 ? "+ " : "- ")}{Mathf.Abs(r.Amount)} {r.SpellAfinity} Resistance\n";
        }

        return resistances;
    }
    #endregion
}
