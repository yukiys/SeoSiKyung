using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<DataSet.EnemyData> enemyDataList = new List<DataSet.EnemyData>();
    public List<DataSet.WeaponData> weaponDataList = new List<DataSet.WeaponData>();

    public int health;
    public Player player;

    void Awake()
    {
        instance = this;
    }

    public DataSet.EnemyData GetEnemyData(string enemyName)
    {
        return enemyDataList.Find(e => e.enemyName == enemyName);
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
