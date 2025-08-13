using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player p, PlayerStateMachine m) : base(p, m) { }

    public override void Enter()
    {
        base.Enter();
        player.DoJump(); // 1단 or 2단
    }

    public override void Tick()
    {   
         if (player.attackDown && player.OnCooltime()) { fsm.ChangeState(player.attack); return; }

        // 가변 점프: 키 떼면 상승 감쇠
        if (player.jumpUp && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // 공중에서 더블 점프
        if (player.jumpCount < player.maxJumps && player.jumpDown)
        {
            player.DoJump();
        }

        // 착지 시 전환
        if (player.grounded)
        {
            if (Mathf.Abs(player.inputX) > Player.INPUT_EPS) fsm.ChangeState(player.move);
            else fsm.ChangeState(player.idle);
        }
    }

    public override void FixedTick()
    {
        // 공중 제어
        var v = rb.linearVelocity;
        v.x = player.inputX * player.moveSpeed;
        rb.linearVelocity = v;
    }
}
