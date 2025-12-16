using NUnit.Framework;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables / References
    [Header("Data")]
    public EnemyData enemyData;
    private EnemyData runtimeData;
    private PlayerData runTimePlayerData;
    public HashSet<SpellEffect> appliedEffects = new HashSet<SpellEffect>();
    public EnemyData RunTimeData => runtimeData;

    [Header("Hitboxes")]
    [SerializeField] private List<AttackHitbox> hitboxes = new List<AttackHitbox>();
    private Dictionary<string, AttackHitbox> hitboxMap;

    [Header("AI Senses")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float sightRange = 8f;
    [SerializeField] private float attackRange = 1.5f;
    private Transform player;
    private bool playerInSightRange = false;

    [Header("Movement")]
    public EnemyMovementBase movement;

    [Header("References")]
    [SerializeField] private GameObject enemyHUDPrefab;    
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private GameObject hudInstance;
    Rigidbody2D rb;

    [Header("Spawn")]
    public EnemySpawnPoint spawnPoint;
    public EnemySpawner spawner;

    [Header("FlyEnemy Range Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    private EnemyAttack currentAttack;

    [Header("Boss Spell Attack")]
    public GameObject spellPrefab;

    private bool isDead = false;
    private bool isAttacking = false;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        runtimeData = Instantiate(enemyData);
        Initialize();        
        SetupHitboxes();

        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (movement == null) movement = GetComponent<EnemyMovementBase>();
        if (movement != null) movement.Initialize(this);
    }

    private void Start()
    {
        if (enemyHUDPrefab != null)
        {
            // Capture the EnemyHUD which is a child
            hudInstance = GetComponentInChildren<EnemyHUD>(true)?.gameObject;
            hudInstance.GetComponent<EnemyHUD>().Init(this);
        }

        StartCoroutine(AI_Tick());
    }

    private void FixedUpdate()
    {
        // Delegate movement when moving. move speed app to rigid body
        if (movement != null)
            movement.OnFixedUpdate();
    }
    #endregion

    #region Initialization

    private void Initialize()
    {
        runtimeData.CharacterHp.Initialize();

        foreach (ItemEntry entry in runtimeData.CharacterInventory)
        {
            if (entry.item != null)
                entry.item.Initialize();
        }

        foreach (EquipmentEntry equipment in runtimeData.CharacterEquipment)
        {
            if (equipment.equipment != null)
                equipment.equipment.Initialize();
        }

        runtimeData.EquipmentStats();
    }

    private void SetupHitboxes()
    {
        hitboxMap = new Dictionary<string, AttackHitbox>();

        foreach (var hb in hitboxes)
        {
            hitboxMap.Add(hb.name, hb);
        }
    }

    #endregion

    #region Senses

    void FindClosestPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, sightRange, playerLayer);
        Transform closest = null;
        float closestDist = Mathf.Infinity;
        foreach (var h in hits)
        {
            float d = Vector2.Distance(transform.position, h.transform.position);
            if (d < closestDist)
            {
                closestDist = d;
                closest = h.transform;
            }
        }

        player = closest;
        playerInSightRange = closest != null;
    }

    private IEnumerator AI_Tick()
    {
        while (true)
        {
            // If you use server/NetworkBehaviour
            // if (!IsServer) { yield return null; continue; }

            FindClosestPlayer();

            if (movement != null)
            {
                movement.SetPlayerInSight(playerInSightRange);
                movement.SetAttacking(isAttacking);
            }

            if (playerInSightRange && player != null)
            {
                float dist = Vector2.Distance(transform.position, player.position);
                if (dist <= attackRange)
                {
                    if (!isAttacking)
                    {
                        movement.SetTarget(null);
                        movement.StopMovement();
                        AttackPlayer();
                    }
                }
                else
                {
                    ChasePlayer();
                }
            }
            else
            {
                Patrolling();
            }

            UpdateAnimator(); // speed, grounded, etc.

            // Tick every 0.1s (10x per second, much lighter than Update)
            yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion

    #region Movement / High-Level Actions

    public void Patrolling()
    {
        if (isAttacking || playerInSightRange)
            return;

        // Example: mover.MoveTo(waypoint) or idle - keep simple here
        if (movement != null)
            movement.SetTarget(null); // no target -> do patrol inside mover if it supports it
    }

    public void ChasePlayer()
    {
        if (player == null) return;
        if (movement != null)
            movement.SetTarget(player);
    }

    public void EnableHitbox(string name)
    {
        if (hitboxMap.TryGetValue(name, out var hb))
            hb.EnableHitbox();
    }

    public void DisableHitbox(string name)
    {
        if (hitboxMap.TryGetValue(name, out var hb))
            hb.DisableHitbox();
    }

    public void AttackPlayer()
    {        
        if (isAttacking) return;
        isAttacking = true;

        // Pick attack from runtimeData.Attacks
        currentAttack = GetRandomAttack();

        if (animator != null) animator.SetTrigger(currentAttack.triggerName);

        // Attack flow: Animation event should call ApplyAttackDamage() and reset isAttacking = false
    }

    // Is Trigger by Animation Event
    // Using Script EnemyAnimationEvent
    public void ApplyAttackEffect()
    {
        if (player == null) { isAttacking = false; return; }

        var atk = currentAttack;
        if (atk == null) { isAttacking = false; return; }

        if (atk.attackType == AttackType.Melee)
        {
            // Define different attacks for each melee enemy
        }
        else if (atk.attackType == AttackType.Ranged)
        {
            if (runtimeData.CharacterCategory == EnemyCategory.CyclopeBat)
            {
                // Fire the projectile
                Vector2 direction = (player.position - firePoint.position).normalized;
                Player playerComponent = player.GetComponent<Player>();
                UseSpell(firePoint.position, direction, playerComponent);
            }
            else if (runtimeData.CharacterCategory == EnemyCategory.Necromancer)
            {
                Vector2 direction = (player.position - firePoint.position).normalized;
                Player playerComponent = player.GetComponent<Player>();
                UseSpell(firePoint.position, direction, playerComponent);
            }
            else if (runtimeData.CharacterCategory == EnemyCategory.Boss)
            {
                UseFallingHandSpell();
            }
        }

        isAttacking = false;
    }

    #endregion

    #region Attack / Spells

    /// <summary>
    /// Call this method for Enemy Fly to use the spell
    /// </summary>
    public void UseSpell(Vector3 position, Vector2 direction, Player player)
    {
        // Instantiate projectile
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Get the projectile script
        Projectile projectileScript = proj.GetComponent<Projectile>();

        // Define direction
        projectileScript.direction = (player.transform.position - firePoint.position).normalized;

        projectileScript.enemy = this;

        // Ignores collision with enemy that fired
        Collider2D enemyCollider = GetComponent<Collider2D>();
        Collider2D projectileCollider = proj.GetComponent<Collider2D>();
        if (enemyCollider != null && projectileCollider != null)
        {
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
        }
    }

    /// <summary>
    /// Call this method for Enemy Boss to use the spell
    /// </summary>
    public void UseFallingHandSpell()
    {
        if (spellPrefab == null) return;

        // Position above the player's head
        Vector3 spawnPos = player.position + Vector3.up * 2f;

        // Instantiate the spell prefab
        GameObject spell = Instantiate(spellPrefab, spawnPos, Quaternion.identity);

        SpellAttack spellAttack = spell.GetComponent<SpellAttack>();

        spellAttack.enemy = this;

        // Ignores collision with enemy that fired
        Collider2D enemyCollider = GetComponent<Collider2D>();
        Collider2D projectileCollider = spell.GetComponent<Collider2D>();
        if (enemyCollider != null && projectileCollider != null)
        {
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
        }

    }

    /// <summary>
    /// Random Attacks
    /// Each Attack have difrent animation / damage / chances
    /// </summary>
    private EnemyAttack GetRandomAttack()
    {
        float totalWeight = 0f;
        foreach (var atk in runtimeData.Attacks)
            totalWeight += atk.weight;

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var atk in runtimeData.Attacks)
        {
            cumulative += atk.weight;
            if (randomValue <= cumulative)
                return atk;
        }

        return runtimeData.Attacks[0]; // Fallback
    }

    /// <summary>
    /// Damage applied to the player
    /// </summary>
    public void ApplyDamage(Collider2D other)
    {
        Debug.Log($"Colliding with: {other.name}");

        if (!other.TryGetComponent<Player>(out Player player)) return;
        runTimePlayerData = player.RunTimePlayerData;

        if (runTimePlayerData == null) return;

        int totalDamage = Mathf.RoundToInt(runtimeData.CharacterAttackPower * (currentAttack.damage / 100f));

        Debug.Log($"Damage: {totalDamage}");

        runTimePlayerData.CharacterHp.TakeDamage(totalDamage);
        player.playerHUDManager.SetHPBar((float)runTimePlayerData.CharacterHp.Current / runTimePlayerData.CharacterHp.Max);
    }

    #endregion

    #region Take Damage / Death
    public void TakeDamage(DamageContext context)
    {
        float finalDamage = ApplyDefense(context.baseDamage);   
        finalDamage = ApplyAffinity(finalDamage, context);
        finalDamage = ApplyLevelScaling(finalDamage, context);

        runtimeData.CharacterHp.TakeDamage(Mathf.CeilToInt(finalDamage));
        //Debug.Log($"Final damage: <Color=orange>{Mathf.CeilToInt(finalDamage)}</Color>, leaving enemy with: {runtimeData.CharacterHp.Current}");

        if (runtimeData.CharacterHp.Current <= 0) Die(context.caster);
    }

    /// <summary>
    /// Call this method to retrieve the resistance value if aplicable
    /// </summary>
    /// <param name="spellAfinity">Attack spell</param>
    /// <returns></returns>
    public float GetResistance(SpellAffinity spellAffinity)
    {
        if (runtimeData?.CharacterResistances == null || runtimeData.CharacterResistances.Count == 0)
            return 1f;

        float totalResistances = runtimeData.CharacterResistances
            .Where(r => r.SpellAfinity == spellAffinity)
            .Sum(r => r.Amount);

        //Debug.Log($"<Color=green>GetResistance: {totalResistances}</Color>");

        return 1f + (totalResistances / 100f);
    }

    /// <summary>
    /// Call this method to destroy an Enemy and get it's drops 
    /// </summary>
    private void Die(Player player)
    {
        #region Null checks
        if (runtimeData == null)
        {
            Debug.Log("Enemy Data is null");
            return;
        }

        if (this == null)
        {
            Debug.Log("Enemy is null");
            return;
        }

        if (isDead) return;
        #endregion

        #region Item Drop
        foreach (ItemEntry entry in RunTimeData.CharacterInventory)
        {
            /* TODO: Item drop logic
            * Using runtimeData.CharacterInventory and runtimeData.CharacterEquipedItems
            * Both have a chance to drop those items, slightly less chance for the equiped items
            * Utilize the rarity of an item to determine the drop rate and drop quantity
            */
            if (entry.item == null) continue;

            // Flag for no drop chance defined
            if (!Item.rarityDropRates.TryGetValue(entry.item.RunTimeItemData.ItemRarity, out float dropChance))
            {
                Debug.LogWarning($"No drop chance defined for rarity {entry.item.RunTimeItemData.ItemRarity}, defaulting to 0.");
                dropChance = 0f;
            }

            bool dropped = false;

            if (entry.isGuarantee)
            {
                dropped = true;
                //Debug.Log($"Item: {entry.item.RunTimeItemData.ItemName} is a guarantee drop");
            }
            else
            {
                // Roll between 0 and 1
                float roll = UnityEngine.Random.value;
                dropped = roll <= dropChance;

                //Debug.Log($"Item: {entry.item.RunTimeItemData.ItemName}: Rarity: {entry.item.RunTimeItemData.ItemRarity}," +
                //    $" Roll: {roll:F2}, Drop chance: {dropChance * 100:F0}%," +
                //    $" Result: {(dropped ? "Dropped" : "No Drop")}");
            }

            // Add to Player Inventory
            if (dropped)
            {
                // Calculate the amount of items to give to the player
                // Amount varies between half the quantity and the full quantity
                int amount = Random.Range(entry.quantity / 2, entry.quantity);

                player.RunTimePlayerData.AddItem(entry, amount);

                player.DisplayNotification(entry.item.RunTimeItemData.ItemName, amount);
            }
        }
        #endregion

        #region Equipment Drop
        foreach (EquipmentEntry equipment in RunTimeData.CharacterEquipment)
        {
            if (equipment.equipment == null) continue;

            if (!Equipment.rarityDropRates.TryGetValue(equipment.equipment.RunTimeEquipmentData.ItemRarity, out float dropChance))
            {
                Debug.LogWarning($"No drop chance defined for rarity {equipment.equipment.RunTimeEquipmentData.ItemRarity}, defaulting to 0.");
                dropChance = 0f;
            }

            float roll = UnityEngine.Random.value;
            bool dropped = roll <= dropChance;

            //Debug.Log($"Item: {equipment.equipment.RunTimeEquipmentData.ItemName}: Rarity: {equipment.equipment.RunTimeEquipmentData.ItemRarity}," +
            //    $" Roll: {roll:F2}, Drop chance: {dropChance * 100:F0}%," +
            //    $" Result: {(dropped ? "Dropped" : "No Drop")}");

            if (dropped)
            {
                player.RunTimePlayerData.AddEquip(equipment);
                player.DisplayNotification(equipment.equipment.RunTimeEquipmentData.ItemName, 1);
            }
        }
        #endregion

        // Add gold to player inventory
        player.RunTimePlayerData.AddGold(RunTimeData.CharacterGold);

        // Add xp to player
        player.playerHUDManager.SetXPBar((float)RunTimeData.CharacterXp.Max / player.RunTimePlayerData.CharacterXp.Max);
        player.RunTimePlayerData.CharacterXp.AddExperience(player.RunTimePlayerData, RunTimeData.CharacterXp.Max);                

        // Destroy EnemyHUD
        if (hudInstance != null)
            Destroy(hudInstance);

        // Inform spawn system that this enemy died
        if (spawnPoint != null)
            spawnPoint.ClearEnemy();

        // Notify enemy death
        if (spawner != null && spawnPoint != null)
        {
            spawner.OnEnemyDeath(spawnPoint, this);
        }

        // Stop Movimentaion
        rb.linearVelocity = Vector2.zero;

        // Trigger Death Animation
        animator.SetTrigger("Death");

        isDead = true;
    }

    // Destroy Enemy
    // Animation End
    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }

    #region Auxiliary methods
    private float ApplyDefense(float damage)
    {
        float baseFactor = 0.95f;
        float scale = 0.04f;
        float minPercent = 0.2f;

        float reduction = Mathf.Pow(baseFactor, runtimeData.CharacterDefense * scale);
        float finalPercent = Mathf.Max(reduction, minPercent);

        Debug.Log($"ApplyDefense damage: {damage * finalPercent} with {runtimeData.CharacterDefense}");
        return damage * finalPercent;
    }

    private float ApplyAffinity (float damage, DamageContext context)
    {
        float bonusPercent = context.caster.GetAffinityBonuses(context.spell.RuntimeSpellData.SpellAfinity);
        float resistancePercent = this.GetResistance(context.spell.RuntimeSpellData.SpellAfinity);
        //float weakness = GetWeaknessMultiplier(context.spell);

        Debug.Log($"ApplyAffinity damage: {damage * bonusPercent / resistancePercent} with bonusPercent: {bonusPercent} and resistancePercent: {resistancePercent}");
        return damage * (bonusPercent / resistancePercent);   
    }

    private float ApplyLevelScaling(float damage, DamageContext context)
    {
        int dif = runtimeData.CharacterLevel - context.caster.GetPlayerLevel();

        if (dif >= 5)
        {
            Debug.Log($"ApplyLevelScaling: {damage * 0.75f} with dif: {dif}");
            return damage * 0.75f;
        }
        if (dif >= 3)
        {
            Debug.Log($"ApplyLevelScaling: {damage * 0.90f} with dif: {dif}");
            return damage * 0.90f;
        }

        Debug.Log($"ApplyLevelScaling: {damage} with dif: {dif}");
        return damage;
    }

    public float GetWeaknessMultiplier(SpellAffinity spell, Enemy enemy)
    {
        if (!Resistance.weaknessChart.TryGetValue(spell, out SpellAffinity weakAgainst)) return 1f;

        bool enemyHasAffinity = enemy.runtimeData.CharacterResistances
            .Any(r => r.SpellAfinity == weakAgainst);

        return enemyHasAffinity ? 1.5f : 1f;
    }
    #endregion
    #endregion

    #region Animator helper

    private void UpdateAnimator()
    {
        if (animator == null || rb == null) return;
        float speed = Mathf.Abs(rb.linearVelocity.x);

        // Update animator parameters
        if (HasAnimatorParameter("Speed", AnimatorControllerParameterType.Float))
            animator.SetFloat("Speed", speed);

        if (movement != null)
        {
            if (HasAnimatorParameter("VerticalVelocity", AnimatorControllerParameterType.Float))
                animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);

            if (HasAnimatorParameter("IsGrounded", AnimatorControllerParameterType.Bool))
                animator.SetBool("IsGrounded", movement.IsGrounded());
        }

        // Flip sprite based only on player position
        if (player != null)
        {
            if (player.position.x > transform.position.x)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (player.position.x < transform.position.x)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Check if the parameters exist
    private bool HasAnimatorParameter(string paramName, AnimatorControllerParameterType type)
    {
        if (animator == null) return false;

        foreach (var param in animator.parameters)
        {
            if (param.name == paramName && param.type == type) return true;
        }
        return false;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    #endregion
}