using UnityEngine;

public class InventoryManagerUI : MonoBehaviour
{
    public void ClickedElement(GameObject gameObject)
    {
        Debug.Log($"Clicked slot {gameObject.name}");
    }
}
