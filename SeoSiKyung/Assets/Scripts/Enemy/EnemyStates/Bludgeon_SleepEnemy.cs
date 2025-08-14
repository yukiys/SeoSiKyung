using UnityEngine;

public class Bludgeon_SleepEnemy : EnemyState
{
    float t, duration = 2f;
    
    public Bludgeon_SleepEnemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        Debug.Log("Bludgeon Enter");
        enemy.isDying = true;
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.Play("bludgeon_sleep");
        t = duration;
    }

    public override void LogicUpdate()
    {
        t -= Time.deltaTime;
        if (t <= 0f)
        {
            Object.Destroy(enemy.gameObject);
        }
    }
}