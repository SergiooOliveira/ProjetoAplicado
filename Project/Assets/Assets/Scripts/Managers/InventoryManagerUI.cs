using UnityEngine;

public class InventoryManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject slot;

    private PlayerData player;
    
    /// <summary>
    /// Call this method to bind an inventory to a player
    /// </summary>
    /// <param name="player"></param>
    public void Bind(PlayerData player)
    {
        this.player = player;
    }

    public void setAllSlots()
    {
        foreach (Item item in player.CharacterInventory)
        {
            GameObject tempSlot = Instantiate(slot);
            
        }
    }

    public void ClickedElement(GameObject gameObject)
    {
        Debug.Log($"Clicked slot {gameObject.name}");
    }
}
