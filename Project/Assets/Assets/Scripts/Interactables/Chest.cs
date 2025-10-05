using UnityEngine;

public class Chest : MonoBehaviour
{
    public ChestData chestData;
    private bool isOpened = false;

    /// <summary>
    /// Call this method to Interact with a chest
    /// </summary>
    public void Interact()
    {
        // Only allow to open once
        if (isOpened) return;

        Debug.Log($"Giving Player {chestData.ToString()}");
    }
}