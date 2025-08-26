using System.Collections.Generic;
using DataSet;
using Unity.VisualScripting;
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
    [Header("Object")]
    public GameObject fbobject;
    [Header("Cooldown")]
    public float fbCooltime;
    public Vector2 boxCastSize = new Vector2(0.4f,0.05f);
    public float boxCastMaxDistance = 0.7f;
    float Curtime;
    // 데드존
    public const float INPUT_EPS = 0.05f;
    public const float SPEED_EPS = 0.05f;

    // 컴포넌트
    public Rigidbody2D rb { get; private set; }
    Collider2D col;

    // 숨기기
    [HideInInspector] public float inputX;
    [HideInInspector] public bool attackDown;
    [HideInInspector] public bool  jumpDown, jumpUp;
    [HideInInspector] public bool  grounded;
                     public int   jumpCount;
    [HideInInspector] public List<WeaponData> selectedWeapon;

    // FSM
    public PlayerFSM fsm { get; private set; }
    public Idle_Player  idle;
    public Move_Player  move;
    public Jump_Player  jump;
    public Attack_Player attack;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        if (!sr) sr = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        fsm = new PlayerFSM();
        idle = new Idle_Player(this, fsm);
        move = new Move_Player(this, fsm);
        jump = new Jump_Player(this, fsm);
        attack = new Attack_Player(this, fsm);
    }

    void OnEnable() => fsm.ChangeState(idle);

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        jumpDown = Input.GetKeyDown(KeyCode.Space);
        attackDown = Input.GetKeyDown(KeyCode.LeftControl);
        if (sr != null && Mathf.Abs(inputX) > 0.001f) sr.flipX = inputX < 0;
        timereading();
        checkgrounded();
        fsm.Tick();
        jumpDown = false;
        attackDown = false; // 일회성 입력 리셋
    }

    
    void FixedUpdate()
    {
        fsm.FixedTick();
    }
    public void checkgrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position,boxCastSize, 0f, Vector2.down, boxCastMaxDistance, LayerMask.GetMask("Ground"));
        if (raycastHit.collider != null)
        {
            grounded = true;
            jumpCount = 0;
        }
        else
            grounded = false;
    }
    void OnDrawGizmos()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, boxCastSize, 0f, Vector2.down, boxCastMaxDistance, LayerMask.GetMask("Ground"));

        Gizmos.color = Color.red;
        if (raycastHit.collider != null)
        {
            Gizmos.DrawRay(transform.position, Vector2.down * raycastHit.distance);
            Gizmos.DrawWireCube(transform.position + Vector3.down * raycastHit.distance, boxCastSize);
        }
        else
        {
            Gizmos.DrawRay(transform.position, Vector2.down * boxCastMaxDistance);
        }
    }
    public void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpCount++;
    }
    public void timereading()
    {
        Curtime += Time.deltaTime;
    }
    public bool OnCooltime()
    {
        if (Curtime < fbCooltime) return false;
        else return true;
    }
    public void Shoot(GameObject obj)
    {
        if (!OnCooltime()) return;
        Vector2 dir = (sr != null && sr.flipX) ? Vector2.left : Vector2.right;
        Quaternion rotation = Quaternion.Euler(0, 0, -90);
        if (dir == Vector2.left) rotation = Quaternion.Euler(0, 0, 90); 
        GameObject proj = Instantiate(obj, transform.position,rotation);
        Rigidbody2D rd = proj.GetComponent<Rigidbody2D>();
        rd.AddForce(dir * fireForce, ForceMode2D.Impulse);
        Curtime = 0;
    }
    public void SetRun(bool on)
    {
        if(animator!=null)animator.SetBool("IsRunning", on);
    }
}
