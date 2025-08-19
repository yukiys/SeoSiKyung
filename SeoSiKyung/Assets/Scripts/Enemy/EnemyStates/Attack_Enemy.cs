using UnityEngine;

public class Attack_Enemy : EnemyState
{
    public Attack_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {   
        if (enemy.isDying) return;

        enemy.rd.linearVelocity = Vector2.zero;
        
        AnimatorStateInfo info = enemy.anim.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("attack"))
            enemy.anim.Play("attack");
    }

    public override void LogicUpdate()
    {
        AnimatorStateInfo info = enemy.anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("attack") && info.normalizedTime >= 1f)
            fsm.ChangeState(enemy.IdleState);
    }
}