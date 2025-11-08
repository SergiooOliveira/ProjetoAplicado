using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    #region Fields / Inspector

    [Header("Projectile Settings")]
    public float speed = 5f;
    public int damage = 1;
    public Vector2 direction;
    public float lifetime = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool impactTriggered = false;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Sets initial speed
        rb.linearVelocity = direction.normalized * speed;

        // Automatically destroys after lifetime
        StartCoroutine(LifetimeRoutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: apply damage to the player
            anim.SetTrigger("Impact");
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            anim.SetTrigger("Impact");
        }
    }

    #endregion

    #region Public Methods

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(lifetime);
        TriggerImpact();
    }

    private void TriggerImpact()
    {
        if (impactTriggered) return;

        impactTriggered = true;

        // Stop the projectile immediately
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Play the impact animation
        if (anim != null)
            anim.SetTrigger("Impact");
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    #endregion
}