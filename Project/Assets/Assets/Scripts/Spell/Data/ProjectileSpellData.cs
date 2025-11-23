using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Data/Projectile Settings")]
public class ProjectileSpellData : ScriptableObject
{
    [Header("Projectile Stats")]
    public float damage;
    public float speed;
    public float range;
}