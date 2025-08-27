using System;
using System.Collections.Generic;
using UnityEngine;
using DataSet;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public WeaponSelectUI ui;
    public List<EnemyData> enemyDataList = new List<EnemyData>();
    public List<WeaponData> weaponDataList = new List<WeaponData>();
    public List<WeaponData> selectedWeaponList = new List<WeaponData>();
    public int curWeaponIdx=-1;

    public int maxHealth = 5;
    public int health = 5;
    public Player player;

    void Awake() { instance = this; }

    public EnemyData GetEnemyData(string enemyName) { return enemyDataList.Find(e => e.enemyName == enemyName); }
    public WeaponData GetWeaponData(string weaponName) { return weaponDataList.Find(e => e.weaponName == weaponName); }
    public String CurWeapon => (curWeaponIdx >= 0) ? selectedWeaponList[curWeaponIdx].weaponName : null;
    public void ChangeWeapon(int num)
    {
        curWeaponIdx = num-1;
        ui.ChangeWeapon(num);
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