using System.Collections.Generic;

namespace DataSet
{
    [System.Serializable]
    public class EnemyData
    {
        public string enemyName;
        public List<string> resistances;
        public int maxHp;
        public float speed;
        public float groundCheckDistance;
        public float wallCheckDistance;
        public float detectRange;
        public float attackRange;
        public bool isRanged;
        public List<float> attackArea;
    
        public string title;
        public string description;
        public string location;
        public int gimmickNumber;
        public List<string> gimmick;
    }

    [System.Serializable]
    public class EnemyDataList
    {
        public List<EnemyData> enemies;
    }
    
}