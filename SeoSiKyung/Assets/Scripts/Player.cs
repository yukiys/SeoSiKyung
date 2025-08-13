using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [Header("Move / Jump")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public int   maxJumps  = 2;
    public float fireForce = 10f;

    [Header("Ground Check")]
    public LayerMask groundMask;
    [Range(0.01f, 0.2f)] public float groundProbeHeight = 0.06f;
    [Range(0.5f, 1f)]   public float groundProbeWidthRatio = 0.9f;

    [Header("Optional")]
    public Animator animator;
    public SpriteRenderer sr;
    public GameObject fbobject;

    // 데드존
    public const float INPUT_EPS = 0.05f;
    public const float SPEED_EPS = 0.05f;

    // 컴포넌트
    public Rigidbody2D rb { get; private set; }
    Collider2D col;

    // 입력/상태 공유
    [HideInInspector] public float inputX;
    [HideInInspector] public bool  jumpDown, jumpUp;
    [HideInInspector] public bool  grounded;
    [HideInInspector] public int   jumpCount;

    // FSM
    public PlayerStateMachine fsm { get; private set; }
    public PlayerIdleState  idle;
    public PlayerMoveState  move;
    public PlayerJumpState  jump;
    public PlayerAttackState attack;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        if (!sr) sr = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        fsm = new PlayerStateMachine();
        idle = new PlayerIdleState(this, fsm);
        move = new PlayerMoveState(this, fsm);
        jump = new PlayerJumpState(this, fsm);
        attack = new PlayerAttackState(this, fsm);
    }

    void OnEnable() => fsm.ChangeState(idle);

    void Update()
    {
        inputX   = Input.GetAxisRaw("Horizontal");
        jumpDown = Input.GetKeyDown(KeyCode.Space);
        jumpUp   = Input.GetKeyUp(KeyCode.Space);

        if (sr!=null && Mathf.Abs(inputX) > 0.001f) sr.flipX = inputX < 0;

        fsm.Tick();
        jumpDown = false; // 일회성 입력 리셋
    }

    void FixedUpdate()
    {
        fsm.FixedTick();
    }

     void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = false;
        }
    }
    public void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        if (!grounded) jumpCount = maxJumps;
        else jumpCount++;
    }
    public void Shoot()
    {
        GameObject fireball = Instantiate(fbobject, transform.position, transform.rotation);
        Rigidbody2D rd = fireball.GetComponent<Rigidbody2D>();
        Vector2 dir = (sr != null && sr.flipX) ? Vector2.left : Vector2.right;
        rb.AddForce(dir * fireForce, ForceMode2D.Impulse); 
    }
    public void SetRun(bool on)
    {
        if(animator!=null)animator.SetBool("IsRunning", on);
    }
}
