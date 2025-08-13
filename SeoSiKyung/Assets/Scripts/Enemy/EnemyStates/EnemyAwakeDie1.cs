using UnityEngine;

public class EnemyAwakeDie1 : EnemyState
{
    float t, duration = 0.7f;
    
    public EnemyAwakeDie1(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.Play("AwakeDie1");
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