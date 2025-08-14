using DataSet;
using UnityEngine;

public class FireBall : MonoBehaviour
{    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.GetComponent<Enemy>();
            Destroy(gameObject);
        } 
        
    }

   
}
