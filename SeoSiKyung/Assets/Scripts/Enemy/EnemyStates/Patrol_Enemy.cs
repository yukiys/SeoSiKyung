using UnityEngine;

public class Patrol_Enemy : EnemyState
{
    float patrolMin = 1.5f;
    float patrolMax = 3.0f;
    float timer;
    int dir;

    public Patrol_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        dir = Random.Range(0, 2) == 0 ? -1 : 1;

        enemy.sr.flipX = dir < 0;
        enemy.anim.SetBool("isMoving", true);

        timer = Random.Range(patrolMin, patrolMax);
    }

    public override void PhysicsUpdate()
    {
        enemy.rd.linearVelocity = new Vector2(dir * enemy.speed, 0);
    }

    public override void LogicUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            enemy.rd.linearVelocity = Vector2.zero;
            enemy.anim.SetBool("isMoving", false);
            fsm.ChangeState(enemy.IdleState);
        }
    }
}
