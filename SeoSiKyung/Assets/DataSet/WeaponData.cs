using System.Collections.Generic;

namespace DataSet
{
    [System.Serializable]
    public class WeaponData
    {
        public string weaponName;
        public string type;
        public int maxDurability;
        public bool isRanged;
        public List<float> attackRange;
    
        public string title;
        public string description;
        public string attackType;
    }

    [System.Serializable]
    public class WeaponDataList
    {
        public List<WeaponData> weapons;
    }
}