using UnityEngine;

public abstract class Spell : ScriptableObject, ISpell
{
    [Header("Identity")]
    [SerializeField] private string spellName;
    [SerializeField] private string spellDescription;
    [SerializeField] private SpellTag spellTag;
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private SpellAfinity spellAfinity;
    [SerializeField] private SpellProjectileType spellProjectileType;

    [Header("Mechanics")]
    [SerializeField] private int spellDamage;
    [SerializeField] private int spellRange;
    [SerializeField] private float spellTravelSpeed;
    [SerializeField] private float spellCooldown;
    [SerializeField] private float spellCastSpeed;
    [SerializeField] private int spellCost;
    [SerializeField] private float spellDuration;

    [Header("Conditions")]
    [SerializeField] private bool isSpellUnlocked;
    [SerializeField] private bool isSpellEquiped;
    [SerializeField] private bool isSpellSelected;

    [Header("Status Effects")]
    [SerializeField] private bool spellHasCC;
    [SerializeField] private bool spellHasBuff;
    [SerializeField] private bool spellHasDebuff;

    // Property implementations
    public string SpellName => spellName;
    public string SpellDescription => spellDescription;
    public SpellTag SpellTag => spellTag;
    public GameObject SpellPrefab => spellPrefab;
    public SpellAfinity SpellAfinity => spellAfinity;
    public SpellProjectileType SpellProjectileType => spellProjectileType;
    public int SpellDamage => spellDamage;
    public int SpellRange => spellRange;
    public float SpellTravelSpeed => spellTravelSpeed;
    public float SpellCooldown => spellCooldown;
    public float SpellCastSpeed => spellCastSpeed;
    public int SpellCost => spellCost;
    public float SpellDuration => spellDuration;

    public bool IsSpellUnlocked => isSpellUnlocked;
    public bool IsSpellEquiped => isSpellEquiped;
    public bool IsSpellSelected => isSpellSelected;

    public bool SpellHasCC => spellHasCC;
    public bool SpellHasBuff => spellHasBuff;
    public bool SpellHasDebuff => spellHasDebuff;

    #region Methods
    public virtual void Cast(Vector3 position, Vector2 direction)
    {
        //GameObject instance = Instantiate(spellPrefab, position, Quaternion.identity);

        //if (instance.TryGetComponent<SpellProjectile>(out SpellProjectile projectile))
        //    projectile.Initialize(this, direction);
        //else if (instance.TryGetComponent<SpellAoE>(out SpellAoE aoe))
        //    aoe.Initialize(this);
        //else if (instance.TryGetComponent<SpellBuff>(out SpellBuff buff))
        //    buff.Initialize(this);
    }

    /// <summary>
    /// Call this method to select this Spell
    /// </summary>
    public void Select()
    {
        isSpellSelected = true;
    }

    /// <summary>
    /// Call this method to deselect this Spell
    /// </summary>
    public void Deselect()
    {
        isSpellSelected = false;
    }
    #endregion
}
