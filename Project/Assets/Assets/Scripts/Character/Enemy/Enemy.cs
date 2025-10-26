using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    private EnemyData runtimeData;

    public EnemyData RunTimeData => runtimeData;

    // AI (???)
    [SerializeField] private LayerMask playerLayer;    
    [SerializeField] private float sightRange, attackRange;
    private bool playerInSightRange;
    private Transform player;

    [SerializeField] private float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    [SerializeField] private float stepHeight = 4f;
    [SerializeField] private float stepCheckDistance = 0.3f;
    [SerializeField] private float stepSmooth = 5f;

    Seeker seeker;
    Rigidbody2D rb;
    bool grounded = true;

    private void Awake()
    {
        Initialize();
        runtimeData = Instantiate(enemyData);

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            /* 
             * See if any Player is in the sight range
             * If a Player is in sight range, store position and check if he is in attack range
             * If he is not on attack range, chase player.
             * if he is in range, attack
            */

            FindClosestPlayer();

            if (playerInSightRange)
            {
                float playerInAttackRange = Vector2.Distance(transform.position, player.position);

                if (playerInAttackRange <= attackRange)
                {
                    AttackPlayer();
                    return;
                }
                else
                {
                    ChasePlayer();
                    return;
                }
            }

            Patrolling();
        }
    }

    void FindClosestPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, sightRange, playerLayer);
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = hit.transform;
            }
        }

        if (closestPlayer != null)
        {
            playerInSightRange = true;
            player = closestPlayer;
        }
        else
        {
            playerInSightRange = false;
            player = null;
        }
    }

    private void Initialize()
    {
        enemyData.CharacterHp.Initialize();

        foreach (Item item in enemyData.CharacterInventory)
        {
            item.Initialize();
        }

        foreach (Equipment equipment in enemyData.CharacterEquipedItems)
        {
            equipment.Initialize();
        }
    }

    private void FixedUpdate()
    {
        if (path == null) return;

        grounded = Physics2D.OverlapCircle(feetPoint.position, groundedRadius, groundLayer);
        TryJumpAlongPath();

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * runtimeData.CharacterMovementSpeed;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, runtimeData.CharacterMovementSpeed);
    }

    #region Movement
    /// <summary>
    /// This method should handle the move factor of the enemy
    /// </summary>
    public void Patrolling()
    {
        Debug.LogWarning("Enemy is Patrolling");
    }

    private void ChasePlayer()
    {   
        Debug.LogWarning("Enemy is chasing, path starting");

        Debug.LogWarning("Grounded? " + grounded);

        if (grounded)
        {
            Debug.LogWarning("Looking for a path");
            seeker.StartPath(rb.position, player.position, OnPathComplete);
        }
    }

    // Put these fields in your enemy MonoBehaviour
    [Header("Jump / Step Settings")]
    [SerializeField] private float maxJumpHeight = 2.0f;       // maximum vertical difference we will try to jump
    [SerializeField] private float minStepHeight = 0.15f;      // minimum vertical difference treated as a step/jump
    [SerializeField] private float jumpVelocity = 10f;         // vertical velocity applied when jumping
    [SerializeField] private float spaceCheckHeight = 0.2f;    // how much vertical clearance we need above the obstacle
    [SerializeField] private float spaceCheckWidth = 0.3f;     // width for the overhead box check
    [SerializeField] private LayerMask groundLayer;            // what counts as ground/obstacle
    [SerializeField] private Transform feetPoint;              // where we test grounded (small circle at bottom)
    [SerializeField] private float groundedRadius = 0.12f;
    [SerializeField] private float jumpCooldown = 1.5f;        // seconds between allowed jumps
    private float lastJumpTime = -999f;

    [SerializeField] private bool debugGizmos = true;

    // Call this each FixedUpdate before applying horizontal movement
    private void TryJumpAlongPath()
    {
        // 1) must have a path and still have waypoints left
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count) return;

        // 2) must be grounded (so we don't multi-jump)
        if (!grounded) return;

        // 3) look ahead a bit to the next waypoint (and possibly the one after if needed)
        // Use the next waypoint we are moving towards; if it's essentially same Y then maybe check the next non-equal Y waypoint.
        int lookIndex = currentWaypoint;
        Vector2 nextWP = path.vectorPath[lookIndex];

        // If the next waypoint is at almost same x but small Y difference, it's valid; otherwise we may look further a few nodes
        // Try to find the next waypoint that has a Y difference beyond a tiny epsilon
        for (int i = currentWaypoint; i < Mathf.Min(path.vectorPath.Count, currentWaypoint + 4); i++)
        {
            float dy = path.vectorPath[i].y - rb.position.y;
            // pick the first waypoint with noticeable vertical difference
            if (Mathf.Abs(dy) > 0.01f)
            {
                nextWP = path.vectorPath[i];
                lookIndex = i;
                break;
            }
        }

        float heightDiff = nextWP.y - rb.position.y;

        if (debugGizmos)
        {
            Debug.DrawLine(rb.position, nextWP, Color.yellow);
            Debug.DrawRay(feetPoint.position, Vector2.up * (spaceCheckHeight + heightDiff), Color.cyan);
        }

        // 4) If the target is higher than a minimal step and less than or equal to max jump height -> consider jump
        if (heightDiff >= minStepHeight && heightDiff <= maxJumpHeight)
        {
            //Debug.LogWarning($"HeightDiff = {heightDiff} for nextWP.y={nextWP.y}");

            // 5) Check if there is space above the obstacle (so we don't jump into a ceiling)
            // We'll check an area at the landing position (slightly above nextWP) for obstacles.
            Vector2 spaceCheckCenter = new Vector2(nextWP.x, nextWP.y + (spaceCheckHeight * 0.5f));
            Vector2 boxHalf = new Vector2(spaceCheckWidth * 0.5f, spaceCheckHeight * 0.5f);

            Collider2D overlap = Physics2D.OverlapBox(spaceCheckCenter, boxHalf * 2f, 0f, groundLayer);

            if (debugGizmos)
            {
                // draw the box corners for debugging (approx)
                Vector2 tl = spaceCheckCenter + new Vector2(-boxHalf.x, boxHalf.y);
                Vector2 tr = spaceCheckCenter + new Vector2(boxHalf.x, boxHalf.y);
                Vector2 bl = spaceCheckCenter + new Vector2(-boxHalf.x, -boxHalf.y);
                Vector2 br = spaceCheckCenter + new Vector2(boxHalf.x, -boxHalf.y);
                Debug.DrawLine(tl, tr, Color.green);
                Debug.DrawLine(tr, br, Color.green);
                Debug.DrawLine(br, bl, Color.green);
                Debug.DrawLine(bl, tl, Color.green);
            }

            //if (debugGizmos)
            //    Debug.LogWarning($"Jump cooldown remaining: {Mathf.Max(0, jumpCooldown - (Time.time - lastJumpTime))}");
            Debug.LogWarning($"FeetY={feetPoint.position.y:F3}, PathNodeY={nextWP.y:F3}");

            // if nothing overlaps, we have space and can jump
            if (overlap == null)
            {
                if (Time.time - lastJumpTime >= jumpCooldown)
                {
                    // 6) Apply jump - set vertical velocity directly for consistent behavior
                    Vector2 v = rb.linearVelocity;
                    v.y = jumpVelocity;
                    rb.linearVelocity = v;

                    // optional: slightly push toward the waypoint horizontally so the arc moves forward
                    float dirX = Mathf.Sign(nextWP.x - rb.position.x);
                    //Debug.LogWarning("Jumping! heightDiff=" + heightDiff);
                    rb.linearVelocity = new Vector2(dirX * Mathf.Abs(rb.linearVelocity.x), jumpVelocity);
                    //rb.linearVelocity = new Vector2(dirX * Mathf.Abs(rb.linearVelocity.x), rb.linearVelocity.y);

                    // you may optionally increment currentWaypoint if this jump should skip small nodes:
                    lastJumpTime = Time.time;
                    currentWaypoint = lookIndex;
                }
            }
            else
            {
                // space is blocked - do nothing (or you could try to find a NodeLink2 or alternative route)
            }
        }
    }


    #endregion

    #region Attack
    /// <summary>
    /// Call this method to make the enemy attack
    /// </summary>
    public void AttackPlayer()
    {
        Debug.LogWarning("Enemy is attacking");

        switch (runtimeData.CharacterType)
        {
            case EnemyType.Ground:
                GroundAttack();
                break;
            case EnemyType.Flying:
                FlyingAttack();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Call this method to handle a ground enemy attack
    /// </summary>
    private void GroundAttack()
    {

    }

    /// <summary>
    /// Call this method to handle a flying enemy attack
    /// </summary>
    private void FlyingAttack()
    {

    }

    /// <summary>
    /// Call this method for Enemy to use the spell
    /// </summary>
    /// <param name="position">Player position</param>
    /// <param name="direction">Player direction</param>
    public void UseSpell(Vector3 position, Vector2 direction, Player player)
    {
        // TODO: Should be an override cause it's not the same logic
        runtimeData.CharacterEquipedSpells[0].Cast(position, direction, player);
    }
    #endregion

    #region Take Damage
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

        // Step 2: Resistance Multiplier
        float resistanteMultiplier = 1f - (GetResistance(spell.SpellAfinity) / 100f);
        baseDamage *= resistanteMultiplier;

        // Step 3: Level disadvantage penalties
        int levelDifference = runtimeData.CharacterLevel - player.GetPlayerLevel();
        if (levelDifference >= 5) baseDamage *= 0.75f;          // -25%
        else if (levelDifference >= 3) baseDamage *= 0.90f;     // -10%

        // Step 4: Clamp damage and apply
        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(baseDamage));

        runtimeData.CharacterHp.TakeDamage(finalDamage);
        //Debug.Log($"{player.RunTimePlayerData.CharacterName} - RuntimeData.CharacterHp: {runtimeData.CharacterHp.Current}");
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
        #endregion

        //Debug.Log($"EnemyManager.Instance is {(EnemyManager.Instance == null ? "NULL" : "OK")}");
        //Debug.Log($"Enemy reference (this) is {(this == null ? "NULL" : "OK")}");

        EnemyManager.Instance.RemoveEnemy(this);

        foreach (Item item in runtimeData.CharacterInventory)
        {
            /* TODO: Item drop logic
            * Using runtimeData.CharacterInventory and runtimeData.CharacterEquipedItems
            * Both have a chance to drop those items, slightly less chance for the equiped items
            * Utilize the rarity of an item to determine the drop rate and drop quantity
            */

            int dropNumber = Random.Range(0, runtimeData.CharacterLevel * 2);

            Debug.Log($"Adding {dropNumber} {item} to player inventory");

            // Add to Player Inventory
            player.RunTimePlayerData.AddItem(item, dropNumber);
        }
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
