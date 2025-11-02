using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text tb_Name;
    [SerializeField] private TMP_Text tb_Amount;
    [SerializeField] private GameObject tooltipObject;
    private GameObject tooltipInstance;

    private InventoryItem entry;
    
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
        // Instantiate the Tooltip
        tooltipInstance = Instantiate(tooltipObject, GetComponentInParent<Canvas>().transform);
        tooltipInstance.transform.position = Input.mousePosition;
        
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
        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance);
            tooltipInstance = null;
        }
    }

    public void FixedUpdate()
    {
        if (tooltipInstance != null)
            tooltipInstance.transform.position = Input.mousePosition;
    }
}