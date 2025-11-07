using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // TODO: When closing inventory it needs to close the tooltip as well, otherwise it stays there

    [SerializeField] private TMP_Text tb_Name;
    [SerializeField] private TMP_Text tb_Amount;
    [SerializeField] private GameObject tooltipObject;
    private GameObject tooltipInstance;

    private Canvas canvas;
    private RectTransform panel;
    private bool isHovering;

    private InventoryItem entry;

    private void Update()
    {
        if (isHovering && tooltipInstance != null)
            UpdateTooltipPosition();
    }

    /// <summary>
    /// Call this method to set the text values in each slot
    /// </summary>
    /// <param name="i"></param>
    public void SetSlot(InventoryItem e)
    {
        entry = e;

        if (entry.item == null) return;

        tb_Name.text = entry.item.RunTimeItemData.ItemName;
        tb_Amount.text = entry.quantity.ToString();
    }

    /// <summary>
    /// This method is called when the mouse is hover an element
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (entry.item == null) return;
        
        canvas = GetComponentInParent<Canvas>();

        // Instantiate the Tooltip
        tooltipInstance = Instantiate(tooltipObject, canvas.transform);
        tooltipInstance.transform.position = Input.mousePosition;

        panel = tooltipInstance.GetComponent<RectTransform>();

        // Position it initially
        UpdateTooltipPosition();
        isHovering = true;

        // Get all transforms for the toolTip information
        Transform itemSpriteTransform =     tooltipInstance.transform.Find("ItemDetailPanel/ItemSprite");
        Transform nameTransform =           tooltipInstance.transform.Find("ItemDetailPanel/DetailsGroup/tb_itemName");
        Transform descriptionTransform =    tooltipInstance.transform.Find("ItemDetailPanel/DetailsGroup/DetailsSubGroup/tb_itemDescription");
        Transform rarityTransform =         tooltipInstance.transform.Find("ItemDetailPanel/DetailsGroup/DetailsSubGroup/RaritySellGroup/tb_itemRarity");
        Transform sellValueTransform =      tooltipInstance.transform.Find("ItemDetailPanel/DetailsGroup/DetailsSubGroup/RaritySellGroup/tb_itemSellValue");

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
            Debug.LogWarning($"Item prefab for {entry.item.RunTimeItemData.ItemName} has no SpriteRenderer!");
            return;
            
            //Debug.Log($"Item Sprite: {prefabRenderer.sprite.name}");
        }

        // Set the values
        itemSprite.sprite = entry.item.RunTimeItemData.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_itemName.text = entry.item.RunTimeItemData.ItemName;
        tb_itemDescription.text = entry.item.RunTimeItemData.ItemDescription;
        tb_itemRarity.text = entry.item.RunTimeItemData.ItemRarity.ToString();
        tb_itemSellValue.text = "Sell value: " + entry.item.RunTimeItemData.ItemSellValue.ToString();

        tooltipInstance.SetActive(true);
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