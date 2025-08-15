using System;
using UnityEngine;

public class EnemyFSM
{
    public EnemyState CurrentState { get; set; }

    public void Initialize(EnemyState enemyState)
    {
        CurrentState = enemyState;
        CurrentState.Enter();
    }

    public void ChangeState(EnemyState next)
    {
        if (CurrentState == next) return;
        CurrentState.Exit();
        CurrentState = next;
        CurrentState.Enter();
    }

    public void LogicUpdate() => CurrentState?.LogicUpdate();
    public void PhysicsUpdate() => CurrentState?.PhysicsUpdate();
}