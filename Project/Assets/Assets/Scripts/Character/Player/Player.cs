using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : Character
{
    public static Player Instance;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool canPlayerInteract = false;
    private Chest interactedChest = null;
    
    private float horizontal;
    private float jumpingPower = 8f;
    private bool isFacingRight = true;

    #region Unity Methods
    public void Awake ()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void FixedUpdate ()
    {
        rb.linearVelocity = new Vector2(horizontal * MovementSpeed, rb.linearVelocity.y);

        if (!isFacingRight && horizontal > 0f) Flip();
        else if (isFacingRight && horizontal < 0f) Flip();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameManager.Instance.grimoireTag)
        {
            //Debug.Log("Triggered with " + collision.name);
            Player.Instance.AddSpell(SpellManager.Instance.GetSpell(collision.name));
            Destroy(collision.gameObject);
        }

        // Cares about all the interactables
        if (collision.tag == GameManager.Instance.interactableTag)
        {
            canPlayerInteract = true;
            if (collision.TryGetComponent<Chest>(out Chest chest))
                interactedChest = chest;
            // Can Add more collisions components as needed
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Turns the flag false so the player can't interact
        if (collision.tag == GameManager.Instance.interactableTag)
        {
            canPlayerInteract = false;
        }
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
        //if (callbackContext.performed) Debug.Log($"Space Pressed");
        //if (IsGrounded()) Debug.Log($"Space Pressed");

        if (callbackContext.performed && IsGrounded())
        {
            //Debug.Log("Jumping");
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

        if (callbackContext.performed)
        {
            //Instantiate(fireball, new Vector2(rb.position.x, rb.position.y), Quaternion.identity);            
            //SpellManager.Instance.ShowEquippedSpells();
            if (SpellManager.Instance.selectedSpell == null) return;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 castDirection = (mousePos - Player.Instance.rb.position).normalized;

            SpellManager.Instance.selectedSpell.Cast(Player.Instance.rb.position, castDirection);
        }
    }

    /// <summary>
    /// This event is called when the player interacts
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnInteract (InputAction.CallbackContext callbackContext)
    {
        // TODO: .started helped but I should be able to do even better
        if (callbackContext.started && canPlayerInteract)
            interactedChest.Interact();
    }
    #endregion
}
