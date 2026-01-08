using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private SpellData spellData;
    
    private SpellData runtimeSpellData;
    public SpellData RuntimeSpellData => runtimeSpellData;

    public void InitializeRuntimeData()
    {
        if (spellData == null) return;

        runtimeSpellData = ScriptableObject.Instantiate(spellData);
        runtimeSpellData.Initialize();
    }
}