using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string iOName;
    [SerializeField] private Sprite iOIcon;
    [SerializeField] private InteractableType iOInteractableType;
    [SerializeField] private string[] iODialogue;
}
