using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resistance
{
    [SerializeField] private SpellAfinity spellAfinity;
    [SerializeField] private float amount;

    public SpellAfinity SpellAfinity => spellAfinity;
    public float Amount => amount;

    public static readonly Dictionary<SpellAfinity, SpellAfinity> weaknessChart = new()
    {
        { SpellAfinity.Fire, SpellAfinity.Ice},
        { SpellAfinity.Wind, SpellAfinity.Fire },
        { SpellAfinity.Ice, SpellAfinity.Wind },
        { SpellAfinity.Light, SpellAfinity.Dark },
        { SpellAfinity.Dark, SpellAfinity.Light }
    };

    public Resistance (int value)
    {
        amount = value;
    }
}