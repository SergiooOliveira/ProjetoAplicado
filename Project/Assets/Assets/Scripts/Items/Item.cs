using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static readonly Dictionary<ItemRarity, float> rarityDropRates = new()
    {
        { ItemRarity.Common, 0.60f },       // 60%
        { ItemRarity.Rare, 0.40f },         // 40%
        { ItemRarity.Epic, 0.25f },         // 25%
        { ItemRarity.Legendary, 0.1f }      // 10%
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