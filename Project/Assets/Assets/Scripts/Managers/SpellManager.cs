using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    public List<Spell> spells;

    public void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
}
