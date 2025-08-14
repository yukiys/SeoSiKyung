using UnityEngine;

public class Move_Player : PlayerState
{
    public Move_Player(Player p, PlayerFSM m) : base(p, m) { }

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
        if (player.attackDown && player.OnCooltime()) { fsm.ChangeState(player.attack); return; }

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
