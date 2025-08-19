using UnityEngine;

public class Trace_Enemy : EnemyState
{
    public Trace_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        base.Enter();
        AnimatorStateInfo info = enemy.anim.GetCurrentAnimatorStateInfo(0);
        if (enemy.idlewalk)
        {
            if (!info.IsName("idle") && !info.IsName("walk"))
                enemy.anim.Play("idle");
        }
        else
        {
            if (!info.IsName("idle"))
                enemy.anim.Play("idle");
        }
    }

    public override void PhysicsUpdate()
    {
        if (!enemy.player)
        {
            enemy.rd.linearVelocity = Vector2.zero;
            return;
        }

        float dx = enemy.player.position.x - enemy.transform.position.x;
        int dir = dx > 0f ? 1 : -1;

        if (!enemy.GroundAhead(dir) || enemy.WallAhead(dir))
        {
            enemy.rd.linearVelocity = Vector2.zero;
            return;
        }

        enemy.rd.linearVelocity = new Vector2(dir * enemy.speed, 0);
        enemy.sr.flipX = dir > 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.InAttackRange())
        {
            fsm.ChangeState(enemy.AttackState);
            return;
        }

        if (!enemy.InDetectRange() && CanLeave()) fsm.ChangeState(enemy.IdleState);
    }
}
