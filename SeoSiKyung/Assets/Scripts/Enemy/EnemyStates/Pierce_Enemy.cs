using UnityEngine;

public class Pierce_Enemy : EnemyState
{
    float t, duration = 2f;

    public Pierce_Enemy(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm) { }

    public override void Enter()
    {
        enemy.isDying = true;
        enemy.rd.linearVelocity = Vector2.zero;
        enemy.anim.Play("pierce");
        t = duration;
    }

    public override void LogicUpdate()
    {
        t -= Time.deltaTime;
        if (t <= 0f)
        {
            if (enemy.PierceCorpse != null) Object.Instantiate(enemy.PierceCorpse, enemy.transform.position, enemy.transform.rotation);
            
            Object.Destroy(enemy.gameObject);
        }
    }
}