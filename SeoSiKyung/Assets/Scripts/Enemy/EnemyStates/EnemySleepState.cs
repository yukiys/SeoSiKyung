    using UnityEngine;

public class EnemySleepState : EnemyState
{
    public EnemySleepState(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.Play("Sleep");
    }
}