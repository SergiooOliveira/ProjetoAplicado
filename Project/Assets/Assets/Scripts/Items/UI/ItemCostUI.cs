using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCostUI : MonoBehaviour
{
    [SerializeField] private Image img_icon;
    [SerializeField] private TMP_Text tb_cost;

    // Used only for gold
    public void Initialize(float amount)
    {
        img_icon.sprite = Resources.Load<Sprite>("Sprites/Items/GoldIcon");
        tb_cost.text = amount.ToString();
    }

    public void Initialize(ItemCost itemCost)
    {
        img_icon.sprite = itemCost.item.RunTimeItemData.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_cost.text = itemCost.quantity.ToString();
    }
}
