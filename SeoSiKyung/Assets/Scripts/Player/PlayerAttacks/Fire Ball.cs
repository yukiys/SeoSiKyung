using Assets.DataSet;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public AttackType type =AttackType.Fire;
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {  
            var enemy = other.GetComponent<Enemy>();
            enemy.OnHit(type);
            Destroy(gameObject);
        } 
        
    }

   
}
