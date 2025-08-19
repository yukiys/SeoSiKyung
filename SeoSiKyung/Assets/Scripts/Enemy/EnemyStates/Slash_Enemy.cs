using UnityEngine;

public class Slash_Enemy : EnemyState
{
    float t, duration = 2f;

    public Slash_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.isDying = true;
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.Play("slash");
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