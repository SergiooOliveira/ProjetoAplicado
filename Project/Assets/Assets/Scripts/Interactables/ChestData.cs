using UnityEngine;

[CreateAssetMenu(menuName = "Interactables/Chest")]
public class ChestData : ScriptableObject
{
    public string chestName;
    public GameObject dropObject;
    private ItemData itemDrop;

    public void Initialize()
    {
        Item item = dropObject.GetComponent<Item>();

        item.Initialize();
        itemDrop = item.RunTimeItemData;
    }

    /// <summary>
    /// Call this method to get the info of an item
    /// </summary>
    /// <returns>Returns the name and the type of the item</returns>
    public override string ToString()
    {
        string info = $"Name: {itemDrop.ItemName}\nItem Type: {itemDrop.ItemType}";

        return info;
    }
}