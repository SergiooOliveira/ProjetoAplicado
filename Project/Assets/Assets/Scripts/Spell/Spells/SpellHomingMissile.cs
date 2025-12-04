using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class SpellHomingMissile : MonoBehaviour
{
    [SerializeField] private float spellSpeed;
    [SerializeField] private float spellRange;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float targetSearchRadious = 10f;

    private Transform target;
    private SpellData spellData;
    private Player caster;
    private Rigidbody2D rb;
    private Vector2 initialDirection;
    private float lifetime;

    public float TargetSearchRadious => targetSearchRadious;

    public void Initialize(SpellData spell, Player caster, Vector2 initialDir, Transform target)
    {
        this.spellData = spell;
        this.caster = caster;        
        this.target = target;
        this.initialDirection = initialDir;
   
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = initialDirection.normalized * this.spellSpeed;
        transform.right = initialDirection;

        if (spellSpeed > 0f && spellRange > 0f)
            lifetime = spellRange / spellSpeed;
        else if (spellRange > 0f)
            lifetime = spellRange;
        else
            lifetime = 10f;

        Destroy(gameObject, lifetime);
    }

    // TODO: Make so the spell has a slower speed in the begining and goes faster once x seconds pass
    private void FixedUpdate()
    {
        if (rb == null || target == null) return;        

        // Direction missile should move toward
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        Vector2 currentDir = rb.linearVelocity.normalized;

        float angle = Vector2.SignedAngle(currentDir, direction);
        float rotateStep = Mathf.Clamp(angle, -rotateSpeed * Time.fixedDeltaTime, rotateSpeed * Time.fixedDeltaTime);
        Vector2 newDir = Quaternion.Euler(0, 0, rotateStep) * currentDir;

        rb.linearVelocity = newDir.normalized * spellSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"Collision: {collision.name}");

        if (collision.CompareTag(GameManager.Instance.playerTag)) return;
        if (!collision.TryGetComponent<Enemy>(out Enemy enemy)) return;

        if (enemy != null) Debug.Log("Enemy not null");

        spellData.OnHit(caster, collision);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}