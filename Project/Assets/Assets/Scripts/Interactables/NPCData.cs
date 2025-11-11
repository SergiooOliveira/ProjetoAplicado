using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Interactables/NPC")]
public class NPCData : ScriptableObject
{
    #region Serialized Fields
    [Header("Identity")]
    [SerializeField] private string npcName;
    [SerializeField] private GameObject npcPrefab;

    [Header("Items")]
    [SerializeField] private List<ItemEntry> npcInventory;
    [SerializeField] private List<EquipmentEntry> npcEquipment;

    [Header("UI")]
    [SerializeField] private GameObject npcItemPanel;
    #endregion

    #region Property implementation
    // *----- Identity -----*
    public string NPCName => npcName;
    public GameObject NPCPrefab => npcPrefab;

    // *----- Inventory -----*
    public List<ItemEntry> NPCInventory => npcInventory;
    public List<EquipmentEntry> NPCEquipment => npcEquipment;

    // *----- UI -----*
    public GameObject NPCItemPanel => npcItemPanel;
    #endregion
}