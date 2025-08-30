using UnityEngine;

public class ChangeWP_Player : PlayerState
{
    public ChangeWP_Player(Player p, PlayerFSM fsm) : base(p, fsm) { }

    public override void Enter()
    {
        base.Enter();
        if (player.OneDown)
        {
            GameManager.instance.ChangeWeapon(1);
        }
        else if (player.TwoDown)
        {
            GameManager.instance.ChangeWeapon(2);
        }
        else if (player.ThreeDown)
        {
            GameManager.instance.ChangeWeapon(3);
        }
        player.OneDown = false;
        player.TwoDown = false;
        player.ThreeDown = false;

    }
    public override void Tick()
    {
        if (player.grounded)
        {
            if (Mathf.Abs(player.inputX) > Player.INPUT_EPS) { fsm.ChangeState(player.move); return; }
            else { fsm.ChangeState(player.idle); return; }
        }
        else if (player.jumpCount < player.maxJumps && player.jumpDown)
        { fsm.ChangeState(player.jump); return; }
        else if (player.Ondelaytime())
        {
            fsm.ChangeState(player.idle);
            player.delayTime = 0;
        }
    }
}
