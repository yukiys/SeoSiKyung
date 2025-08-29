using UnityEngine;

public class Bludgeon_SleepEnemy : EnemyState
{
    public Bludgeon_SleepEnemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.isDying = true;
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.Play("bludgeon_sleep");
    }

    public override void LogicUpdate()
    {
        AnimatorStateInfo info = enemy.anim.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1f)
            Object.Destroy(enemy.gameObject);
    }
}