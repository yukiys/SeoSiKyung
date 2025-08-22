using System.IO;
using UnityEngine;
using DataSet;

public class DataManager : MonoBehaviour
{
    void Start()
    {
        LoadEnemyData(Path.Combine(Application.dataPath, "DataSet/EnemyData.json"));
        LoadWeaponData(Path.Combine(Application.dataPath, "DataSet/WeaponData.json"));
    }

    public void LoadEnemyData(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("file 존재 X");
            return;
        }

        string json = File.ReadAllText(path);
        EnemyDataList data = JsonUtility.FromJson<EnemyDataList>(json);

        if (data != null && data.enemies != null)
        {
            GameManager.instance.enemyDataList = data.enemies;
            Debug.Log("load 완료");
        }
    }
    public void LoadWeaponData(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("file 존재 X");
            return;
        }

        string json = File.ReadAllText(path);
        WeaponDataList weaponData = JsonUtility.FromJson<WeaponDataList>(json);

        if (weaponData != null && weaponData.weapons != null)
        {
            GameManager.instance.weaponDataList = weaponData.weapons;
            Debug.Log("load 완료");
        }
    }
}
