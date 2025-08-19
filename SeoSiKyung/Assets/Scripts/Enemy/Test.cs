using Assets.DataSet;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Enemy enemy;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("SLASH!");
            enemy.OnHit(AttackType.Slash);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("BLUDGEON!");
            enemy.OnHit(AttackType.Bludgeon);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("PIERCE");
            enemy.OnHit(AttackType.Pierce);
        }
    }
}
