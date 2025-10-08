using FishNet.Object;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(PlayerInput))]
public class Player : NetworkBehaviour
{
    public static Player Instance;
    private Character character;
    private PlayerInput playerInput;
    
    // por causa de spellManager
    // ou fica Player.Instance.Character.EquipedSpells ou Player.Instance.GetComponent<Character>().EquipedSpells
    public Character Character => character;  
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private readonly string grimoireTag = "Grimoire";

    private float horizontal;    
    private float jumpingPower = 7f;
    private bool isFacingRight = true;

    #region Unity Methods
    public void Awake()
    {
        character = GetComponent<Character>();
        playerInput = GetComponent<PlayerInput>();

        // if (Instance != null) Destroy(gameObject);
        // else Instance = this;
        if (Instance == null)
            Instance = this;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Only initialize for the local player
        if (IsOwner)
        {
            playerInput.enabled = true;
            character.Initialize("Player", 6f, 0, 0.5f, 0, 0, 1);
            GameManager.Instance.player = this.gameObject;
        }
        else
        {
            playerInput.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        rb.linearVelocity = new Vector2(horizontal * character.MovementSpeed, rb.linearVelocity.y);

        if (!isFacingRight && horizontal > 0f) Flip();
        else if (isFacingRight && horizontal < 0f) Flip();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == grimoireTag)
        {
            Debug.Log("Triggered with " + collision.name);
            Player.Instance.character.AddSpell(SpellManager.Instance.GetSpell(collision.name));
            Destroy(collision.gameObject);
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


    public void OnInteract (InputAction.CallbackContext callbackContext)
    {
        
    }

    
    #endregion
}
