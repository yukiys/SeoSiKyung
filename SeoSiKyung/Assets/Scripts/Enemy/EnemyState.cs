using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyFSM fsm;

    protected EnemyState(Enemy enemy, EnemyFSM fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}
