using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<EnemyData> EnemyDataList = new List<EnemyData>();

    public int health;
    public PlayerMove player;

    void Awake()
    {
        instance = this;
    }

    public EnemyData GetEnemyData(string Name)
    {
        return EnemyDataList.Find(e => e.name == Name);
    }

    public void HealthDown(int h)
    {
        if (health > h)
        {
            health -= h;
        }
        else
        {
            // player.die();
        }
    }
}
