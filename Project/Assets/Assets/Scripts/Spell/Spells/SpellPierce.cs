using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class SpellPierce : MonoBehaviour
{
    [SerializeField] private float spellSpeed;
    [SerializeField] private float spellRange;
    [SerializeField] private float spellPierceAmount;

    private SpellData runtimeSpellData;
    private Player caster;
    private Rigidbody2D rb;
    private float lifetime;
    private float counter = 0;

    public SpellData RuntimeSpellData => runtimeSpellData;

    public void Initialize(SpellData spell, Vector2 dir, Player caster)
    {
        this.runtimeSpellData = Instantiate(spell);
        this.runtimeSpellData.Initialize();
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
        if (collision.CompareTag(GameManager.Instance.interactableTag)) return;
        if (collision.CompareTag(GameManager.Instance.gridTag)) { Destroy(gameObject); return; }        

        runtimeSpellData.OnHit(caster, collision);

        counter++;
        if (counter >= spellPierceAmount)
            Destroy(gameObject);
    }
}