using Unity.Burst.Intrinsics;
using UnityEngine;

public class Awake_Enemy : EnemyState
{
    float t, duration = 0.7f;
    public Awake_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        t = duration;
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.SetBool("isMoving", false);
        enemy.anim.SetTrigger("isAwake");
    }

    public override void LogicUpdate()
    {
        t -= Time.deltaTime;
        if (t <= 0f)
        {
            fsm.ChangeState(enemy.IdleState);
        }
    }
}