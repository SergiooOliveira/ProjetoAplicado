using UnityEngine;

public class NPCItemManager : MonoBehaviour
{
    [SerializeField] private Transform itemList;
    [SerializeField] private GameObject itemSlot;

    private NPCData npc;

    private void Awake()
    {
        npc = GetComponentInParent<NPC>().npcData;
    }

    public void OnEnable()
    {
        Debug.Log("OnEnable NPCItemManager");
        DeleteList();
        SetAllSlots();
    }

    private void SetAllSlots()
    {
        if (npc == null)
        {
            Debug.Log($"NPC {this.name} is null");
            return;
        }

        foreach (ItemEntry entry in npc.NPCInventory)
        {
            GameObject newSlot = Instantiate(itemSlot, itemList);

            NPCitemSlot slotUI = newSlot.GetComponent<NPCitemSlot>();



            slotUI.SetItem(entry);
        }
    }

    private void DeleteList()
    {
        foreach (Transform i in itemList)
        {
            Destroy(i.gameObject);
        }
    }
}
