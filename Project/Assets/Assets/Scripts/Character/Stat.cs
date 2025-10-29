using UnityEngine;

[System.Serializable]
public class Stat
{
    private int current;
    [SerializeField] private int max;

    public int Current => current;
    public int Max => max;

    public Stat (int maxValue)
    {
        max = maxValue;
        current = max;
    }

    /// <summary>
    /// This method is called in the Awake so Current can be set
    /// </summary>
    public void Initialize()
    {
        current = max;
    }

    #region Methods
    /// <summary>
    /// Call this method to set a new Max value to a Stat
    /// </summary>
    /// <param name="amount">New max amount</param>
    public void SetNewMax(int amount) => max += amount;

    /// <summary>
    /// Call this method to increase both the current and the max
    /// </summary>
    /// <param name="amount">Amount to increase</param>
    public void IncreaseMaxCurrent(int amount)
    {
        Debug.LogWarning("Inside IncreaseMaxCurrent");
        max += amount;
        current += amount;
    }

    /// <summary>
    /// Call this method to heal the player
    /// </summary>
    /// <param name="amount">Amount to heal</param>
    public void IncreaseCurrent(int amount) => current = Mathf.Max(Max, Current + amount);

    /// <summary>
    /// Call this method to set the max xp to level up
    /// </summary>
    /// <param name="level">Current player level</param>
    /// <returns>New Max Xp</returns>
    public void NewXpMax(int level) => max = (int)(100 * Mathf.Pow((float)1.2, level - 1));

    public void TakeDamage(int damage) => current = Mathf.Max(0, current -= damage);

    #endregion
}
