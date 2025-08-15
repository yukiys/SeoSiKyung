using UnityEngine;

public class Attack_Enemy : EnemyState
{
    public float t, duration = 2.2f;
    public Attack_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {   
        if (enemy.isDying) return;

        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.SetBool("isMoving", false);
        enemy.anim.Play("attack");

        t = duration;
    }

    public override void LogicUpdate()
    {
        t -= Time.deltaTime;
        if (t <= 0f) fsm.ChangeState(enemy.IdleState);
    }
}