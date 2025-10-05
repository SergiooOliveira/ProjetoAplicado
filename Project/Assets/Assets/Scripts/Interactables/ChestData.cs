using UnityEngine;

[CreateAssetMenu(menuName = "Interactables/Chest")]
public class ChestData : ScriptableObject
{
    public string chestName;
    public Item fixedDrop;

    /// <summary>
    /// Call this method to get the info of an item
    /// </summary>
    /// <returns>Returns the name and the type of the item</returns>
    public override string ToString()
    {
        string info = $"Name: {fixedDrop.name}\nItem Type: {fixedDrop.ItemType}";

        return info;
    }
}