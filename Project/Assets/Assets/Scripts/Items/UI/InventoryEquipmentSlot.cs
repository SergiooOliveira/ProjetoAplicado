using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryEquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text tb_Name;
    [SerializeField] private TMP_Text tb_Amount;
    [SerializeField] private GameObject tooltipObject;
    private GameObject tooltipInstance;

    private Canvas canvas;
    private RectTransform panel;
    private bool isHovering;

    private EquipmentEntry entry;

    private void Update()
    {
        if (isHovering && tooltipInstance != null)
            UpdateTooltipPosition();
    }

    public void SetSlot(EquipmentEntry e)
    {
        entry = e;

        if (entry.equipment == null) return;

        tb_Name.text = entry.equipment.RunTimeEquipmentData.ItemName;
        tb_Amount.text = entry.quantity.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (entry.equipment == null) return;
        EquipmentData ed = entry.equipment.RunTimeEquipmentData;

        canvas = GetComponentInParent<Canvas>();

        // Instantiate the tooltip
        tooltipInstance = Instantiate(tooltipObject, canvas.transform);
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
            Debug.LogWarning($"Error getting an equipment tooltip component");
            return;
        }

        // Set the values
        img_sprite.sprite = ed.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_equipmentType.text = ed.ItemType.ToString();
        tb_equipDescription.text = ed.ItemDescription;
        tb_equipName.text = ed.ItemName;
        tb_equipRarity.text = ed.ItemRarity.ToString();
        tb_equipSellValue.text = ed.ItemSellValue.ToString();

        // Create the string to show stats
        string statString = "";
        

        statString += AppendStat(ed.ItemHpBonus, "max HP");
        statString += AppendStat(ed.ItemAttackBonus, "Attack");
        statString += AppendStat(ed.ItemAttackSpeedBonus, "Attack speed");
        statString += AppendStat(ed.ItemDefenseBonus, "Defense");
        statString += AppendStat(ed.ItemManaBonus, "Mana");
        statString += AppendStat(ed.ItemMovementSpeedBonus, "MovementSpeed");

        tb_equipStats.text = statString;

        Debug.Log($"{tb_equipName.text}");
    }

    private string AppendStat(float value, string name)
    {
        string stat = "";
        if (value != 0)
            stat += $"{(value > 0 ? "+ ": "- ")}{Mathf.Abs(value)} {name}\n";

        return stat;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance);
            tooltipInstance = null;
        }
    }

    private void UpdateTooltipPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 tooltipSize = panel.sizeDelta * canvas.scaleFactor;

        float screenHeight = Screen.height;

        // Start tooltip right next to the mouse (a few pixels offset to avoid overlap)
        Vector2 targetPos = mousePos + new Vector2(8f, -8f);

        // Clamp vertically if tooltip would go off-screen
        if (targetPos.y + tooltipSize.y > screenHeight)
            targetPos.y = screenHeight - tooltipSize.y - 5f;
        else if (targetPos.y < 0)
            targetPos.y = 5f;

        panel.position = targetPos;
    }
}
