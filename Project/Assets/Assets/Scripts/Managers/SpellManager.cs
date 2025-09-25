using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;
    
    public List<Spell> Spells { get; private set; }

    public Transform spellDisplayCanvas;

    public void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        // Load all the Spells from Resource file
        Spells = new List<Spell>(Resources.LoadAll<Spell>("Spells"));
    }

    public void Start()
    {

    }

    public void ShowEquippedSpells()
    {
        foreach (Spell spell in Player.Instance.EquipedSpells)
        {
            Debug.Log("Equipped: " + spell.name);
        }
    }

    public Spell GetSpell(string name)
    {
        return Spells.Find(spell => spell.name == name);
    }

    //public void UpdateSpellDisplayCanvas()
    //{
    //    foreach (Spell spell in Player.Instance.EquipedSpells)
    //        spellDisplayCanvas.GetComponentInChildren<Image>();
    //}
}
