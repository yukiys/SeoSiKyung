using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(Player p, PlayerStateMachine m) : base(p, m) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Tick()
    {
        player.Shoot();
        if (player.grounded)
        {
            if (Mathf.Abs(player.inputX) > Player.INPUT_EPS) fsm.ChangeState(player.move);
            else fsm.ChangeState(player.idle);
        }
        if (player.jumpCount < player.maxJumps && player.jumpDown)
        { fsm.ChangeState(player.jump); }
    }
}
