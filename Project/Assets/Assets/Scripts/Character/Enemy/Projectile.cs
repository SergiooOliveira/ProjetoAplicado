using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    #region Fields / Inspector

    [HideInInspector] public Enemy enemy;

    [Header("Projectile Settings")]
    public float speed = 5f;
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

    public void OnIdleAnimationEnd()
    {
        anim.SetTrigger("Move");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("Impact");
            enemy.ApplyDamage(collision);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            anim.SetTrigger("Impact");
        }
    }
    #endregion

    #region Public Methods
    private IEnumerator LifetimeRoutine()
    {
        Debug.Log("Starting IEnumerator");
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