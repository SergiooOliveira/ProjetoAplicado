using TMPro;
using UnityEngine;

public class StatManagerUI : MonoBehaviour
{
    [Header ("Simple Stats")]
    [SerializeField] private TMP_Text hp;
    [SerializeField] private TMP_Text attack;
    [SerializeField] private TMP_Text attackSpeed;
    [SerializeField] private TMP_Text defense;
    [SerializeField] private TMP_Text movementSpeed;

    [Header("Resistances Stats")]
    [SerializeField] private TMP_Text fireResistance;
    [SerializeField] private TMP_Text iceResistance;
    [SerializeField] private TMP_Text windResistance;
    [SerializeField] private TMP_Text lightResistance;
    [SerializeField] private TMP_Text darkResistance;

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnEnable()
    {
        UpdateStats();
    }

    private void UpdateStats()
    {
        hp.text = player.RunTimePlayerData.CharacterHp.Current.ToString() + "/" + player.RunTimePlayerData.CharacterHp.Max.ToString();
        attack.text = player.RunTimePlayerData.CharacterAttackPower.ToString();
        attackSpeed.text = player.RunTimePlayerData.CharacterAttackSpeed.ToString();
        defense.text = player.RunTimePlayerData.CharacterDefense.ToString();
        movementSpeed.text = player.RunTimePlayerData.CharacterMovementSpeed.ToString();

        foreach (Resistance resistance in player.RunTimePlayerData.CharacterResistances)
        {
            switch (resistance.SpellAfinity)
            {
                case SpellAfinity.Fire:                    
                    fireResistance.text = resistance.Amount.ToString() + " %";
                    break;
                case SpellAfinity.Ice:
                    iceResistance.text = resistance.Amount.ToString() + " %";
                    break;
                case SpellAfinity.Wind:
                    windResistance.text = resistance.Amount.ToString() + " %";
                    break;
                case SpellAfinity.Light:
                    lightResistance.text = resistance.Amount.ToString() + " %";
                    break;
                case SpellAfinity.Dark:
                    darkResistance.text = resistance.Amount.ToString() + " %";
                    break;
            }
        }
    }
}
