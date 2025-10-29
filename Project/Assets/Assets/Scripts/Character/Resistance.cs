using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resistance
{
    public static readonly Dictionary<SpellAfinity, SpellAfinity> weaknessChart = new()
    {
        { SpellAfinity.Fire, SpellAfinity.Ice},
        { SpellAfinity.Wind, SpellAfinity.Fire },
        { SpellAfinity.Ice, SpellAfinity.Wind },
        { SpellAfinity.Light, SpellAfinity.Dark },
        { SpellAfinity.Dark, SpellAfinity.Light }
    };

    #region Serialized Fields
    [SerializeField] private SpellAfinity spellAfinity;
    [SerializeField] private float amount;
    [SerializeField] private const int MAX_RESISTANCE = 85;
    #endregion

    #region Property implementation
    public SpellAfinity SpellAfinity => spellAfinity;
    public float Amount => amount;
    #endregion

    public Resistance (int amount)
    {
        this.amount = amount;
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