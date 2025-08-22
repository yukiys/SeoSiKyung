using UnityEngine;

public class Attack_Player : PlayerState
{
    public Attack_Player(Player p, PlayerFSM fsm) : base(p, fsm) { }

    public override void Enter()
    {
        base.Enter();
        player.Shoot(player.fbobject);

    }
    public override void Tick()
    {
        if (player.grounded)
        {
            if (Mathf.Abs(player.inputX) > Player.INPUT_EPS) fsm.ChangeState(player.move);
            else fsm.ChangeState(player.idle);
        }
        else if (player.jumpCount < player.maxJumps && player.jumpDown)
        { fsm.ChangeState(player.jump); }
    }
}
