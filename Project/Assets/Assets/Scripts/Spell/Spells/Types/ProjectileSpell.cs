using UnityEngine;

[CreateAssetMenu(menuName = "Spells/New Projectile Spell")]
public class ProjectileSpell : Spell
{
    public override void Cast(Vector3 position, Vector2 direction, Player player)
    {
        if (SpellPrefab == null)
        {
            Debug.Log("Spell prefab is null in ProjectileSpell");
            return;
        }

        if (player == null)
        {
            Debug.Log("Player is null in ProjectileSpell");
            return;
        }

        //Debug.Log($"[ProjectileSpell] Casting {SpellName} single projectile");
        GameObject instance = Instantiate(SpellPrefab, player.transform.position, Quaternion.identity);

        if (instance.TryGetComponent<SpellProjectile>(out SpellProjectile projectile))
            projectile.Initialize(this, direction, player);
        else
            Debug.LogWarning($"{SpellName}: prefab {SpellPrefab.name} missing SpellProjectile");
    }
}
