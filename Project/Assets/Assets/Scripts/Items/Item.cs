using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static readonly Dictionary<ItemRarity, float> rarityDropRates = new()
    {
        { ItemRarity.Common, 0.60f },
        { ItemRarity.Rare, 0.40f },
        { ItemRarity.Epic, 0.25f },
        { ItemRarity.Legendary, 0.1f }
    };


    [SerializeField] private ItemData itemData;
    private ItemData runTimeItemData;

    public void Initialize()
    {        
        runTimeItemData = ScriptableObject.Instantiate(itemData);
        // Debug.Log($"Initializing {runTimeItemData.ItemName}");
    }

    public ItemData RunTimeItemData => runTimeItemData;
}