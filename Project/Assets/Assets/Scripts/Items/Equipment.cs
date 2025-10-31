using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public static readonly Dictionary<ItemRarity, float> rarityDropRates = new()
    {
        { ItemRarity.Common, 0.2f },        // 20%
        { ItemRarity.Rare, 0.1f },          // 10%
        { ItemRarity.Epic, 0.05f },         // 5%
        { ItemRarity.Legendary, 0.01f }     // 1%
    };

    [SerializeField] private EquipmentData equipmentData;
    private EquipmentData runTimeEquipmentData;

    public void Initialize()
    {
        runTimeEquipmentData = ScriptableObject.Instantiate(equipmentData);
    }

    public EquipmentData RunTimeEquipmentData => runTimeEquipmentData;
}