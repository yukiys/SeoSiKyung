using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyFSM fsm;
    protected float timeInState;
    protected float min = 0.1f;

    protected EnemyState(Enemy enemy, EnemyFSM fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public virtual void Enter() { timeInState = 0f; }
    public virtual void Exit() { }
    public virtual void LogicUpdate() { timeInState += Time.deltaTime; }
    public virtual void PhysicsUpdate() { }
    protected bool CanLeave() => timeInState >= min;
}
