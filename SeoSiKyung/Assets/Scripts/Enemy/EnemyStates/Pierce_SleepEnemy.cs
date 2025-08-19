using UnityEngine;

public class Pierce_SleepEnemy : EnemyState
{
    float t, duration = 2f;
    
    public Pierce_SleepEnemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.isDying = true;
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.Play("pierce_sleep");
        t = duration;
    }

    public override void LogicUpdate()
    {
        AnimatorStateInfo info = enemy.anim.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1f){
            if (enemy.PierceCorpse != null)
                Object.Instantiate(enemy.PierceCorpse, enemy.transform.position, enemy.transform.rotation);
            Object.Destroy(enemy.gameObject);
        }
    }
}