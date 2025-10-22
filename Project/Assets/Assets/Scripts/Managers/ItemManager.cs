using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public List<Item> items {  get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        items = new List<Item>(Resources.LoadAll<Item>("Items"));
        //LoadedItems();
    }

    private void LoadedItems()
    {
        foreach (Item item in items)
        {
            Debug.Log($"{item.RunTimeItemData.ItemName} Loaded");
        }
    }

    /// <summary>
    /// Call this method to open a chest and give an item to the Player
    /// </summary>
    public void RollItem()
    {        
        int index = Random.Range(0, items.Count);

        Debug.Log($"Gave Player the item {items[index]}");
    }
}