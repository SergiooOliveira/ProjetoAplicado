using TMPro;
using UnityEngine;

public class NPCShopManager : MonoBehaviour
{
    [SerializeField] private Transform chatMenu;
    [SerializeField] private Transform buyMenu;
    [SerializeField] private Transform sellMenu;
    [SerializeField] private TMP_Text tb_playerGold;

    public void OpenChatMenu()
    {
        Debug.Log("Opening chat menu");
        chatMenu.gameObject.SetActive(true);
        buyMenu.gameObject.SetActive(false);
        sellMenu.gameObject.SetActive(false);
    }
    public void OpenBuyMenu()
    {
        chatMenu.gameObject.SetActive(false);
        Debug.Log("Opening buy menu");
        buyMenu.gameObject.SetActive(true);
        sellMenu.gameObject.SetActive(false);
    }

    public void OpenSellMenu()
    {
        chatMenu.gameObject.SetActive(false);
        //Debug.Log("Opening sell menu");
        buyMenu.gameObject.SetActive(false);
        sellMenu.gameObject.SetActive(true);
    }

    public void UpdateGoldUI(PlayerData playerData)
    {
        tb_playerGold.text = "Gold: " + playerData.CharacterGold.ToString();
    }
}