using UnityEngine;

public class EnemyTraceState : EnemyState
{
    public EnemyTraceState(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.anim.SetBool("isMoving", true);
    }

    public override void PhysicsUpdate()
    {
        if (!enemy.player)
        {
            enemy.rd.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 dir = (enemy.player.position - enemy.transform.position).normalized;
        enemy.rd.linearVelocity = dir * enemy.speed;
        enemy.sr.flipX = dir.x < 0;
    }

    public override void LogicUpdate()
    {
        if (!enemy.player || Vector2.Distance(enemy.transform.position, enemy.player.position) > 6f)
        {
            enemy.rd.linearVelocity = Vector2.zero;
            enemy.anim.SetBool("isMoving", false);
            fsm.ChangeState(enemy.IdleState);
        }
    }
}
