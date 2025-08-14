using Assets.DataSet;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Enemy enemy;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            enemy.OnHit(AttackType.Bludgeon);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            enemy.OnHit(AttackType.Pierce);
        }
    }
}
