using UnityEngine;

public class Sleep_Enemy : EnemyState
{
    public Sleep_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.Play("Sleep");
    }
}