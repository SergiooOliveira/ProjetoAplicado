using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resistance
{
    #region Variables
    public static readonly Dictionary<SpellAffinity, SpellAffinity> weaknessChart = new()
    {
        { SpellAffinity.Fire, SpellAffinity.Ice},
        { SpellAffinity.Wind, SpellAffinity.Fire },
        { SpellAffinity.Ice, SpellAffinity.Wind },
        { SpellAffinity.Light, SpellAffinity.Dark },
        { SpellAffinity.Dark, SpellAffinity.Light }
    };

    private const int MAX_RESISTANCE = 85;
    #endregion

    #region Serialized Fields
    [SerializeField] private SpellAffinity spellAfinity;
    [SerializeField] private float amount;    
    #endregion

    #region Property implementation
    public SpellAffinity SpellAfinity => spellAfinity;
    public float Amount => Mathf.Clamp(amount, 0, MAX_RESISTANCE);
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
    }
}