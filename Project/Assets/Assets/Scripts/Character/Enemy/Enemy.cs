using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    
    public void Interact()
    {
        Debug.Log(enemyData.ToString());
    }
}