using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    private ItemData runTimeItemData;

    public void Initialize()
    {
        Debug.Log($"Initializing {runTimeItemData.ItemName}");
        runTimeItemData = itemData;
    }

    public ItemData RunTimeItemData => runTimeItemData;
}