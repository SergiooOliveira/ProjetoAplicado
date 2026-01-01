using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class SelfSpell : MonoBehaviour
{
    private SpellData runtimeSpellData;
    private Player caster;

    public SpellData RuntimeSpellData => runtimeSpellData;
    public void Initialize(SpellData spell, Player caster, Transform target)
    {
        this.runtimeSpellData = Instantiate(spell);
        this.runtimeSpellData.Initialize();

        this.caster = caster;        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameManager.Instance.playerTag))
        {
            runtimeSpellData.OnHit(caster, collision);

            Destroy(gameObject);
        }
    }
}