using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Item")]
public class Item : ScriptableObject, IItem
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private GameObject itemPrefeb;
    [SerializeField] private ItemType itemType;
    [SerializeField] private bool isItemSellable;
    [SerializeField] private int itemSellValue;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public GameObject ItemPrefab => itemPrefeb;
    public ItemType ItemType => itemType;
    public bool IsItemSellable => isItemSellable;
    public int ItemSellValue => itemSellValue;
}