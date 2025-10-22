using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    private ItemData runTimeItemData;

    public void Initialize()
    {        
        runTimeItemData = ScriptableObject.Instantiate(itemData);
        // Debug.Log($"Initializing {runTimeItemData.ItemName}");
    }

    public ItemData RunTimeItemData => runTimeItemData;
}