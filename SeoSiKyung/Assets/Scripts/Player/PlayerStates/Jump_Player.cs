using Unity.VisualScripting;
using UnityEngine;

public class Jump_Player : PlayerState
{
    public Jump_Player(Player p, PlayerFSM m) : base(p, m) { }

    public override void Enter()
    {
        base.Enter();
        player.DoJump(); // 1단 or 2단
    }

    public override void Tick()
    {
        if (player.attackDown && player.OnCooltime()) { fsm.ChangeState(player.attack); return; }
        // 공중에서 더블 점프
        if (player.jumpCount < player.maxJumps && player.jumpDown)
        {
            player.DoJump();
        }

        // 착지 시 전환
        if (player.grounded)
        {
            if (Mathf.Abs(player.inputX) > Player.INPUT_EPS) { fsm.ChangeState(player.move); return; }
            else { fsm.ChangeState(player.idle); return; }
        }
        if (player.OneDown ||player.TwoDown||player.ThreeDown) { fsm.ChangeState(player.Change); return; }

    }

    public override void FixedTick()
    {        // 공중 제어
        var v = rb.linearVelocity;
        v.x += player.inputX * player.moveSpeed*0.01f;
        if (v.x > player.moveSpeed)
            v.x = player.inputX * player.moveSpeed;
        rb.linearVelocity = v;
    }
}
