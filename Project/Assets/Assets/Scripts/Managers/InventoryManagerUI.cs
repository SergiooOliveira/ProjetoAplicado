using TMPro;
using UnityEngine;

public class InventoryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform itemListPanel;
    [SerializeField] private GameObject slot;

    private PlayerData player;
    
    public void setAllSlots()
    {
        foreach (Item item in player.CharacterInventory)
        {
            GameObject newSLot = Instantiate(slot, itemListPanel);

            Transform name = newSLot.transform.Find("Name");
            Transform quantity = newSLot.transform.Find("Amount");

            TMP_Text tb_name = name.GetComponent<TMP_Text>();
            TMP_Text tb_quantity = quantity.GetComponent<TMP_Text>();

            tb_name.text = item.ItemName;
            tb_quantity.text = item.ItemQuantity.ToString();
        }
    }

    /// <summary>
    /// Call this method to delete all the items in the inventory
    /// </summary>
    public void DeleteInventory()
    {
        foreach (Transform i in itemListPanel)
        {
            Destroy(i.gameObject);
        }
    }

    public void ClickedElement(GameObject gameObject)
    {
        Debug.Log($"Clicked slot {gameObject.name}");
    }
}
