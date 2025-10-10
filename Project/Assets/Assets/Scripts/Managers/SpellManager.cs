using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;
    
    public List<Spell> Spells { get; private set; }

    public Transform spellDisplayCanvas;

    // Current spell selected
    public Spell selectedSpell = null;

    public void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        // Load all the Spells from Resource file
        Spells = new List<Spell>(Resources.LoadAll<Spell>("Spells"));
    }

    /// <summary>
    /// Call this method to see all the equipped spells in the console
    /// </summary>
    public void ShowEquippedSpells()
    {
        foreach (Player player in GameManager.Instance.Players)
        {
            foreach (Spell spell in player.RunTimePlayerData.CharacterEquipedSpells)
                Debug.Log("Equipped: " + spell.name);
        }
    }

    /// <summary>
    /// Call this method to get the spell object
    /// </summary>
    /// <param name="name">Spell name</param>
    /// <returns>Spell</returns>
    public Spell GetSpell(string name)
    {
        return Spells.Find(spell => spell.name == name);
    }

    // TODO: Change this to another manager, either GameManager or UiManager
    #region UI

    //public void UpdateSpellDisplayCanvas()
    //{
    //    foreach (Spell spell in Player.Instance.EquipedSpells)
    //        spellDisplayCanvas.GetComponentInChildren<Image>();
    //}

    #endregion
}
