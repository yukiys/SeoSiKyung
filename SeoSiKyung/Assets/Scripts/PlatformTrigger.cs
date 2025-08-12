using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public List<Enemy> enemies=new List<Enemy>();
    Collider2D cd;

    void Awake()
    {
        cd = GetComponent<Collider2D>();
        FindEnemies();
    }

    private void FindEnemies()
    {
        Bounds bounds = cd.bounds;
        bounds.Expand(0.5f);

        Collider2D[] tracked = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f);

        enemies.Clear();
        foreach (var t in tracked)
        {
            Enemy enemy = t.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            foreach (var e in enemies)
            {
                e.WakeUp();
            }
        }
    }
}
