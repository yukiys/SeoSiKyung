using UnityEngine;

public abstract class PlayerState
{
    protected readonly Player player;
    protected readonly PlayerFSM fsm;
    protected readonly Rigidbody2D rb;

    protected float stateTimer;
    protected bool animTrigger;

    protected PlayerState(Player p, PlayerFSM m)
    { player = p; fsm = m; rb = p.rb; }

    public virtual void Enter() { animTrigger = false; }
    public abstract void Tick();                 // 매 프레임 로직
    public virtual void FixedTick() { }          // 물리(속도/힘)는 여기서
    public virtual void Exit() { }

    public void StartTimer(float t) => stateTimer = t;
    public bool TimerUp => stateTimer <= 0f;
    public void AnimationTrigger() => animTrigger = true;
}
