using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    public string name;
    public int maxHP;
    public List<string> resistances;
}

public class EnemyDataList
{
    public List<EnemyData> enemies;
}