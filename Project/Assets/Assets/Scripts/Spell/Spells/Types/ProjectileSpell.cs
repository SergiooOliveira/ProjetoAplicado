using UnityEngine;

[CreateAssetMenu(menuName = "Spells/New Projectile Spell")]
public class ProjectileSpell : Spell
{
    public override void Cast(Vector3 position, Vector2 direction)
    {
        if (SpellPrefab == null) return;

        //Debug.Log($"[ProjectileSpell] Casting {SpellName} single projectile");
        GameObject instance = Instantiate(SpellPrefab, position, Quaternion.identity);

        if (instance.TryGetComponent<SpellProjectile>(out SpellProjectile projectile))
            projectile.Initialize(this, direction);
        else
            Debug.LogWarning($"{SpellName}: prefab {SpellPrefab.name} missing SpellProjectile");
    }
}
