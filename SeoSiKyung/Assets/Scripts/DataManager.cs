using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    string path;

    void Start()
    {
        path = Path.Combine(Application.dataPath, "DataSet/enemy_data.json");
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
        EnemyDataList data = JsonUtility.FromJson<EnemyDataList>(json);

        if (data != null && data.enemies != null)
        {
            GameManager.instance.EnemyDataList = data.enemies;
            Debug.Log("load 완료");
        }
    }
}
