using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    private Player player;
    private PlayerData playerData;

    private Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Animator animator;

    private bool canPlayerInteract = false;
    private Chest interactedChest = null;

    private float horizontal;
    private float jumpingPower = 8f;
    private bool isFacingRight = true;

    #region Unity Methods

    public override void OnStartClient()
    {
        //Debug.Log("test owner");
        if (IsOwner)
        {
            //Debug.Log("owner true");
            GetComponent<PlayerInput>().enabled = true;
        }
            
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        playerData = player.RunTimePlayerData;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * playerData.CharacterMovementSpeed, rb.linearVelocity.y);

        if (!isFacingRight && horizontal > 0f) Flip();
        else if (isFacingRight && horizontal < 0f) Flip();

        if (!IsOwner)
            return;
        // Collect input every frame
        horizontal = Input.GetAxisRaw("Horizontal");

        // Sets the parameter value in the Animator
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameManager.Instance.grimoireTag)
        {
            //Debug.Log("Triggered with " + collision.name);
            playerData.AddSpell(SpellManager.Instance.GetSpell(collision.name));
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
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        //Debug.Log($"Carreguei no {callbackContext.ReadValue<Vector2>()}");
        horizontal = callbackContext.ReadValue<Vector2>().x;
    }

    /// <summary>
    /// This event is called when the player jumps
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        //if (callbackContext.performed) Debug.Log($"Space Pressed");
        //if (IsGrounded()) Debug.Log($"Space Pressed");

        Debug.Log("jump press");
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
    public void OnAttack(InputAction.CallbackContext callbackContext)
    {
        // Each spell has different interactions
        // Check what is the selected spell first
        // At the moment we only have fireball       

        // Debug.Log("Attacking");

        if (callbackContext.performed)
        {
            //Instantiate(fireball, new Vector2(rb.position.x, rb.position.y), Quaternion.identity);            
            //SpellManager.Instance.ShowEquippedSpells();
            if (SpellManager.Instance.selectedSpell == null) return;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 castDirection = (mousePos - rb.position).normalized;

            SpellManager.Instance.selectedSpell.Cast(rb.position, castDirection, player);
        }
    }

    /// <summary>
    /// This event is called when the player interacts
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        // TODO: .started helped but I should be able to do even better
        if (callbackContext.started && canPlayerInteract)
            interactedChest.Interact(playerData);
    }

    /// <summary>
    /// Call this method to select the next spell in the spell book
    /// </summary>
    /// <param name="callbackContext"></param>
    public void NextSpell(InputAction.CallbackContext callbackContext)
    {
        for (int i = 0; i < playerData.CharacterEquipedSpells.Count; i++)
        {
            if (playerData.CharacterEquipedSpells[i].IsSpellSelected)
            {
                playerData.CharacterEquipedSpells[i].Deselect();

                int nextIndex = (i + 1) % playerData.CharacterEquipedSpells.Count;
                playerData.CharacterEquipedSpells[nextIndex].Select();

                break;
            }
        }
    }
    #endregion
}