using UnityEngine;

public class Patrol_Enemy : EnemyState
{
    float patrolMin = 1.5f, patrolMax = 3.0f;
    float timer;
    int dir;

    public Patrol_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        base.Enter();
        
        dir = Random.Range(0, 2) == 0 ? -1 : 1;
        if (!enemy.GroundAhead(dir) || enemy.WallAhead(dir)) dir *= -1;

        enemy.sr.flipX = dir > 0;
        enemy.anim.SetBool("isMoving", true);
        timer = Random.Range(patrolMin, patrolMax);
    }

    public override void PhysicsUpdate()
    {
        if (!enemy.GroundAhead(dir) || enemy.WallAhead(dir))
        {
            dir *= -1;
            enemy.sr.flipX = dir > 0;
        }

        enemy.rd.linearVelocity = new Vector2(dir * enemy.speed, 0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.InAttackRange())
        {
            fsm.ChangeState(enemy.AttackState);
            return;
        }
        if (enemy.InDetectRange() && CanLeave())
        {
            fsm.ChangeState(enemy.TraceState);
            return;
        }
        
        timer -= Time.deltaTime;
        if (timer <= 0f && CanLeave()) fsm.ChangeState(enemy.IdleState);
    }
}