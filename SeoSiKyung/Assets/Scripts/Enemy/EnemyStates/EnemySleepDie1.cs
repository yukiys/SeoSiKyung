using UnityEngine;

public class EnemySleepDie1 : EnemyState
{
    float t, duration = 0.7f;
    
    public EnemySleepDie1(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.Play("SleepDie1");
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