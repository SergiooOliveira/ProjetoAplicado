using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Info and stats of enemy
    // TODO: Shouldn't be a MonoBehaviour. EnemyManager should be the Monobehaviour
    public EnemyData enemyData;
    private EnemyData runtimeData;

    public EnemyData RunTimeData => runtimeData;

    private void Awake()
    {
        enemyData.CharacterHp.Initialize();
        runtimeData = Instantiate(enemyData);
    }

    public void Interact()
    {
        Debug.Log(runtimeData.ToString());
    }

    /// <summary>
    /// This method should handle the move factor of the enemy
    /// </summary>
    public void Move()
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

    /// <summary>
    /// Call this method to make the enemy attack
    /// </summary>
    public void Attack()
    {
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
    /// Call this method to give damage to the enemy
    /// </summary>
    /// <param name="damageReceived">Damage Received</param>
    public void CalculateDamage(Player player, Spell spell)
    {
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
        Debug.Log($"RuntimeData.CharacterHp: {runtimeData.CharacterHp.Current}");
        if (runtimeData.CharacterHp.Current <= 0) Die();
    }

    /// <summary>
    /// Call this method to retrieve the resistance value if aplicable
    /// </summary>
    /// <param name="spellAfinity">Attack spell</param>
    /// <returns></returns>
    private float GetResistance (SpellAfinity spellAfinity)
    {        
        SpellAfinity resistanceAfinity = Resistance.weaknessChart.ContainsKey(spellAfinity) ? Resistance.weaknessChart[spellAfinity] : spellAfinity;

        foreach (Resistance r in runtimeData.CharacterResistances)
        {
            if (r.SpellAfinity == resistanceAfinity)
            {
                Debug.Log($"{r.SpellAfinity} is {r.Amount}% resistant to {spellAfinity}");
                return r.Amount;
            }
        }

        return 0f;
    }

    /// <summary>
    /// Call this method to destroy an Enemy and get it's drops 
    /// </summary>
    private void Die()
    {
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

        Debug.Log($"EnemyManager.Instance is {(EnemyManager.Instance == null ? "NULL" : "OK")}");
        Debug.Log($"Enemy reference (this) is {(this == null ? "NULL" : "OK")}");

        EnemyManager.Instance.RemoveEnemy(this);

        foreach (Item item in runtimeData.CharacterInventory)
        {
            int dropNumber = Random.Range(0, runtimeData.CharacterLevel * 2);

            // Add to Player Inventory
            Debug.Log($"Dropped {dropNumber} {item.ItemName} to Player");
        }
    }    
}
