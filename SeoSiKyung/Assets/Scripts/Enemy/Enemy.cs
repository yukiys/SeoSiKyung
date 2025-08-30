using System.Collections.Generic;
using UnityEngine;
using Assets.DataSet;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    #region ---- Enemy Data ----
    [Header("Identity")]
    public string enemyName;

    [Header("Stats")]
    public List<string> resistances;
    public int maxHp;
    public int Hp;
    [HideInInspector] public float speed;
    public float jumpPower;
    [HideInInspector] public float jumpCoolDown = 0.35f;
    [HideInInspector] private float lastJumpTime;

    [Header("Senses")]
    public float groundCheckDistance;
    public float headCheckDistance = 0.2f;
    public float wallCheckDistance;
    public float detectRange;
    public float attackRange;

    [Header("Ranges")]
    [HideInInspector] public bool isRanged;
    public List<float> attackArea;

    [Header("animation")]
    [HideInInspector] public bool idlewalk;

    [Header("Pattern")]
    [HideInInspector] public string pattern;
    #endregion

    #region ---- Inspector References ----
    [Header("Checks & Masks")]
    public Transform groundCheck;
    public LayerMask playerMask;
    public LayerMask groundMask;
    public LayerMask wallMask;
    private ContactFilter2D groundFilter;
    private ContactFilter2D wallFilter;
    private RaycastHit2D[] castHits = new RaycastHit2D[4];
    private const float skin = 0.06f;

    [Header("Corpse Prefabs")]
    public GameObject SlashCorpse;
    public GameObject BludgeonCorpse;
    public GameObject PierceCorpse;
    public GameObject FireCorpse;
    public GameObject IceCorpse;
    #endregion

    #region ---- Runtime State & Components ----
    [HideInInspector] public Vector2 spawnPos;
    [HideInInspector] public bool isDying = false;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Collider2D cd;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Transform player;
    #endregion

    #region ---- FSM & Enemy States ----
    [Header("FSM")]
    public EnemyFSM fsm { get; set; }

    [Header("SleepStates")]
    public Sleep_Enemy SleepState { get; set; }
    public Slash_SleepEnemy Slash_Sleep { get; set; }
    public Pierce_SleepEnemy Pierce_Sleep { get; set; }
    public Bludgeon_SleepEnemy Bludgeon_Sleep { get; set; }
    public Fire_SleepEnemy Fire_Sleep { get; set; }
    public Ice_SleepEnemy Ice_Sleep { get; set; }

    [Header("AwakeStates")]
    public Awake_Enemy AwakeState { get; set; }
    public Slash_Enemy Slash { get; set; }
    public Bludgeon_Enemy Bludgeon { get; set; }
    public Pierce_Enemy Pierce { get; set; }
    public Fire_Enemy Fire { get; set; }
    public Ice_Enemy Ice { get; set; }

    [Header("CommonStates")]
    public Idle_Enemy IdleState { get; set; }
    public Trace_Enemy TraceState { get; set; }
    public Attack_Enemy AttackState { get; set; }
    public Patrol_Enemy PatrolState { get; set; }
    public Chase_Enemy ChaseState { get; set; }
    public Return_enemy ReturnState { get; set; }
    public GoHome_Enemy GoHomeState { get; set; }
    #endregion

    void Awake()
    {
        spawnPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();

        fsm = new EnemyFSM();
        GetEnemyStates(fsm);

        GetEnemyData(enemyName);

        groundFilter = new ContactFilter2D { useLayerMask = true, useTriggers = false };
        groundFilter.SetLayerMask(groundMask);

        wallFilter = new ContactFilter2D { useLayerMask = true, useTriggers = false };
        wallFilter.SetLayerMask(wallMask);
    }

    void Start() => fsm.Initialize(SleepState);

    void Update() => fsm.LogicUpdate();
    void FixedUpdate() => fsm.PhysicsUpdate();

    public EnemyState GetEnemyPattern()
    {
        if (pattern == "Patrol") return PatrolState;
        else if (pattern == "Chase") return ChaseState;
        else if (pattern == "Return") return ReturnState;

        return null;
    }

    void GetEnemyStates(EnemyFSM fsm)
    {
        SleepState = new Sleep_Enemy(this, fsm);
        Slash_Sleep = new Slash_SleepEnemy(this, fsm);
        Pierce_Sleep = new Pierce_SleepEnemy(this, fsm);
        Bludgeon_Sleep = new Bludgeon_SleepEnemy(this, fsm);
        Fire_Sleep = new Fire_SleepEnemy(this, fsm);
        Ice_Sleep = new Ice_SleepEnemy(this, fsm);

        AwakeState = new Awake_Enemy(this, fsm);
        Slash = new Slash_Enemy(this, fsm);
        Bludgeon = new Bludgeon_Enemy(this, fsm);
        Pierce = new Pierce_Enemy(this, fsm);
        Fire = new Fire_Enemy(this, fsm);
        Ice = new Ice_Enemy(this, fsm);

        IdleState = new Idle_Enemy(this, fsm);
        TraceState = new Trace_Enemy(this, fsm);
        AttackState = new Attack_Enemy(this, fsm);
        PatrolState = new Patrol_Enemy(this, fsm);
        ChaseState = new Chase_Enemy(this, fsm);
        ReturnState = new Return_enemy(this, fsm);
        GoHomeState = new GoHome_Enemy(this, fsm);
    }

    void GetEnemyData(string enemyName)
    {
        DataSet.EnemyData data = GameManager.instance.GetEnemyData(enemyName);
        if (data != null)
        {
            resistances = data.resistances;
            maxHp = data.maxHp;
            Hp = maxHp;
            speed = data.speed;
            jumpPower = data.jumpPower;

            groundCheckDistance = data.groundCheckDistance;
            wallCheckDistance = data.wallCheckDistance;
            detectRange = data.detectRange;
            attackRange = data.attackRange;

            isRanged = data.isRanged;
            attackArea = data.attackArea;

            idlewalk = data.idlewalk;
            pattern = data.pattern;
        }
    }

    public bool IsResisted(AttackType type)
    {
        string key = type.ToString();

        for (int i = 0; i < resistances.Count; i++)
            if (key == resistances[i])
                return true;
        return false;
    }

    public void OnHit(AttackType type)
    {
        if (isDying) return;
        if (IsResisted(type))
        {
            if (fsm.CurrentState == SleepState) fsm.ChangeState(AwakeState);
            return;
        }

        if (fsm.CurrentState == SleepState)
        {
            Hp = 0;

            if (type == AttackType.Slash) fsm.ChangeState(Slash_Sleep);
            else if (type == AttackType.Bludgeon) fsm.ChangeState(Bludgeon_Sleep);
            else if (type == AttackType.Pierce) fsm.ChangeState(Pierce_Sleep);
            else if (type == AttackType.Fire) fsm.ChangeState(Fire_Sleep);
            else if (type == AttackType.Ice) fsm.ChangeState(Ice_Sleep);
            return;
        }
        if (--Hp <= 0)
        {
            if (type == AttackType.Slash) fsm.ChangeState(Slash);
            else if (type == AttackType.Bludgeon) fsm.ChangeState(Bludgeon);
            else if (type == AttackType.Pierce) fsm.ChangeState(Pierce);
            else if (type == AttackType.Fire) fsm.ChangeState(Fire);
            else if (type == AttackType.Ice) fsm.ChangeState(Ice);
        }
    }

    public bool IsGrounded()
    {
        int count = rb.Cast(Vector2.down, groundFilter, castHits, skin);
        for (int i = 0; i < count; i++)
            if (castHits[i].normal.y > 0.5f) return true;

        return false;
    }
    public bool IsHeadClear()
    {
        int count = rb.Cast(Vector2.up, groundFilter, castHits, headCheckDistance);
        return count == 0;
    }
    public bool CanJump() => (Time.time - lastJumpTime) >= jumpCoolDown && IsGrounded() && IsHeadClear() && pattern == "Chase";
    public bool RecentlyJumped() => (Time.time - lastJumpTime) <= 0.15f;

    public void Jump(int dir)
    {
        lastJumpTime = Time.time;
        rb.linearVelocity = new Vector2(2 * dir * speed, jumpPower);
    }

    public bool GroundAhead(int dir)
    {
        Vector2 origin = (Vector2)groundCheck.position + new Vector2(dir * groundCheckDistance, 0f);
        Vector2 size = new Vector2(cd.bounds.size.x * 0.6f, 0.18f);

        var hit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, 0.3f, groundMask);
        return hit.collider != null;
    }

    public bool WallAhead(int dir)
    {
        int count = rb.Cast(new Vector2(dir, 0f), wallFilter, castHits, skin);
        return count > 0;
    }

    public bool InDetectRange() => Vector2.SqrMagnitude(player.position - transform.position) <= detectRange * detectRange;

    public bool InAttackRange() => Vector2.SqrMagnitude(player.position - transform.position) <= attackRange * attackRange;

    public void EnemyAttack()
    {
        float x = attackArea[0];
        float y = attackArea[1];
        float w = attackArea[2];
        float h = attackArea[3];

        int dir = sr.flipX ? -1 : 1;

        Vector2 leftBottom = new Vector2(x * -dir, y);
        Vector2 center = (Vector2)transform.position + leftBottom + new Vector2(w * 0.5f, h * 0.5f);

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, new Vector2(w, h), 0f, playerMask);
        foreach (var hit in hits)
        {
            var p = hit.GetComponent<Player>();
            if (p != null) GameManager.instance.HealthDown();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (!groundCheck || !cd) return;
        Vector2 size = new Vector2(cd.bounds.size.x * 0.6f, 0.18f);
        Color c = Color.yellow;
        c.a = 0.3f;
        Gizmos.color = c;
        for (int dir = -1; dir <= 1; dir += 2)
        {
            Vector2 origin = (Vector2)groundCheck.position + new Vector2(dir * groundCheckDistance, 0f);
            Gizmos.DrawWireCube(origin + Vector2.down * 0.15f, size);
        }
    }
}