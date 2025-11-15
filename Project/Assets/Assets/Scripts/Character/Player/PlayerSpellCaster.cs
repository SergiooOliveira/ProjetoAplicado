using UnityEngine;

public class PlayerSpellCaster : MonoBehaviour
{
    #region Serialized Fields
    [Header("Player Data")]
    [SerializeField] private PlayerData playerData;  // Reference to player data
    #endregion

    #region Unity Callbacks
    private void Update()
    {
        HandleSpellCastInput();
    }
    #endregion

    #region Input Handling
    /// <summary>
    /// Detects player input to cast the active spell
    /// </summary>
    private void HandleSpellCastInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryCastActiveSpell();
        }
    }
    #endregion

    #region Spell Casting
    /// <summary>
    /// Attempts to cast the player's active spell
    /// </summary>
    private void TryCastActiveSpell()
    {
        Spell spell = playerData.GetActiveSpell();

        if (spell == null)
        {
            Debug.Log("Nenhum spell ativo!");
            return;
        }

        if (!spell.IsReadyToCast())
        {
            Debug.Log("Spell está em cooldown!");
            return;
        }

        // Register the launch for cooldown
        spell.RegisterCast();

        // 1. Player position
        Vector3 castPos = transform.position;

        // 2. Mouse direction in the world
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorld - castPos).normalized;

        // 3. Player Reference
        Player player = GetComponent<Player>();

        // 4. Cast the spell
        spell.Cast(castPos, direction, player);
    }
    #endregion
}