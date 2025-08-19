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
        DebugDrawAttackBox();

        AnimatorStateInfo info = enemy.anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("attack") && info.normalizedTime >= 1f)
            fsm.ChangeState(enemy.IdleState);
    }
    
    void DebugDrawAttackBox()
    {
        float x = enemy.attackArea[0];
        float y = enemy.attackArea[1];
        float w = enemy.attackArea[2];
        float h = enemy.attackArea[3];

        int dir = enemy.sr.flipX ? -1 : 1;

        Vector2 leftBottom = new Vector2(x * -dir, y);
        Vector2 center = (Vector2)enemy.transform.position + leftBottom + new Vector2(w * 0.5f, h * 0.5f);

        Vector2 a = new Vector2(center.x - w/2f, center.y - h/2f);
        Vector2 b = new Vector2(center.x + w/2f, center.y - h/2f);
        Vector2 d = new Vector2(center.x - w/2f, center.y + h/2f);
        Vector2 e = new Vector2(center.x + w/2f, center.y + h/2f);
        Debug.DrawLine(a, b, Color.blue);
        Debug.DrawLine(b, e, Color.blue);
        Debug.DrawLine(e, d, Color.blue);
        Debug.DrawLine(d, a, Color.blue);
    }
}