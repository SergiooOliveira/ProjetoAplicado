using UnityEngine;

public class PlayerSpellSelector : MonoBehaviour
{
    #region Fields
    public PlayerData playerData;
    public Spell activeSpell { get; private set; }
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        InitializeActiveSpell();
    }

    private void Update()
    {
        HandleSpellSwitch();
    }
    #endregion

    #region Initialization
    private void InitializeActiveSpell()
    {
        if (playerData.CharacterSpells.Count > 0)
            SetActiveSpellByIndex(0);
    }
    #endregion

    #region Input Handling
    private void HandleSpellSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetActiveSpellByIndex(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetActiveSpellByIndex(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetActiveSpellByIndex(2);
    }
    #endregion

    #region Spell Management
    public void SetActiveSpellByIndex(int index)
    {
        if (index < 0 || index >= playerData.CharacterSpells.Count)
            return;

        // Update PlayerData
        //playerData.selectedSpellIndex = index;

        // Update local reference
        activeSpell = playerData.CharacterSpells[index];

        Debug.Log("Spell ativo mudou para: " + activeSpell.name);
    }
    #endregion
}