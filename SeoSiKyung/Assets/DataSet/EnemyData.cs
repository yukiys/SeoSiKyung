using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public string name;
    public int maxHP;
    public List<string> resistances;
}

[System.Serializable]
public class EnemyDataList
{
    public List<EnemyData> enemies;
}