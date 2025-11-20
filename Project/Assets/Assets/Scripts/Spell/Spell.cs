using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Spell")]
public abstract class Spell : ScriptableObject, ISpell
{
    #region Serialized Fields
    [Header("Identity")]
    [SerializeField] private string spellName;
    [SerializeField] private string spellDescription;    
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private SpellTag spellTag;
    [SerializeField] private SpellAffinity spellAfinity;

    [Header("Mechanics")]
    [SerializeField] private float spellCooldown;    
    [SerializeField] private int spellCost;
    [SerializeField] private float spellCastTime;

    [Header("Behaviour")]
    [SerializeField] private SpellCastType spellCastType;
    [SerializeField] private SpellImpactType spellImpactType;

    [Header("Damage Settings")]
    [SerializeField] private float spellDamage;
    [SerializeField] private float spellRange;
    [SerializeField] private float spellProjectileSpeed;
    #endregion

    #region Property implementation
    // *----- Identity -----*
    public string SpellName => spellName;
    public string SpellDescription => spellDescription;    
    public GameObject SpellPrefab => spellPrefab;
    public SpellTag SpellTag => spellTag;
    public SpellAffinity SpellAfinity => spellAfinity;

    // *----- Mechanics -----*
    public float SpellCooldown => spellCooldown;
    public int SpellCost => spellCost;
    public float SpellCastTime => spellCastTime;

    // *----- Behaviour -----*
    public SpellCastType SpellCastType => spellCastType;
    public SpellImpactType SpellImpactType => spellImpactType;

    // *----- Damage Settings -----*
    public float SpellDamage => spellDamage;
    public float SpellRange => spellRange;
    public float SpellProjectileSpeed => spellProjectileSpeed;

    #endregion

    #region Methods
    public void Cast(Player caster, Vector2 direction)
    {
        switch (SpellCastType)
        {
            case SpellCastType.Projectile:
                CastProjectile(caster, direction);
                break;
            case SpellCastType.Area:
                CastArea(caster, direction);
                break;
            case SpellCastType.Self:
                CastSelf(caster);
                break;
        }
    }

    private void CastProjectile(Player caster, Vector2 direction)
    {
        GameObject instance = Instantiate(SpellPrefab, caster.transform.position, Quaternion.identity);

        if (instance.TryGetComponent<SpellProjectile>(out SpellProjectile projectile))
            projectile.Initialize(this, direction, caster);
    }

    private void CastArea(Player caster, Vector2 direction)
    {
        Debug.Log($"{SpellName} cast as area effect");
    }

    private void CastSelf(Player caster)
    {
        Debug.Log($"{SpellName} cast as self");
    }
    #endregion
}