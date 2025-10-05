using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    public void Start()
    {

    }

    /// <summary>
    /// Call this method to see all the equipped spells in the console
    /// </summary>
    public void ShowEquippedSpells()
    {
        foreach (Spell spell in Player.Instance.EquipedSpells)
        {
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



    /// <summary>
    /// Call this method to select the next spell in the spell book
    /// </summary>
    /// <param name="callbackContext"></param>
    public void NextSpell(InputAction.CallbackContext callbackContext)
    {
        for (int i = 0; i < Player.Instance.EquipedSpells.Count; i++)
        {
            if (Player.Instance.EquipedSpells[i].IsSpellSelected)
            {
                Player.Instance.EquipedSpells[i].Deselect();

                int nextIndex = (i + 1) % Player.Instance.EquipedSpells.Count;
                Player.Instance.EquipedSpells[nextIndex].Select();

                break;
            }
        }
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
