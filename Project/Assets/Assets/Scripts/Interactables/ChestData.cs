using UnityEngine;

[CreateAssetMenu(menuName = "Interactables/ChestData")]
public class ChestData : ScriptableObject
{
    [SerializeField] private GameObject dropObject;
    [SerializeField] private int amount;
    private ItemEntry itemDrop;
    private EquipmentEntry equipmentDrop;

    public GameObject DropObject => dropObject;
    public ItemEntry ItemDrop => itemDrop;
    public EquipmentEntry EquipmentDrop => equipmentDrop;

    public void Initialize()
    {
        dropObject.TryGetComponent<Item>(out Item item);
        dropObject.TryGetComponent<Equipment>(out Equipment equipment);
        
        if (item != null)
        {
            item.Initialize();
            
            itemDrop = new ItemEntry
            {
                item = item,
                quantity = amount,
                isGuarantee = true
            };            
        }

        if (equipment != null)
        {
            equipment.Initialize();

            equipmentDrop = new EquipmentEntry
            {
                equipment = equipment,
                quantity = amount,
                isEquipped = false
            };
        }
    }
}