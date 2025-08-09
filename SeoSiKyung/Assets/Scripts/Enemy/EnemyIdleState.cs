using UnityEngine;

public class EnemyIdleState : EnemyState
{
    float idleMin = 0.8f;
    float idleMax = 1.6f;
    float timer;
    public EnemyIdleState(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.SetBool("isMoving", false);

        timer = Random.Range(idleMin, idleMax);
    }

    public override void LogicUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            fsm.ChangeState(enemy.PatrolState);
        }
    }
}