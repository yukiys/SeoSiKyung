using System.Collections.Generic;
using UnityEngine;
using Assets.DataSet;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float speed = 2f;
    public int currentHp;
    public List<string> resistances;
    public bool isDying = false;
    public Transform groundCheck;
    public LayerMask groundMask;
    public LayerMask wallMask;
    public float groundCheckDistance = 3f;
    public float wallCheckDistance = 3f;
    public float detectRange = 9f;
    public float attackRange = 6f;
    public GameObject PierceCorpse;

    [HideInInspector] public Rigidbody2D rd;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Collider2D cd;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Transform player;
    [HideInInspector] public PlatformTrigger platform;


    public EnemyFSM fsm { get; set; }
    public Sleep_Enemy SleepState { get; set; }
    public Pierce_SleepEnemy Pierce_Sleep { get; set; }
    public Bludgeon_SleepEnemy Bludgeon_Sleep { get; set; }
    public Awake_Enemy AwakeState { get; set; }
    public Bludgeon_Enemy Bludgeon { get; set; }
    public Pierce_Enemy Pierce { get; set; }
    public Idle_Enemy IdleState { get; set; }
    public Patrol_Enemy PatrolState { get; set; }
    public Trace_Enemy TraceState { get; set; }
    public Attack_Enemy AttackState { get; set; }

    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();

        fsm = new EnemyFSM();
        SleepState = new Sleep_Enemy(this, fsm);
        Pierce_Sleep = new Pierce_SleepEnemy(this, fsm);
        Bludgeon_Sleep = new Bludgeon_SleepEnemy(this, fsm);
        AwakeState = new Awake_Enemy(this, fsm);
        Bludgeon = new Bludgeon_Enemy(this, fsm);
        Pierce = new Pierce_Enemy(this, fsm);
        IdleState = new Idle_Enemy(this, fsm);
        PatrolState = new Patrol_Enemy(this, fsm);
        TraceState = new Trace_Enemy(this, fsm);
        AttackState = new Attack_Enemy(this, fsm);
    }

    void Start()
    {
        DataSet.EnemyData data = GameManager.instance.GetEnemyData(enemyName);
        if (data != null)
        {
            currentHp = data.maxHp;
            resistances = data.resistances;
        }

        fsm.Initialize(SleepState);
    }

    void Update() => fsm.LogicUpdate();
    void FixedUpdate() => fsm.PhysicsUpdate();


    private bool IsResisted(AttackType type)
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
            // if (type == AttackType.Slash) fsm.ChangeState(Slash_Sleep);
            if (type == AttackType.Bludgeon) fsm.ChangeState(Bludgeon_Sleep);
            else if (type == AttackType.Pierce) fsm.ChangeState(Pierce_Sleep);
            // else if (type == AttackType.Fire) fsm.ChangeState(Fire_Sleep);
            // else if (type == AttackType.Ice) fsm.ChangeState(Ice_Sleep);
            return;
        }
        if(--currentHp<=0)
        {
            // if (type == AttackType.Slash) fsm.ChangeState(Slash);
            if (type == AttackType.Bludgeon) fsm.ChangeState(Bludgeon);
            else if (type == AttackType.Pierce) fsm.ChangeState(Pierce);
            // else if (type == AttackType.Fire) fsm.ChangeState(Fire);
            // else if (type == AttackType.Ice) fsm.ChangeState(Ice);
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
}