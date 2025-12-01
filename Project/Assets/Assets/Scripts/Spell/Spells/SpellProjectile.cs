using System.Linq;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D), typeof(Collider2D))]
public class SpellProjectile : MonoBehaviour
{
    [SerializeField] private float spellSpeed;
    [SerializeField] private float spellRange;

    private SpellData spellData;
    private Player caster;
    private Rigidbody2D rb;
    private float lifetime;

    public void Initialize(SpellData spell, Vector2 dir, Player caster)
    {
        this.spellData = spell;
        this.caster = caster;

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = dir.normalized * spellSpeed;

        if (spellSpeed > 0f && spellRange > 0f)
            lifetime = spellRange / spellSpeed;
        else if (spellRange > 0f)
            lifetime = spellRange;
        else
            lifetime = 10f;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameManager.Instance.playerTag)) return;
        if (collision.CompareTag(GameManager.Instance.gridTag)) return;
        if (collision.CompareTag(GameManager.Instance.interactableTag)) return;

        spellData.OnHit(caster, collision);
        
        Destroy(gameObject);
    }
}