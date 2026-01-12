using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    #region Variables
    [Header("Slot components")]
    [SerializeField] private TMP_Text tb_Name;
    [SerializeField] private TMP_Text tb_Amount;

    [Header("Tooltips")]
    [SerializeField] private GameObject itemTooltipObject;
    [SerializeField] private GameObject equipmentTooltipObject;

    [Header("Upgrade")]
    [SerializeField] private GameObject upgradeObject;
    private GameObject upgradeInstance;

    private PlayerData player;

    // Types of Entry
    private ItemEntry item;
    private EquipmentEntry equipment;
    
    private GameObject tooltipInstance;
    private StatManagerUI statManagerUI;
    private InventoryManagerUI inventoryManagerUI;

    // For positioning
    private Canvas canvas;
    private RectTransform panel;
    private bool isHovering;
    #endregion

    #region Unity MonoBehaviours
    private void Awake()
    {
        player = GetComponentInParent<Player>().RunTimePlayerData;
        statManagerUI = GetComponentInParent<StatManagerUI>();
        inventoryManagerUI = GetComponentInParent<InventoryManagerUI>();
    }

    private void Update()
    {
        if (isHovering && tooltipInstance != null)
            UpdateTooltipPosition();
    }

    private void OnDisable()
    {
        DestroyTooltip();
    }
    #endregion

    #region Inventory list setters
    /// <summary>
    /// Call this method to set the text values in the item slot
    /// </summary>
    /// <param name="i"></param>
    public void SetItemSlot(ItemEntry i)
    {
        item = i;

        if (item.item == null) return;

        tb_Name.text = item.item.RunTimeItemData.ItemName;
        tb_Amount.text = item.quantity.ToString();
    }

    /// <summary>
    /// Call this method to set the text values in the equipment slot
    /// </summary>
    /// <param name="e"></param>
    public void SetEquipmentSlot(EquipmentEntry e)
    {
        equipment = e;

        if (equipment.equipment == null) return;

        tb_Name.text = equipment.equipment.RunTimeEquipmentData.ItemName;
        tb_Amount.text = equipment.quantity.ToString();
    }
    #endregion

    #region OnPointer methods
    /// <summary>
    /// This method is called when the mouse is hover an element
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // If both null, do nothing
        if (item.item == null && equipment.equipment == null) return;

        // Display the correct tooltip
        if (item.item != null) DisplayItemTooltip();
        if (equipment.equipment != null) DisplayEquipmentTooltip();
    }

    /// <summary>
    /// This method is called when the mouse stops hovering an element
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyTooltip();
    }

    /// <summary>
    /// This method is called when the mouse clicks an element
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Read the right click
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            /* Check if its an equipment
             * If its an equipment check if that equipment slot is empty
             * Is empty? Cool, just set the Equip tag on
             * Is not empty? Show a little pop up to ask, swap equipment? Then, yes or no 
             */

            // Only equipments
            if (item.item != null) return;

            // Check what type of slot it has
            /*
             * 1 - Helmet, Chestplate, Leggings, Weapon
             * 2 - Accessories
             */

            // Get the playerData from parent
            EquipmentEntry equippedEquipment = player.CharacterEquipedEquipment.SingleOrDefault(e => e.equipment.RunTimeEquipmentData.ItemSlot == equipment.equipment.RunTimeEquipmentData.ItemSlot);
            Debug.Log($"Equipped equipment in that slot: {equippedEquipment.equipment?.RunTimeEquipmentData.ItemName}");
            if (equippedEquipment.equipment == null)
            {
                Debug.Log("equippedEquipment is null");
                
                player.EquipEquipment(equipment);
            }
            else
            {
                if (equippedEquipment.equipment.RunTimeEquipmentData.ItemName == equipment.equipment.RunTimeEquipmentData.ItemName)
                {
                    player.UnequipEquipment(equipment);
                }
                else
                {
                    // Swap
                    player.SwapEquipment(equipment, equippedEquipment);
                }
            }

            // Update UI
            statManagerUI.UpdateEquipedEquipment();
            statManagerUI.UpdateUI();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item.item != null) return;

            if (upgradeInstance != null)
            {
                return;
            }

            upgradeInstance = Instantiate(upgradeObject, canvas.transform);
            EquipmentUpgrade equipmentUpgrade = upgradeInstance.GetComponent<EquipmentUpgrade>();
            inventoryManagerUI.UpdateUpgradeInstance(upgradeInstance);
            if (equipmentUpgrade != null)
            {
                equipmentUpgrade.Initialize(equipment.equipment.RunTimeEquipmentData, player, statManagerUI, inventoryManagerUI);
            }
        }
    }
    #endregion

    #region Displays
    /// <summary>
    /// Call this method to display the item tooltip
    /// </summary>
    private void DisplayItemTooltip()
    {
        canvas = GetComponentInParent<Canvas>();

        // Instantiate the Tooltip
        tooltipInstance = Instantiate(itemTooltipObject, canvas.transform);
        tooltipInstance.transform.position = Input.mousePosition;

        panel = tooltipInstance.GetComponent<RectTransform>();

        // Position it initially
        UpdateTooltipPosition();
        isHovering = true;

        // Get all transforms for the toolTip information
        Transform itemSpriteTransform = tooltipInstance.transform.Find("ItemDetailPanel/ItemSprite");
        Transform nameTransform = tooltipInstance.transform.Find("ItemDetailPanel/DetailsGroup/tb_itemName");
        Transform descriptionTransform = tooltipInstance.transform.Find("ItemDetailPanel/DetailsGroup/DetailsSubGroup/tb_itemDescription");
        Transform rarityTransform = tooltipInstance.transform.Find("ItemDetailPanel/DetailsGroup/DetailsSubGroup/RaritySellGroup/tb_itemRarity");
        Transform sellValueTransform = tooltipInstance.transform.Find("ItemDetailPanel/DetailsGroup/DetailsSubGroup/RaritySellGroup/tb_itemSellValue");

        // Get all Components
        Image itemSprite = itemSpriteTransform.GetComponent<Image>();
        TMP_Text tb_itemName = nameTransform.GetComponent<TMP_Text>();
        TMP_Text tb_itemDescription = descriptionTransform.GetComponent<TMP_Text>();
        TMP_Text tb_itemRarity = rarityTransform.GetComponent<TMP_Text>();
        TMP_Text tb_itemSellValue = sellValueTransform.GetComponent<TMP_Text>();

        // Check if all the Components are not null
        if (itemSprite == null ||
            tb_itemName == null ||
            tb_itemDescription == null ||
            tb_itemRarity == null ||
            tb_itemSellValue == null)
        {
            Debug.LogWarning($"Item prefab for {item.item.RunTimeItemData.ItemName} has a null component!");
            return;
        }

        // Set the values
        itemSprite.sprite = item.item.RunTimeItemData.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_itemName.text = item.item.RunTimeItemData.ItemName;
        tb_itemDescription.text = item.item.RunTimeItemData.ItemDescription;
        tb_itemRarity.text = item.item.RunTimeItemData.ItemRarity.ToString();
        tb_itemSellValue.text = "Sell value: " + item.item.RunTimeItemData.ItemSellValue.ToString();
    }

    /// <summary>
    /// Call this method to display the equipment tooltip
    /// </summary>
    private void DisplayEquipmentTooltip()
    {
        #region Get Components
        EquipmentData ed = equipment.equipment.RunTimeEquipmentData;
        canvas = GetComponentInParent<Canvas>();

        // Instantiate the tooltip
        tooltipInstance = Instantiate(equipmentTooltipObject, canvas.transform);
        tooltipInstance.transform.position = Input.mousePosition;

        panel = tooltipInstance.GetComponent<RectTransform>();

        // Position it initially
        UpdateTooltipPosition();
        isHovering = true;

        // Get all the transforms
        Transform equipSpriteTransform = tooltipInstance.transform.Find("C/SpriteDescriptionC/img_sprite");
        Transform equipTypeTransform = tooltipInstance.transform.Find("C/SpriteDescriptionC/tb_equipmentType");
        Transform equipDescriptionTransform = tooltipInstance.transform.Find("C/SpriteDescriptionC/tb_description");
        Transform equipNameTransform = tooltipInstance.transform.Find("C/NameC/tb_name");
        Transform equipRarityTransform = tooltipInstance.transform.Find("C/NameC/tb_rarity");
        Transform equipStatsTransform = tooltipInstance.transform.Find("C/NameC/tb_stats");
        Transform equipSellValueTransform = tooltipInstance.transform.Find("C/NameC/tb_sellValue");

        // Get all the components
        Image img_sprite = equipSpriteTransform.GetComponent<Image>();
        TMP_Text tb_equipmentType = equipTypeTransform.GetComponent<TMP_Text>();
        TMP_Text tb_equipDescription = equipDescriptionTransform.GetComponent<TMP_Text>();
        TMP_Text tb_equipName = equipNameTransform.GetComponent<TMP_Text>();
        TMP_Text tb_equipRarity = equipRarityTransform.GetComponent<TMP_Text>();
        TMP_Text tb_equipStats = equipStatsTransform.GetComponent<TMP_Text>();
        TMP_Text tb_equipSellValue = equipSellValueTransform.GetComponent<TMP_Text>();

        // Just checking if all the Components are not null
        if (img_sprite == null ||
            tb_equipmentType == null ||
            tb_equipDescription == null ||
            tb_equipName == null ||
            tb_equipRarity == null ||
            tb_equipStats == null ||
            tb_equipSellValue == null)
        {
            Debug.LogWarning($"Item prefab for {ed.ItemName} has a null component!");
            return;
        }
        #endregion

        #region Set values
        // Set the values
        img_sprite.sprite = ed.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_equipmentType.text = ed.ItemType.ToString();
        tb_equipDescription.text = ed.ItemDescription;
        tb_equipName.text = ed.ItemName;
        tb_equipRarity.text = ed.ItemRarity.ToString();

        if (ed.IsItemSellable)
            tb_equipSellValue.text = ed.ItemSellValue.ToString();
        else tb_equipSellValue.text = "Not sellable";
        #endregion

        #region Stats string
        // Create the string to show stats
        StringBuilder statString = new StringBuilder();
        if (ed.ItemHpBonus != 0)            statString.AppendLine(AppendStat(ed.ItemHpBonus, "max HP"));
        if (ed.ItemAttackBonus != 0)        statString.AppendLine(AppendStat(ed.ItemAttackBonus, "Attack"));
        if (ed.ItemAttackSpeedBonus != 0)   statString.AppendLine(AppendStat(ed.ItemAttackSpeedBonus, "Attack speed"));
        if (ed.ItemDefenseBonus != 0)       statString.AppendLine(AppendStat(ed.ItemDefenseBonus, "Defense"));
        if (ed.ItemManaBonus != 0)          statString.AppendLine(AppendStat(ed.ItemManaBonus, "Mana"));
        if (ed.ItemMovementSpeedBonus != 0) statString.AppendLine(AppendStat(ed.ItemMovementSpeedBonus, "MovementSpeed"));

        if (ed.ItemResistanceBonus.Count > 0)
        {
            statString.AppendLine("<b>Ressistances</b><size=50%>\n</size>");
            foreach (Resistance res in ed.ItemResistanceBonus)
            {
                statString.AppendLine($"\t{AppendStat(res.Amount, res.SpellAfinity.ToString() + " Resistance")}");
            }
        }

        if (ed.ItemDamageAffinity.Count > 0)
        {
            statString.AppendLine("\n<b>Damage Affinities</b>");
            foreach (Resistance res in ed.ItemDamageAffinity)
            {
                statString.AppendLine($"\t{AppendStat(res.Amount, res.SpellAfinity.ToString() + " Damage Affinity")}");
            }
        }

        Debug.Log(statString.ToString());
        tb_equipStats.text = statString.ToString();
        #endregion
    }

    /// <summary>
    /// Call this method to return the text value of the stats
    /// </summary>
    /// <param name="value">Amount</param>
    /// <param name="name">Stat</param>
    /// <returns></returns>
    private string AppendStat(float value, string name)
    {
        return $"{(value > 0 ? "+ " : "- ")}{Mathf.Abs(value)} {name}";
    }

    private void DestroyTooltip()
    {
        // Turn the flag false
        isHovering = false;

        // Destroy instance
        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance);
            tooltipInstance = null;
        }
    }

    #region Tooltip position
    private void UpdateTooltipPosition()
    {
        // Read mouse position in screen pixels
        Vector2 mouse = Input.mousePosition;

        // Measure tooltip size in screen pixels
        Vector2 tooltipSize = panel.rect.size * canvas.scaleFactor;

        float x = mouse.x;
        float y = mouse.y;

        // Clamp horizontally so the right edge stays inside the screen
        if (x + tooltipSize.x > Screen.width)
            x = Screen.width - tooltipSize.x;

        if (x < 0)
            x = 0;

        // Clamp vertically so the tooltip never leaves the top/bottom
        if (y + tooltipSize.y * 0.5f > Screen.height)
            y = Screen.height - tooltipSize.y * 0.5f;

        if (y - tooltipSize.y * 0.5f < 0)
            y = tooltipSize.y * 0.5f;

        // Apply the final position (ScreenSpace Overlay → world camera is null)
        Vector2 clamped = new Vector2(x, y);
        panel.position = clamped;
    }
    #endregion
    #endregion
}