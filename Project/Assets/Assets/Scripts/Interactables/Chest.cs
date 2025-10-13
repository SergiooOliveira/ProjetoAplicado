using UnityEngine;

public class Chest : MonoBehaviour
{
    public ChestData chestData;
    public Spell spellReward;
    //public PlayerData playerData;
    private bool isOpened = false;

    /// <summary>
    /// Call this method to Interact with a chest
    /// </summary>  
    public void Interact(PlayerData playerData)
    {
        if (isOpened) return;

        Debug.Log($"Giving Player {chestData}");

        if (spellReward != null && playerData != null)
        {
            playerData.AddSpell(spellReward);
            Debug.Log($"Added spell {spellReward.name} to player!");
        }
        else
        {
            Debug.LogWarning("Chest is missing spellReward or playerData reference!");
        }

        isOpened = true;
        Destroy(gameObject);
    }
}