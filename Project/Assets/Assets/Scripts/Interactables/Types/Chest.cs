using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private string chestName;
    public List<ChestData> chestData;
    public List<Spell> spellReward;
    private bool isOpened = false;

    public string ChestName => chestName;

    private void Awake()
    {
        foreach (ChestData data in chestData)
        {
            if (data.DropObject != null)
                data.Initialize();
        }

    }

    /// <summary>
    /// Call this method to Interact with a chest
    /// </summary>  
    public void Interact(PlayerData playerData)
    {
        if (isOpened) return;
        if (playerData == null)
        {
            Debug.Log("Player data is null");
            return;
        }

        foreach (ChestData data in chestData)
        {
            // Chest will give an item
            if (data != null)
            {
                if (data.ItemDrop.item != null)
                {
                    // It's an item
                    playerData.AddItem(data.ItemDrop, data.ItemDrop.quantity);
                    Debug.Log($"{chestName} gave item: {data.ItemDrop.item.RunTimeItemData.ItemName}");
                }

                if (data.EquipmentDrop.equipment != null)
                {
                    // It's an equipment
                    playerData.AddEquip(data.EquipmentDrop);
                    Debug.Log($"{chestName} gave item: {data.EquipmentDrop.equipment.RunTimeEquipmentData.ItemName}");
                }
            }
        }

        foreach (Spell spell in spellReward)
        {
            // Chest will give a spell
            if (spell != null)
            {
                playerData.AddSpell(spell);
                Debug.Log($"Added spell {spell.SpellName} to player!");
            }
        }

        isOpened = true;
        Destroy(gameObject);
    }
}