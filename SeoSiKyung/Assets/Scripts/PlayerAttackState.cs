using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(Player p, PlayerStateMachine m) : base(p, m) { }

    public override void Enter()
    {
        base.Enter();
        player.SetRun(false);
    }
    public override void Tick()
    {
        if (Mathf.Abs(player.inputX) > Player.INPUT_EPS)
        { fsm.ChangeState(player.move); return; }

        if (player.jumpCount < player.maxJumps && player.jumpDown)
        { fsm.ChangeState(player.jump); }
    }
}
