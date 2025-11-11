using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resistance
{
    public static readonly Dictionary<SpellAffinity, SpellAffinity> weaknessChart = new()
    {
        { SpellAffinity.Fire, SpellAffinity.Ice},
        { SpellAffinity.Wind, SpellAffinity.Fire },
        { SpellAffinity.Ice, SpellAffinity.Wind },
        { SpellAffinity.Light, SpellAffinity.Dark },
        { SpellAffinity.Dark, SpellAffinity.Light }
    };

    #region Serialized Fields
    [SerializeField] private SpellAffinity spellAfinity;
    [SerializeField] private float amount;
    [SerializeField] private const int MAX_RESISTANCE = 85;
    #endregion

    #region Property implementation
    public SpellAffinity SpellAfinity => spellAfinity;
    public float Amount => amount;
    #endregion

    public Resistance (SpellAffinity affinity, int amount)
    {
        this.spellAfinity = affinity;
        this.amount = amount;
    }

    public Resistance (Resistance r)
    {
        this.spellAfinity = r.spellAfinity;
        this.amount = r.amount;
    }

    /// <summary>
    /// Call this method to add an amount to a resistance
    /// </summary>
    /// <param name="amount">Amount to add in %</param>
    public void AddAmount(float amount)
    {
        this.amount += amount;
        this.amount = Mathf.Clamp(this.amount, 0, MAX_RESISTANCE);
    }
}