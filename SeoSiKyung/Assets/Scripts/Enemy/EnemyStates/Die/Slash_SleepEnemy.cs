using UnityEngine;

public class Slash_SleepEnemy : EnemyState
{
    float t, duration = 2f;

    public Slash_SleepEnemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.isDying = true;
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.Play("slash_sleep");
        t = duration;
    }

    public override void LogicUpdate()
    {
        AnimatorStateInfo info = enemy.anim.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1f)
            Object.Destroy(enemy.gameObject);
    }
}