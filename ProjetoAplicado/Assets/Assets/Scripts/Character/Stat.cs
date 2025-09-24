using UnityEngine;

public class Stat
{
    public int Current { get; private set; }
    public int Max { get; private set; }

    public Stat (int max)
    {
        Max = max;
        Current = max;
    }

    #region Methods
    /// <summary>
    /// Call this method to set a new Max value to a Stat
    /// </summary>
    /// <param name="amount">New max amount</param>
    public void SetNewMax (int amount)
    {
        Max = amount;
    }

    /// <summary>
    /// Call this method to increase the player hp
    /// </summary>
    /// <param name="amount">Amount to increase</param>
    public void IncreaseMaxHP (int amount)
    {
        Max += amount;
        Current += amount;
    }

    /// <summary>
    /// Call this method to heal the player
    /// </summary>
    /// <param name="amount">Amount to heal</param>
    public void Heal (int amount)
    {
        Current = Mathf.Max(Max, Current + amount);
    }

    /// <summary>
    /// Call this method to set the max xp to level up
    /// </summary>
    /// <param name="level">Current player level</param>
    /// <returns>New Max Xp</returns>
    public void NewXpMax (int level)
    {
        Max = (int)(100 * Mathf.Pow((float)1.2, level - 1));
    }
    #endregion
}
