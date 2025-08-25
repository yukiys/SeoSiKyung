using System;
using System.Collections.Generic;
using UnityEngine;
using DataSet;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<EnemyData> enemyDataList = new List<EnemyData>();
    public List<WeaponData> weaponDataList = new List<WeaponData>();

    public int maxHealth = 5;
    public int health = 5;
    public Player player;

    void Awake()
    {
        instance = this;
    }

    public EnemyData GetEnemyData(string enemyName)
    {
        return enemyDataList.Find(e => e.enemyName == enemyName);
    }

    public void HealthDown()
    {
        if (--health > 0)
        {
            return;
        }
        else
        {
            Debug.Log("Player Die!");
        }
    }
}