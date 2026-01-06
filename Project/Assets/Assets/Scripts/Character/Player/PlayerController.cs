using System.Linq;
using TMPro;
// using UnityEditor.Experimental.GraphView;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhotonView))]
public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Global Variables
    private Player player;
    private PlayerData playerData;
    private GameObject currentChanneledObject;
    private Spell currentChanneledSpellData;
    private PlayerHUDManager playerHUDManager;
    private float manaAccumulator = 0f;

    private Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Animator animator;

    private bool canPlayerInteract = false;
    private Chest interactedChest = null;
    private NPC interactedNPC = null;

    private float horizontal;
    private float jumpingPower = 8f;
    private bool isFacingRight = true;

    // Inventory
    [Header("Inventory")]
    private bool isInventoryOpen = false;
    public GameObject inventoryPanel;
    public GameObject spellInventoryPanel;
    private SpellManager spellManager;



    // Add networking-related variables
    private Vector3 networkPosition;
    private float networkRotationZ;

    #endregion

    #region Unity Methods
    // public override void OnStartClient()
    // {
    //     //Debug.Log("test owner");
    //     if (IsOwner)
    //     {
    //         //Debug.Log("owner true");
    //         GetComponent<PlayerInput>().enabled = true;
    //     }
            
    // }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        playerData = player.RunTimePlayerData;
        spellManager = GetComponentInChildren<SpellManager>();
        playerHUDManager = GetComponentInChildren<PlayerHUDManager>();

        if (playerData == null)
            Debug.Log("Player data is null");

    }

    private void Start()
    {
        //playerData.CharacterInventory.Add();
    }

    private void FixedUpdate()
    {       
        if (photonView.IsMine)
        {
            rb.linearVelocity = new Vector2(horizontal * playerData.CharacterMovementSpeed, rb.linearVelocity.y);

            if (!isFacingRight && horizontal > 0f) Flip();
            else if (isFacingRight && horizontal < 0f) Flip();

            // Collect input every frame
            horizontal = Input.GetAxisRaw("Horizontal");

            // Sets the parameter value in the Animator
            animator.SetFloat("Speed", Mathf.Abs(horizontal));

            HandleChanneledMana();
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: Remake interactable object to add a spell
        //if (collision.tag == GameManager.Instance.grimoireTag)
        //{
        //    //Debug.Log("Triggered with " + collision.name);
        //    //playerData.AddSpell(SpellManager.GetSpell(collision.name));
        //    Destroy(collision.gameObject);
        //}

        // Cares about all the interactables
        if (collision.tag == GameManager.Instance.interactableTag)
        {
            canPlayerInteract = true;
            if (collision.TryGetComponent<Chest>(out Chest chest))
                interactedChest = chest;
            if (collision.TryGetComponent<NPC>(out NPC npc))
                interactedNPC = npc;
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
    /// <summary>
    /// Call this method to verify if the Player is on the Ground Layer
    /// </summary>
    /// <returns>True or False</returns>
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    /// <summary>
    /// This method is used to Flip the Player orientation
    /// </summary>
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
        // Debug.Log($"Carreguei no {callbackContext.ReadValue<Vector2>()}");
        horizontal = callbackContext.ReadValue<Vector2>().x;
    }

    /// <summary>
    /// This event is called when the player jumps
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        // if (callbackContext.performed) Debug.Log($"Space Pressed");
        //if (IsGrounded()) Debug.Log($"Space Pressed");

        // Debug.Log("jump press");
        if (callbackContext.performed && IsGrounded())
        {
            // Debug.Log("Jumping");
            if (photonView.IsMine)
            {
                // Debug.Log("is mine is true 😃");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);                
            }

        }
    }

    /// <summary>
    /// This event is called when the player attacks
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnAttack(InputAction.CallbackContext callbackContext)
    {
        int index = playerData.GetActiveSpellIndex();
        if (index == -1) return;

        SpellEntry activeSpellEntry = playerData.GetSlot(index);
        Spell spell = activeSpellEntry.spell;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 castDirection = (mousePos - rb.position).normalized;

        if (spell.RuntimeSpellData.SpellManaCostType == SpellManaCostType.Continuous)
        {
            if (callbackContext.started)
            {
                if (playerData.CharacterMana.Current >= spell.RuntimeSpellData.SpellCost)
                {
                    Debug.Log("Casting channeled spell");
                    currentChanneledObject = spell.RuntimeSpellData.Cast(player, castDirection);
                    currentChanneledSpellData = spell;
                }
                else StopChanneledSpell();
            }
            else if (callbackContext.canceled)
            {
                Debug.Log("Stopping channeled spell");
                StopChanneledSpell();
            }
        }
        else
        {
            if (callbackContext.started)
            {
                if (playerData.CharacterMana.Current >= spell.RuntimeSpellData.SpellCost)
                {
                    playerData.CharacterMana.ConsumeMana(spell.RuntimeSpellData.SpellCost);
                    playerHUDManager.SetManaValues((float)playerData.CharacterMana.Current / playerData.CharacterMana.Max);
                    spell.RuntimeSpellData.Cast(player, castDirection);
                }
                else
                {
                    Debug.Log("Not enough mana!");
                }
            }
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
        {
            if (interactedChest != null)
                interactedChest.Interact(playerData);
            else if (interactedNPC != null)
                interactedNPC.Interact(playerData);
        }
    }

    /// <summary>
    /// Call this method to Open and Close the inventory
    /// </summary>
    /// <param name="callbackContext"></param>
    public void ToggleInventory(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            isInventoryOpen = inventoryPanel.activeSelf;

            inventoryPanel.SetActive(!isInventoryOpen);

            // inventoryManagerUI.SetAllSlots();
        }
    }

    public void OpenSpellInventory(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            spellInventoryPanel.SetActive(!spellInventoryPanel.activeSelf);
        }
    }

    public void SelectSpell1(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            SpellEntry spell = playerData.CharacterEquippedSpells.Find(s => s.slot == 0);

            if (spell.spell != null)
            {
                int index = playerData.GetActiveSpellIndex();

                //Debug.LogWarning($"<Color=blue>Trying to swap Slot[{index}]</Color>");
                //Debug.LogWarning($"<Color=Yellow>Equipped spell: {playerData.GetSlot(index).spell.SpellName}</Color>");
                //Debug.LogWarning($"<Color=red>Trying to equip: {spell.spell.SpellName}</Color>");
                playerData.SwapActiveSpell(playerData.GetSlot(index), spell);
                spellManager.SetAllSlots();
            }
        }
    }

    public void SelectSpell2(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            SpellEntry spell = playerData.CharacterEquippedSpells.Find(s => s.slot == 1);

            if (spell.spell != null)
            {
                int index = playerData.GetActiveSpellIndex();

                //Debug.LogWarning($"<Color=blue>Trying to swap Slot[{index}]</Color>");
                //Debug.LogWarning($"<Color=Yellow>Equipped spell: {playerData.GetSlot(index).spell.SpellName}</Color>");
                //Debug.LogWarning($"<Color=red>Trying to equip: {spell.spell.SpellName}</Color>");
                playerData.SwapActiveSpell(playerData.GetSlot(index), spell);
                spellManager.SetAllSlots();
            }
        }
    }

    public void SelectSpell3(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            SpellEntry spell = playerData.CharacterEquippedSpells.Find(s => s.slot == 2);

            if (spell.spell != null)
            {
                int index = playerData.GetActiveSpellIndex();
                playerData.SwapActiveSpell(playerData.GetSlot(index), spell);
                spellManager.SetAllSlots();
            }
        }
    }
    #endregion

    #region Methods
    private void HandleChanneledMana()
    {
        if (currentChanneledObject != null && currentChanneledSpellData != null)
        {
            float realTimeCost = currentChanneledSpellData.RuntimeSpellData.CurrentCostPerSecond;
            float costThisFrame = realTimeCost * Time.fixedDeltaTime;
            manaAccumulator += costThisFrame;

            if (manaAccumulator >= 1f)
            {
                int manaToSpend = Mathf.FloorToInt(manaAccumulator);
                if (playerData.CharacterMana.Current >= manaToSpend)
                {
                    playerData.CharacterMana.ConsumeMana(manaToSpend);
                    manaAccumulator -= manaToSpend;
                    playerHUDManager.SetManaValues((float)playerData.CharacterMana.Current / playerData.CharacterMana.Max);
                    //Debug.Log($"Player Mana: <Color=blue>{playerData.CharacterMana.Current}</Color> consuming {manaToSpend}");
                }
                else
                {
                    //Debug.Log($"<Color=red>Mana ran out, stopping channeled spell</Color>");
                    StopChanneledSpell();
                    manaAccumulator = 0f;
                }
            }
        }
        else
        {
            manaAccumulator = 0f;
        }
    }

    private void StopChanneledSpell()
    {
        if (currentChanneledObject != null)
        {
            Destroy(currentChanneledObject);
            currentChanneledObject = null;
            currentChanneledSpellData = null;
        }
    }
    #endregion


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send position and rotation to the network
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation.eulerAngles.z);
             stream.SendNext(rb.linearVelocity.y); // Send y-velocity for syncing
        }
        else
        {
            // Receive position and rotation from the network
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotationZ = (float)stream.ReceiveNext();
            float receivedVerticalVelocity = (float)stream.ReceiveNext();
            
            // Apply received data
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, receivedVerticalVelocity); // Apply y-velocity
        }
    }
}