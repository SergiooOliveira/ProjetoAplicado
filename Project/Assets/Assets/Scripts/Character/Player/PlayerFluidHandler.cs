using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerFluidHandler : MonoBehaviour
{
    private Player player;
    private PlayerData playerData;

    private readonly Dictionary<FluidZone, float> activeFluids = new();

    private float damageTimer;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerData = player.RunTimePlayerData;
    }

    public void EnterFluid(FluidZone fluid)
    {
        if (activeFluids.ContainsKey(fluid))
            return;

        activeFluids.Add(fluid, fluid.speedMultiplier);
        RecalculateSpeed();
    }

    public void ExitFluid(FluidZone fluid)
    {
        if (!activeFluids.Remove(fluid))
            return;

        RecalculateSpeed();
    }

    private void RecalculateSpeed()
    {
        float finalMultiplier = 1f;

        foreach (float mult in activeFluids.Values)
            finalMultiplier *= mult;

        playerData.SetFluidSpeedMultiplier(finalMultiplier);
    }

    private void FixedUpdate()
    {
        if (activeFluids.Count == 0) return;

        foreach (var fluid in activeFluids.Keys)
        {
            if (fluid.damagePerSecond <= 0f) continue;

            damageTimer += Time.deltaTime;

            if (damageTimer >= 1f)
            {
                int dmg = Mathf.RoundToInt(fluid.damagePerSecond);
                playerData.CharacterHp.TakeDamage(dmg);
                player.PlayerHUDManager.SetHPBar((float)playerData.CharacterHp.Current / playerData.CharacterHp.Max);
                player.OnDamageTaken();
                damageTimer = 0f;
            }
        }
    }
}
