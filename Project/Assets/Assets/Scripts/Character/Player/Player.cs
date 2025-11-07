using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData playerData;
    
    private PlayerData runTimePlayerData;

    [SerializeField] private GameObject notificationSlotPrefab;
    [SerializeField] private Transform notificationPanel;

    #region Unity Methods
    public void Awake ()
    {
        runTimePlayerData = Instantiate(playerData);
        Initialize();
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
        runTimePlayerData.ClearSpellList();

        foreach (ItemEntry item in runTimePlayerData.CharacterInventory)
        {
            item.item.Initialize();
        }

        foreach (EquipmentEntry eq in runTimePlayerData.CharacterEquipItems)
        {
            Equipment equipment = eq.equipment;

            equipment.Initialize();

            if (eq.isEquipped)
            {
                // Int and floats
                runTimePlayerData.AddBonusHp(equipment.RunTimeEquipmentData.ItemHpBonus);
                runTimePlayerData.AddBonusAttack(equipment.RunTimeEquipmentData.ItemAttackBonus);
                runTimePlayerData.AddBonusAttackSpeed(equipment.RunTimeEquipmentData.ItemAttackSpeedBonus);
                runTimePlayerData.AddBonusDefense(equipment.RunTimeEquipmentData.ItemDefenseBonus);
                runTimePlayerData.AddBonusMana(equipment.RunTimeEquipmentData.ItemManaBonus);
                runTimePlayerData.AddBonusMovementSpeed(equipment.RunTimeEquipmentData.ItemMovementSpeedBonus);

                // Resistances
                foreach (Resistance resistance in equipment.RunTimeEquipmentData.ItemResistanceBonus)
                {
                    runTimePlayerData.AddResistance(resistance);
                }
            }
        }
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
}