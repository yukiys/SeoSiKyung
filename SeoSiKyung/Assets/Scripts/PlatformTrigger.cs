using TMPro;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public EnemyMove[] enemies;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var e in enemies)
            {
                e.WakeUp();
            }
        }
    }
}
