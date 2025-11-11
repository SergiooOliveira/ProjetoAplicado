using UnityEngine;

public class NPC : MonoBehaviour
{    
    public NPCData npcData;
    private NPCData runtimeData;
    
    private bool isInventoryOpen = false;

    public NPCData RuntimeData => runtimeData;

    private void Awake()
    {
        runtimeData = Instantiate(npcData);
        Initialize();
    }


    private void Initialize()
    {
        foreach (ItemEntry entry in runtimeData.NPCInventory)
        {
            if (entry.item != null)
                entry.item.Initialize();
        }

        foreach (EquipmentEntry entry in runtimeData.NPCEquipment)
        {
            if (entry.equipment != null)
                entry.equipment.Initialize();
        }
    }

    public void Interact(PlayerData playerData)
    {        
        if (runtimeData == null) Debug.LogWarning("NPC data is null");
        if (playerData == null) Debug.LogWarning("Player data is null");

        isInventoryOpen = runtimeData.NPCItemPanel.activeSelf;

        runtimeData.NPCItemPanel.SetActive(!isInventoryOpen);
    }
}