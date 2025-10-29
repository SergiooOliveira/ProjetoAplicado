using TMPro;
using UnityEngine;

public class InventoryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform itemListPanel;
    [SerializeField] private GameObject slot;

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnEnable()
    {
        DeleteList();
        SetAllSlots();
    }

    public void SetAllSlots()
    {
        foreach (Item item in player.RunTimePlayerData.CharacterInventory)
        {
            GameObject newSLot = Instantiate(slot, itemListPanel);

            Transform name = newSLot.transform.Find("Name");
            Transform quantity = newSLot.transform.Find("Quantity");

            TMP_Text tb_name = name.GetComponent<TMP_Text>();
            TMP_Text tb_quantity = quantity.GetComponent<TMP_Text>();

            tb_name.text = item.RunTimeItemData.ItemName;
            tb_quantity.text = item.ItemQuantity.ToString();
        }
    }

    /// <summary>
    /// Call this method to delete all the items in the inventory
    /// </summary>
    public void DeleteList()
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
