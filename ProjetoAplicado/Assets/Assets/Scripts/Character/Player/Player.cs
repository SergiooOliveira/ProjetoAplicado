using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : Character
{
    public static Player Instance;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    
    private float horizontal;
    
    private float jumpingPower = 7f;
    private bool isFacingRight = true;

    public GameObject fireball;

    #region Unity Methods
    public void Awake ()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        this.MovementSpeed = 4f;
    }

    private void FixedUpdate ()
    {
        rb.linearVelocity = new Vector2(horizontal * MovementSpeed, rb.linearVelocity.y);

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
    /// <summary>
    /// This event is called when the player moves
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnMove (InputAction.CallbackContext callbackContext)
    {
        //Debug.Log($"Carreguei no {callbackContext.ReadValue<Vector2>()}");
        horizontal = callbackContext.ReadValue<Vector2>().x;
    }

    /// <summary>
    /// This event is called when the player jumps
    /// </summary>
    /// <param name="callbackContext"></param>
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

    /// <summary>
    /// This event is called when the player attacks
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnAttack (InputAction.CallbackContext callbackContext)
    {
        // Each spell has different interactions
        // Check what is the selected spell first
        // At the moment we only have fireball
        Debug.Log("Attacking");

        if (callbackContext.performed)
        {           
            Instantiate(fireball, new Vector2(rb.position.x, rb.position.y), Quaternion.identity);
        }
    }
    #endregion

}
