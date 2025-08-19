using UnityEngine;

public sealed class PlayerFSM
{
    public PlayerState Current { get; set; }

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
