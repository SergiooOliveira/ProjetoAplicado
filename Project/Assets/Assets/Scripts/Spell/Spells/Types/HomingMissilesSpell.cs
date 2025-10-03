using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Homing Missiles")]
public class HomingMissilesSpell : ProjectileSpell
{
    public override void Cast(Vector3 position, Vector2 direction)
    {
        if (SpellPrefab == null) return;

        //Debug.Log($"[WaterMissilesSpell] Casting {SpellName} at {position}");

        // TODO: Change this to be dynamic and we can choose the amount of missiles we want to send
        Vector2[] compassDir = new Vector2[]{
            Vector2.right,
            new Vector2(1, 1).normalized,
            Vector2.up,
            new Vector2(-1, 1).normalized,
            Vector2.left
        };

        foreach (Vector2 dir in compassDir)
        {
            // small offset so they don't overlap with player collider
            Vector3 spawnPos = position + (Vector3)dir * 0.5f;
            GameObject instance = Instantiate(SpellPrefab, spawnPos, Quaternion.identity);

            if (instance.TryGetComponent<SpellProjectile>(out SpellProjectile projectile))
                projectile.Initialize(this, dir);
            else
                Debug.LogWarning($"{name}: prefab {SpellPrefab.name} is missing SpellProjectile component");
        }
    }
}