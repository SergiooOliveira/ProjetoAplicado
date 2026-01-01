using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Effects/Freeze")]
public class FreezeEffect : SpellEffect
{
    [Header("Freeze Settings")]
    [Tooltip("How long the enemy stops moving in seconds.")]
    [SerializeField] private float baseDuration = 2f;

    // Runtime variable storing the actual duration (in case of upgrades)
    private float currentDuration;

    // Public accessor for the runtime value
    public float Duration => currentDuration;

    // Similar to DamageEffect, call this when instantiating the runtime clone
    public override void Initialize()
    {
        currentDuration = baseDuration;
        // Example: If you had global duration modifiers, apply them here.
        // currentDuration = baseDuration * caster.Stats.StatusDurationMultiplier;
    }

    // Optional: Method to permanently upgrade duration for this run
    public void AddBonusDuration(float extraSeconds)
    {
        currentDuration += extraSeconds;
    }

    public override void Apply(Player caster, Collider2D target)
    {
        // 1. Validate Target
        if (!target.TryGetComponent<Enemy>(out Enemy enemy)) return;

        Debug.Log($"<Color=cyan>Applying freeze for {currentDuration} seconds to {target.name}</Color>");

        // 2. Apply Freeze Logic on the Enemy
        // IMPORTANT: Your Enemy script needs a method to handle this!
        // It should disable movement/attacks and start a timer to re-enable them.
        enemy.ApplyFreeze(currentDuration);
    }

    // LOGIC ID: Used to check if we should Refresh or Add new
    public override string GetEffectID()
    {
        return "Freeze";
    }

    // UI STRING: What the player sees in tooltips
    public override string AddEffectString()
    {
        return $"Freezes enemies for {baseDuration:F1}s";
    }

    // STACKING LOGIC: What happens if they get hit by another freeze while frozen?
    public override void Refresh(SpellEffect newSpellEffect)
    {
        if (newSpellEffect is FreezeEffect newFreeze)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine($"<Color=cyan>Freeze Refresh Triggered</Color>");
            s.AppendLine($"Old duration: {this.currentDuration}");
            s.AppendLine($"New incoming duration: {newFreeze.Duration}");

            // Standard behavior for Crowd Control (CC) is to "refresh" the duration.
            // We update our current duration to match the incoming spell's duration.
            // When 'Apply' is called next by the spell system, it will re-apply this new duration to the enemy controller.
            this.currentDuration = newFreeze.Duration;

            // Optional alternative: Keep the longest duration
            // this.currentDuration = Mathf.Max(this.currentDuration, newFreeze.Duration);

            s.AppendLine($"<Color=cyan>Final refreshed duration: {this.currentDuration}</Color>");
            Debug.Log(s);
        }
    }
}