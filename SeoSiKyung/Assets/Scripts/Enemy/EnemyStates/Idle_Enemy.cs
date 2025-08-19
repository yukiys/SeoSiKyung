using UnityEngine;

public class Idle_Enemy : EnemyState
{
    float idleMin = 0.8f, idleMax = 1.6f;
    float timer;
    public Idle_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        base.Enter();

        enemy.rd.linearVelocity = Vector2.zero;
        
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
        
        timer = Random.Range(idleMin, idleMax);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.InAttackRange())
        {
            fsm.ChangeState(enemy.AttackState);
            return;
        }
        if (enemy.InDetectRange() && CanLeave())
        {
            fsm.ChangeState(enemy.TraceState);
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f && CanLeave()) fsm.ChangeState(enemy.PatrolState);
    }
}