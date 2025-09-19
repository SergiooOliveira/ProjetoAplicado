using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private float movementX = 0f;

    public void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            //Debug.Log($"Carreguei no {callbackContext.ReadValue<Vector2>()}");
            movementX = callbackContext.ReadValue<Vector2>().x;
        }
    }
}
