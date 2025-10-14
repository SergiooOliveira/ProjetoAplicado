using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public void ClickedElement(GameObject gameObject)
    {
        Debug.Log($"Clicked slot {gameObject.name}");
    }

}
