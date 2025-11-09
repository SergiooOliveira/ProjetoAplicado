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
    public EnemyData RunTimeData => runtimeData;

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
    private GameObject hudInstance;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    [Header("Spawn")]
    public EnemySpawnPoint spawnPoint;
    public EnemySpawner spawner;

    [Header("FlyEnemy Range Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    private EnemyData.EnemyAttack currentAttack;

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
        GiveStat();

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

        foreach (InventoryItem entry in runtimeData.CharacterInventory)
        {
            if (entry.item != null)
                entry.item.Initialize();
        }

        foreach (EquipmentEntry equipment in runtimeData.CharacterEquipItems)
        {
            if (equipment.equipment != null)
                equipment.equipment.Initialize();
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

    public void AttackPlayer()
    {
        if (isAttacking) return;
        isAttacking = true;

        // Pick attack from runtimeData.Attacks
        currentAttack = GetRandomAttack();
        if (animator != null) animator.SetTrigger(currentAttack.triggerName);

        // Attack flow: Animation event should call ApplyAttackDamage() and reset isAttacking = false
    }

    // Player Take Damage
    // Is Trigger by Animation Event
    // Using Script EnemyAnimationEvent
    public void ApplyAttackEffect()
    {
        if (player == null) { isAttacking = false; return; }

        // Pega o ataque atual que disparou a animação
        var atk = currentAttack;
        if (atk == null) { isAttacking = false; return; }

        if (atk.attackType == AttackType.Melee)
        {
            // TODO:
            // Apply damage to player
            //player.TakeDamage(atk.damage);
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
                // TODO: Attack Necromancer
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
    /// Call this method for Enemy to use the spell
    /// </summary>
    public void UseSpell(Vector3 position, Vector2 direction, Player player)
    {
        // Instantiate projectile
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Get the projectile script
        Projectile projectileScript = proj.GetComponent<Projectile>();

        // Define direção
        projectileScript.direction = (player.transform.position - firePoint.position).normalized;

        // Ignores collision with enemy that fired
        Collider2D enemyCollider = GetComponent<Collider2D>();
        Collider2D projectileCollider = proj.GetComponent<Collider2D>();
        if (enemyCollider != null && projectileCollider != null)
        {
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
        }
    }

    public void UseFallingHandSpell()
    {
        if (spellPrefab == null) return;

        // Position above the player's head
        Vector3 spawnPos = player.position + Vector3.up * 2f;

        // Instantiate the spell prefab
        GameObject spell = Instantiate(spellPrefab, spawnPos, Quaternion.identity);
    }

    // Random Attacks
    // Each Attack have difrent animation / damage / chances
    private EnemyData.EnemyAttack GetRandomAttack()
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

    #endregion

    #region Take Damage / Death

    /// <summary>
    /// Call this method to give damage to the enemy
    /// </summary>
    /// <param name="damageReceived">Damage Received</param>
    public void CalculateDamage(Player player, Spell spell)
    {
        #region Null checks
        if (enemyData == null)
        {
            Debug.Log("Enemy Data is null");
            return;
        }

        if (spell == null)
        {
            Debug.Log("Spell is null");
            return;
        }

        if (player == null)
        {
            Debug.Log("Player is null");
            return;
        }
        #endregion

        // Step 1: Percentile defense reduction
        float defenseMultiplier = 100f / (100f + runtimeData.CharacterDefense);
        float baseDamage = spell.SpellDamage * defenseMultiplier;

        /* Result between the Spell Afinity and the Enemy Resistance
         * But Player can have an Equipment to add damage to said Resistance
         */

        // Player equips that match the afinity
        List<EquipmentEntry> playerEquips = player.RunTimePlayerData.CharacterEquipItems
            .FindAll(entry =>
                entry.isEquipped &&
                entry.equipment != null &&
                entry.equipment.RunTimeEquipmentData != null &&
                entry.equipment.RunTimeEquipmentData.ItemDamageAfinity != null &&
                entry.equipment.RunTimeEquipmentData.ItemDamageAfinity
                    .Any(ida => ida.SpellAfinity == spell.SpellAfinity));

        // Sum all the damage bonus to that resistance
        float resistanceDamage = playerEquips
            .SelectMany(entry => entry.equipment.RunTimeEquipmentData.ItemDamageAfinity)
            .Where(r => r.SpellAfinity == spell.SpellAfinity)
            .Sum(r => r.Amount);

        float resistanceMultiplier = 1f - (GetResistance(spell.SpellAfinity) / 100f) + (resistanceDamage / 100f);

        baseDamage *= resistanceMultiplier;

        // Step 3: Level disadvantage penalties
        int levelDifference = runtimeData.CharacterLevel - player.GetPlayerLevel();
        if (levelDifference >= 5) baseDamage *= 0.75f;          // -25%
        else if (levelDifference >= 3) baseDamage *= 0.90f;     // -10%

        // Step 4: Clamp damage and apply
        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(baseDamage));

        runtimeData.CharacterHp.TakeDamage(finalDamage);
        
        //Debug.Log($"{player.RunTimePlayerData.CharacterName} did {finalDamage} damage - Enemy hp: {runtimeData.CharacterHp.Current}");
                
        if (runtimeData.CharacterHp.Current <= 0) Die(player);
    }

    /// <summary>
    /// Call this method to retrieve the resistance value if aplicable
    /// </summary>
    /// <param name="spellAfinity">Attack spell</param>
    /// <returns></returns>
    private float GetResistance(SpellAfinity spellAfinity)
    {
        SpellAfinity resistanceAfinity = Resistance.weaknessChart.ContainsKey(spellAfinity) ? Resistance.weaknessChart[spellAfinity] : spellAfinity;

        foreach (Resistance r in runtimeData.CharacterResistances)
        {
            if (r.SpellAfinity == resistanceAfinity)
            {
                // Debug.Log($"{r.SpellAfinity} is {r.Amount}% resistant to {spellAfinity}");
                return r.Amount;
            }
        }

        return 0f;
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
        foreach (InventoryItem entry in RunTimeData.CharacterInventory)
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
        foreach (EquipmentEntry equipment in RunTimeData.CharacterEquipItems)
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

        // Destroy EnemyHUD
        if (hudInstance != null)
            Destroy(hudInstance);

        // Inform spawn system that this enemy died
        if (spawnPoint != null)
            spawnPoint.ClearEnemy();

        // Notify enemy death
        if (spawner != null)
            spawner.OnEnemyDeath(spawnPoint);

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

    #endregion

    #region Stats / Equipment Bonuses

    /// <summary>
    /// Call this method to apply the equipment bonus
    /// </summary>
    private void GiveStat()
    {
        foreach (EquipmentEntry equipment in enemyData.CharacterEquipItems)
        {
            Equipment iterationEquipment = equipment.equipment;

            if (equipment.isEquipped)
            {
                // Int and floats
                runtimeData.AddBonusHp(iterationEquipment.RunTimeEquipmentData.ItemHpBonus);
                runtimeData.AddBonusAttack(iterationEquipment.RunTimeEquipmentData.ItemAttackBonus);
                runtimeData.AddBonusAttackSpeed(iterationEquipment.RunTimeEquipmentData.ItemAttackSpeedBonus);
                runtimeData.AddBonusDefense(iterationEquipment.RunTimeEquipmentData.ItemDefenseBonus);
                runtimeData.AddBonusMana(iterationEquipment.RunTimeEquipmentData.ItemManaBonus);
                runtimeData.AddBonusMovementSpeed(iterationEquipment.RunTimeEquipmentData.ItemMovementSpeedBonus);

                // Resistances
                foreach (Resistance resistance in iterationEquipment.RunTimeEquipmentData.ItemResistanceBonus)
                {
                    runtimeData.AddResistance(resistance);
                }
            }
        }
    }

    #endregion

    #region Animator helper

    private void UpdateAnimator()
    {
        if (animator == null || rb == null) return;
        float speed = Mathf.Abs(rb.linearVelocity.x);

        if (HasAnimatorParameter("Speed", AnimatorControllerParameterType.Float))
            animator.SetFloat("Speed", speed);

        if (movement != null)
        {
            if (HasAnimatorParameter("VerticalVelocity", AnimatorControllerParameterType.Float))
                animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);

            if (HasAnimatorParameter("IsGrounded", AnimatorControllerParameterType.Bool))
                animator.SetBool("IsGrounded", movement.IsGrounded());
        }

        // Flip sprite depending on velocity.x
        if (rb.linearVelocity.x > 0.05f) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (rb.linearVelocity.x < -0.05f) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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