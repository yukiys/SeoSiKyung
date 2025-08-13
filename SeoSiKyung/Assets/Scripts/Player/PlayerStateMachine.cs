using UnityEngine;

public sealed class PlayerStateMachine
{
    public PlayerState Current { get; private set; }

    public void ChangeState(PlayerState next)
    {
        if (Current == next) return;
        Current?.Exit();
        Current = next;
        Current?.Enter();
    }

    public void Tick()      => Current?.Tick();
    public void FixedTick() => Current?.FixedTick();
}
