using Unity.Burst.Intrinsics;
using UnityEngine;

public class Awake_Enemy : EnemyState
{
    public Awake_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.rd.linearVelocity = Vector2.zero;
        
        AnimatorStateInfo info = enemy.anim.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("awake"))
            enemy.anim.Play("awake");
    }

    public override void LogicUpdate()
    {
        AnimatorStateInfo info = enemy.anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("awake") && info.normalizedTime >= 1f)
            fsm.ChangeState(enemy.IdleState);
    }
}