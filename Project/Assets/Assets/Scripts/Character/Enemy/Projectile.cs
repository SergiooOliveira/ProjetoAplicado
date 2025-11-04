using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public Vector2 direction;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.linearVelocity = direction.normalized * speed;
    }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Logic damage player
                anim.SetTrigger("Impact");
            }
        }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}