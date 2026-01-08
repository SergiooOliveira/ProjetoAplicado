using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData playerData;
    
    private PlayerData runTimePlayerData;

    [SerializeField] private GameObject notificationSlotPrefab;
    [SerializeField] private Transform notificationPanel;
    [SerializeField] private Transform spellSpawnPoint;

    public PlayerHUDManager playerHUDManager;

    #region Property implementation
    public Transform SpellSpawnPoint => spellSpawnPoint;
    #endregion

    #region Unity Methods
    public void Awake ()
    {
        runTimePlayerData = Instantiate(playerData);
        Initialize();
        playerHUDManager = GetComponentInChildren<PlayerHUDManager>();
    }

    public void Start()
    {
        GameManager.Instance.RegisterPlayer(this);
    }

    /// <summary>
    /// Call this method to initialize the player data
    /// </summary>
    public void Initialize()
    {
        runTimePlayerData.CharacterHp.Initialize();
        runTimePlayerData.CharacterMana.Initialize();
        runTimePlayerData.CharacterXp.NewXpMax(GetPlayerLevel());

        // Initialize item list
        foreach (ItemEntry item in runTimePlayerData.CharacterInventory)
        {
            item.item.Initialize();
        }

        // Initialize equipment list

        for (int i = 0; i < runTimePlayerData.CharacterEquipment.Count(); i++)
        {
            EquipmentEntry eq = runTimePlayerData.CharacterEquipment[i];

            eq.equipment.Initialize();

            // Check if its equipped
            if (eq.isEquipped)
            {
                // Check if slot already has something
                // if does unequip
                bool isEquipped = (runTimePlayerData.CharacterEquipedEquipment.Find(e => e.equipment.RunTimeEquipmentData.ItemSlot == eq.equipment.RunTimeEquipmentData.ItemSlot).equipment != null);

                if (isEquipped) runTimePlayerData.UnequipEquipment(eq);
                else runTimePlayerData.EquipEquipment(eq);
            }
        }

        runTimePlayerData.InitializeSpells();
        runTimePlayerData.InitializeEquippedSpells();
    }

    /// <summary>
    /// Public getter for run time player data
    /// </summary>
    public PlayerData RunTimePlayerData => runTimePlayerData;

    public int GetPlayerLevel() => runTimePlayerData.CharacterLevel;

    public void DisplayNotification(string name, int amount)
    {
        GameObject newNotificationSlot = Instantiate(notificationSlotPrefab, notificationPanel);

        Transform nameTransform = newNotificationSlot.transform.Find("ItemName");
        Transform amountTransform = newNotificationSlot.transform.Find("ItemAmount");

        TMP_Text tb_name = nameTransform.GetComponent<TMP_Text>();
        TMP_Text tb_amount = amountTransform.GetComponent<TMP_Text>();

        tb_name.text = "+ " + name;
        tb_amount.text = "x" + amount.ToString();

        Destroy(newNotificationSlot, 2f);
    }
    #endregion

    public float GetAffinityBonuses(SpellAffinity spellAffinity)
    {
        float totalBonus = RunTimePlayerData.CharacterEquipedEquipment
            .Where(entry => entry.isEquipped && entry.equipment?.RunTimeEquipmentData?.ItemDamageAffinity != null)
            .SelectMany(entry => entry.equipment.RunTimeEquipmentData.ItemDamageAffinity)
            .Where(ida => ida.SpellAfinity == spellAffinity)
            .Sum(ida => ida.Amount);

        return 1f + (totalBonus / 100f);
    }
}