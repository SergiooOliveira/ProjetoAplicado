using UnityEngine;

public abstract class EnemyMovementBase : MonoBehaviour
{
    #region Fields

    protected Enemy owner;
    protected Rigidbody2D rb;

    [Header("Movement common")]
    public float speed = 3.5f;

    #endregion

    #region Initialization

    public virtual void Initialize(Enemy ownerEnemy)
    {
        owner = ownerEnemy;
        rb = owner.GetComponent<Rigidbody2D>();
    }

    #endregion

    #region FixedUpdate / Movement

    /// <summary>
    /// Called by Enemy each FixedUpdate
    /// </summary>
    public abstract void OnFixedUpdate();

    #endregion

    #region Targeting

    /// <summary>
    /// Set a target Transform to follow. null => no explicit target (patrol)
    /// </summary>
    public abstract void SetTarget(Transform t);

    public virtual void SetAttacking(bool attacking) { }
    public virtual void SetPlayerInSight(bool inSight) { }

    #endregion

    #region Grounded / Utility

    /// <summary>
    /// Returns if mover considers itself grounded (only for grounded mover)
    /// </summary>
    public virtual bool IsGrounded() => false;

    /// <summary>
    /// Stop Movement
    /// </summary>
    public virtual void StopMovement()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    #endregion
}
