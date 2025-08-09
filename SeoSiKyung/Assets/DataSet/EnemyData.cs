using System.Collections.Generic;

namespace DataSet
{
    [System.Serializable]
    public class EnemyData
    {
        public string enemyName;
        public List<string> resistances;
        public int maxHp;
        public List<float> attackRange;
    
        public string title;
        public string description;
        public int gimmickNumber;
        public List<string> gimmick;
    }

    [System.Serializable]
    public class EnemyDataList
    {
        public List<EnemyData> enemies;
    }
    
}