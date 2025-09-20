using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    
    
    private float horizontal;
    private float movementSpeed = 4f;
    private float jumpingPower = 7f;
    private bool isFacingRight = true;

    #region Unity Methods
    public void Awake ()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void FixedUpdate ()
    {
        rb.linearVelocity = new Vector2(horizontal * movementSpeed, rb.linearVelocity.y);

        if (!isFacingRight && horizontal > 0f) Flip();
        else if (isFacingRight && horizontal < 0f) Flip();
    }
    #endregion

    #region Player Checks
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    #endregion

    #region Unity Events
    public void OnMove (InputAction.CallbackContext callbackContext)
    {
        //Debug.Log($"Carreguei no {callbackContext.ReadValue<Vector2>()}");
        horizontal = callbackContext.ReadValue<Vector2>().x;
    }

    public void OnJump (InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed) Debug.Log($"Space Pressed");
        if (IsGrounded()) Debug.Log($"Space Pressed");

        if (callbackContext.performed && IsGrounded())
        {
            Debug.Log("Jumping");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }
    }
    #endregion

}
