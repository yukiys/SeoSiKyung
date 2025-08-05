using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    string path;

    void Start()
    {
        path = Path.Combine(Application.dataPath, "enemy_data.json");
        LoadEnemyData();
    }

    public void LoadEnemyData()
    {
        if (!File.Exists(path))
        {
            Debug.Log("file 존재 X");
            return;
        }

        string json = File.ReadAllText(path);
        EnemyDataList enemyDataList = JsonUtility.FromJson<EnemyDataList>(json);

        if (enemyDataList != null && enemyDataList.enemies != null)
        {
            GameManager.instance.EnemyDataList = enemyDataList.enemies;
            Debug.Log("load 완료");
        }
    }
}
