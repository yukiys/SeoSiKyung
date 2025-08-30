using System;
using UnityEngine;

public class Chase_Enemy : EnemyState
{
    public Chase_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

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
        float dx = enemy.player.position.x - enemy.transform.position.x;
        int dir = dx > 0 ? 1 : -1;

        bool wall = enemy.WallAhead(dir);
        bool ground = enemy.GroundAhead(dir);
        bool isGrounded = enemy.IsGrounded();

        if (wall && isGrounded)
        {
            if (enemy.CanJump() && !enemy.RecentlyJumped())
            {
                enemy.Jump(dir);
                return;
            }
            enemy.rb.linearVelocity = new Vector2(0f, enemy.rb.linearVelocity.y);
            return;
        }
        if (!ground && isGrounded)
        {
            if (enemy.CanJump() && !enemy.RecentlyJumped())
            {
                enemy.Jump(dir);
                return;
            }
            enemy.rb.linearVelocity = new Vector2(0f, enemy.rb.linearVelocity.y);
            return;
        }

        enemy.rb.linearVelocity = new Vector2(dir * enemy.speed, enemy.rb.linearVelocity.y);
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
    }
}