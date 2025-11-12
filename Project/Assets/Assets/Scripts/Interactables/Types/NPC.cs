using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{    
    public NPCData npcData;
    private NPCData runtimeData;
   
    public NPCData RuntimeData => runtimeData;

    private GameObject shopPanel;
    private PlayerData playerData;

    public PlayerData PlayerData => playerData;

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

        // Get ItemPanel
        runtimeData.SetPanel(this.transform.Find("Canvas/ListPanel").gameObject);
    }

    public void Interact(PlayerData playerData)
    {
        bool newState = !runtimeData.NPCItemPanel.activeSelf;
        runtimeData.NPCItemPanel.SetActive(newState);

        this.playerData = playerData;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GameManager.Instance.playerTag)
        {
            if (runtimeData.NPCItemPanel.activeSelf)
            {
                runtimeData.NPCItemPanel.SetActive(false);
                Debug.Log("Closed panel on trigger exit");
            }
        }
    }
}