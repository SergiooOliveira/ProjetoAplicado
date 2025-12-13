using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Spells/SpellData")]
public class SpellData : ScriptableObject, ISpell
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
    [SerializeField] private SpellManaCostType spellManaCostType;

    [Header("Behaviour")]
    [SerializeField] private SpellCastType spellCastType;
    [SerializeField] private SpellImpactType spellImpactType;

    [Header("Effects")]
    [SerializeField] private List<ScriptableObject> spellEffects;

    [Header("VFX")]
    [SerializeField] private GameObject orbVFX;
    [SerializeField] private GameObject projectileVFX;
    [SerializeField] private GameObject impactVFX;

    // Runtime Variables
    private List<ScriptableObject> runtimeEffects;
    private float currentMultiplier = 1f;
    public float CurrentCostPerSecond => spellCost * currentMultiplier;
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
    public SpellManaCostType SpellManaCostType => spellManaCostType;

    // *----- Behaviour -----*
    public SpellCastType SpellCastType => spellCastType;
    public SpellImpactType SpellImpactType => spellImpactType;

    // *----- Effects -----*
    public IReadOnlyList<ScriptableObject> SpellEffects => runtimeEffects;

    // *----- VFX -----*
    public GameObject OrbVFX => orbVFX;
    public GameObject ImpactVFX => impactVFX;
    #endregion

    public void Initialize()
    {
        currentMultiplier = 1f;
        runtimeEffects = new List<ScriptableObject>();

        foreach (SpellEffect original in spellEffects)
        {
            SpellEffect clone = Instantiate(original);

            if (clone is DamageEffect dmgEffect)
                dmgEffect.Initialize();

            runtimeEffects.Add(clone);
        }
    }

    public void UpdateDamageMultiplier(float multiplier)
    {
        this.currentMultiplier = multiplier;

        foreach (SpellEffect effect in runtimeEffects)
        {
            if (effect is DamageEffect dmgEffect)
            {
                dmgEffect.SetDamageMultiplier(multiplier);
            }
        }
    }

    #region Methods
    public GameObject Cast(Player caster, Vector2 direction)
    {
        switch (spellCastType)
        {
            case SpellCastType.Projectile:
                return CastProjectile(caster, direction);
            case SpellCastType.Homing:
                CastHoming(caster, direction);
                return null;
            case SpellCastType.Area:
                //return CastArea(caster, direction);
                return null;
            case SpellCastType.Self:
                CastSelf(caster);
                return null;
            case SpellCastType.Pierce:
                return CastPierce(caster, direction);
            case SpellCastType.Channeled:
                return CastChanneled(caster);

            default:
                return null;
        }
    }

    #region Cast Methods
    private GameObject CastProjectile(Player caster, Vector2 direction)
    {
        GameObject instance = Instantiate(SpellPrefab, caster.transform.position, Quaternion.identity);

        if (instance.TryGetComponent<SpellProjectile>(out SpellProjectile projectile))
            projectile.Initialize(this, direction, caster);
        else
            Debug.Log($"{SpellName} is missing SpellPorjectile component");

        return instance;
    }

    private void CastHoming(Player caster, Vector2 direction)
    {
        Vector2[] compassDir = new Vector2[] {
            Vector2.right,
            new Vector2(1, 1).normalized,
            Vector2.up,
            new Vector2(-1, 1).normalized,
            Vector2.left
        };

        foreach (Vector2 dir in compassDir)
        {
            Vector3 spawnPos = caster.transform.position + (Vector3)dir * 0.5f;
            GameObject instance = Instantiate(SpellPrefab, spawnPos, Quaternion.identity);

            if (instance.TryGetComponent<SpellHomingMissile>(out SpellHomingMissile projectile))
            {
                Enemy target = FindNearestEnemy(spawnPos, projectile.TargetSearchRadious);
                //if (target == null) return;
                projectile.Initialize(this, caster, dir, target != null ? target.transform : null);
            }
        }
    }

    private void CastArea(Player caster, Vector2 direction)
    {
        Debug.Log($"{SpellName} cast as area effect");
    }

    private void CastSelf(Player caster)
    {
        GameObject instance = Instantiate(SpellPrefab, caster.transform.position, Quaternion.identity);

        if (instance.TryGetComponent<SelfSpell>(out SelfSpell self))
            self.Initialize(this, caster, caster.transform);
        else
            Debug.Log($"{SpellName} is missing SelfSpell component");
    }

    private GameObject CastChanneled(Player caster)
    {
        GameObject instance = Instantiate(SpellPrefab, caster.transform.position, Quaternion.identity);

        if (instance.TryGetComponent<SpellChain>(out SpellChain chain))
            chain.Initialize(this, caster);
        else
            Debug.Log($"{SpellName} is missing SpellChain component");

        return instance;
    }

    private GameObject CastPierce(Player caster, Vector2 direction)
    {
        GameObject instance = Instantiate(SpellPrefab, caster.transform.position, Quaternion.identity);

        if (instance.TryGetComponent<SpellPierce>(out SpellPierce pierce))
            pierce.Initialize(this, direction, caster);
        else
            Debug.Log($"{SpellName} is missing SpellPierce component");

        return instance;
    }
    #endregion

    public void OnHit(Player caster, Collider2D target)
    {
        foreach (SpellEffect effect in spellEffects)
        {
            //Debug.Log($"OnHit effect: {effect}");
            if (effect != null)
                effect.Apply(caster, target);
        }
    }

    private Enemy FindNearestEnemy(Vector2 origin, float targetSearchRadious)
    {
        Enemy[] enemies = Enemy.FindObjectsOfType<Enemy>();
        Enemy nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float dist = Vector2.Distance(origin, enemy.transform.position);
            if (dist < minDist && dist <= targetSearchRadious)
            {
                minDist = dist;
                nearest = enemy;
                //Debug.LogWarning($"Found an enemy at {nearest.transform.position}");
            }
        }

        return nearest;
    }

    #endregion
}