using UnityEngine;

public class Character : MonoBehaviour
{
    private string characterName;
    private Stat hp { get; set; } = new Stat(100);
    private Stat xp { get; set; } = new Stat(0);
    private Stat mana { get; set; } = new Stat(50);
    private float movementSpeed;
    private int resistances;
    private float attackSpeed;
    private int attack;
    private int defense;
    //private string afinity;
    private int level;
    private int unspentSkillPoints;
    private Spell[] equipedSpells;

    public string Name { get; set; }

    public float MovementSpeed { get; set; }

    public int Resistances { get; set; }

    public float AttackSpeed { get; set; }

    public int Attack {
        get => attack;
        set => attack = Mathf.Max(0, value);
    }

    public int Defense
    {
        get => defense;
        set => defense = Mathf.Max(0, value);
    }

    public int Level
    {
        get => level;
        set => level = Mathf.Max(1, value);
    }

    public Spell[] EquipedSpells { get; set; }

    public void Initialize(string name, int movementSpeed, int resistances, float attackSpeed, int attack, int defense, int level, Spell[] equipedSpells)
    {
        this.Name = name;
        this.MovementSpeed = movementSpeed;
        this.Resistances = resistances;
        this.AttackSpeed = attackSpeed;
        this.Attack = attack;
        this.Defense = defense;
        this.Level = level;
        this.EquipedSpells = equipedSpells;

        this.xp.NewXpMax(this.Level); // Calling this so Xp.Max != 0
    }
}
