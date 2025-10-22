using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private EquipmentData equipmentData;
    private EquipmentData runTimeEquipmentData;

    public void Initialize()
    {
        runTimeEquipmentData = equipmentData;
    }

    public EquipmentData RunTimeEquipmentData => runTimeEquipmentData;
}