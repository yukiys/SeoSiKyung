using System.Collections.Generic;
using UnityEngine;
using Assets.DataSet;

public class Enemy : MonoBehaviour
{
    #region ---- Enemy Data ----
    [Header("Identity")]
    public string enemyName;

    [Header("Stats")]
    public List<string> resistances;
    public int currentHp;
    public float speed;

    [Header("Senses")]
    public float groundCheckDistance;
    public float wallCheckDistance;
    public float detectRange;
    public float attackRange;

    [Header("Ranges")]
    public bool isRanged;
    public List<float> attackArea;

    [Header("animation")]
    public bool idlewalk;
    #endregion

    #region ---- Inspector References ----
    [Header("Checks & Masks")]
    public Transform groundCheck;
    public LayerMask playerMask;
    public LayerMask groundMask;
    public LayerMask wallMask;

    [Header("Corpse Prefabs")]
    public GameObject SlashCorpse;
    public GameObject BludgeonCorpse;
    public GameObject PierceCorpse;
    public GameObject FireCorpse;
    public GameObject IceCorpse;
    #endregion

    #region ---- Runtime State & Components ----
    [HideInInspector] public bool isDying = false;

    [HideInInspector] public Rigidbody2D rd;
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
    public Patrol_Enemy PatrolState { get; set; }
    public Trace_Enemy TraceState { get; set; }
    public Attack_Enemy AttackState { get; set; }
    #endregion

    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();

        fsm = new EnemyFSM();
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
        PatrolState = new Patrol_Enemy(this, fsm);
        TraceState = new Trace_Enemy(this, fsm);
        AttackState = new Attack_Enemy(this, fsm);
    }

    void Start()
    {
        GetEnemyData(enemyName);
        fsm.Initialize(SleepState);
    }

    void Update() => fsm.LogicUpdate();
    void FixedUpdate() => fsm.PhysicsUpdate();

    public void GetEnemyData(string enemyName)
    {
        DataSet.EnemyData data = GameManager.instance.GetEnemyData(enemyName);
        if (data != null)
        {
            resistances = data.resistances;
            currentHp = data.maxHp;
            speed = data.speed;
            groundCheckDistance = data.groundCheckDistance;
            wallCheckDistance = data.wallCheckDistance;
            detectRange = data.detectRange;
            attackRange = data.attackRange;
            isRanged = data.isRanged;
            attackArea = data.attackArea;
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
            fsm.ChangeState(AwakeState);
            return;
        }

        if (fsm.CurrentState == SleepState)
        {
            if (type == AttackType.Slash) fsm.ChangeState(Slash_Sleep);
            else if (type == AttackType.Bludgeon) fsm.ChangeState(Bludgeon_Sleep);
            else if (type == AttackType.Pierce) fsm.ChangeState(Pierce_Sleep);
            else if (type == AttackType.Fire) fsm.ChangeState(Fire_Sleep);
            else if (type == AttackType.Ice) fsm.ChangeState(Ice_Sleep);
            return;
        }
        if (--currentHp <= 0)
        {
            if (type == AttackType.Slash) fsm.ChangeState(Slash);
            else if (type == AttackType.Bludgeon) fsm.ChangeState(Bludgeon);
            else if (type == AttackType.Pierce) fsm.ChangeState(Pierce);
            else if (type == AttackType.Fire) fsm.ChangeState(Fire);
            else if (type == AttackType.Ice) fsm.ChangeState(Ice);
        }
    }

    public bool GroundAhead(int dir)
    {
        if (!groundCheck) return true;

        Vector2 origin = (Vector2)groundCheck.position + new Vector2(dir * groundCheckDistance, 0f);

        Debug.DrawLine(origin, origin + Vector2.down * 0.3f, Color.yellow);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.3f, groundMask);
        return hit.collider != null;
    }

    public bool WallAhead(int dir)
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(dir * 0.2f, -0.2f);
        Vector2 dirVec = new Vector2(dir, 0f);

        Debug.DrawLine(origin, origin + dirVec * wallCheckDistance, Color.red);
        var hit = Physics2D.Raycast(origin, dirVec, wallCheckDistance, wallMask);
        return hit.collider != null;
    }

    public bool InDetectRange()
    {
        return Vector2.SqrMagnitude(player.position - transform.position) <= detectRange * detectRange;
    }

    public bool InAttackRange()
    {
        return Vector2.SqrMagnitude(player.position - transform.position) <= attackRange * attackRange;
    }

    public void EnemyAttack()
    {
        float x = attackArea[0];
        float y = attackArea[1];
        float w = attackArea[2];
        float h = attackArea[3];

        int dir = 1;
        if (sr.flipX == true) dir *= -1;

        Vector2 LeftBottom = new Vector2(x * -dir, y);
        Vector2 center = (Vector2)transform.position + LeftBottom + new Vector2(w * 0.5f, h * 0.5f);
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, new Vector2(w, h), 0f, playerMask);
        DebugDrawAttackBox(center, w, h, Color.red, 0.2f);

        foreach (var hit in hits)
        {
            var p = hit.GetComponent<Player>();
            if (p != null)
            {
                GameManager.instance.HealthDown();
            }
        }
    }

    void DebugDrawAttackBox(Vector2 center, float w, float h, Color c, float duration)
    {
        Vector3 a = new Vector3(center.x - w/2f, center.y - h/2f, 0);
        Vector3 b = new Vector3(center.x + w/2f, center.y - h/2f, 0);
        Vector3 d = new Vector3(center.x - w/2f, center.y + h/2f, 0);
        Vector3 e = new Vector3(center.x + w/2f, center.y + h/2f, 0);
        Debug.DrawLine(a, b, c, duration);
        Debug.DrawLine(b, e, c, duration);
        Debug.DrawLine(e, d, c, duration);
        Debug.DrawLine(d, a, c, duration);
    }
}