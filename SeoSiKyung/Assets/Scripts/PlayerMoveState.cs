using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player p, PlayerStateMachine m) : base(p, m) { }

    public override void Enter()
    {
        base.Enter();
        player.SetRun(true);
    }

    public override void Tick()
    {
        if (Mathf.Abs(player.inputX) < Player.INPUT_EPS &&
            Mathf.Abs(rb.linearVelocity.x) < Player.SPEED_EPS)
        { fsm.ChangeState(player.idle); return; }

        if (player.jumpCount < player.maxJumps && player.jumpDown)
        { fsm.ChangeState(player.jump); }
    }

    public override void FixedTick()
    {
        var v = rb.linearVelocity;
        v.x = player.inputX * player.moveSpeed;
        rb.linearVelocity = v;
    }

    public override void Exit()
    {
        player.SetRun(false);
    }
}
