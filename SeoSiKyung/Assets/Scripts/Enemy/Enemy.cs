using System.Collections.Generic;
using UnityEngine;
using DataSet;
using Assets.DataSet;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float speed = 2f;
    public int currentHp;
    public List<string> resistances;

    public Rigidbody2D rd;
    public SpriteRenderer sr;
    public Collider2D cd;
    public Animator anim;
    public Transform player;


    public EnemyFSM fsm { get; set; }
    public EnemySleepState SleepState { get; set; }
    public EnemySleepDie1 SleepDie1 { get; set; }
    public EnemySleepDie2 SleepDie2 { get; set; }
    public EnemyAwakeState AwakeState { get; set; }
    public EnemyAwakeDie1 AwakeDie1 { get; set; }
    public EnemyAwakeDie2 AwakeDie2 { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyPatrolState PatrolState { get; set; }
    public EnemyTraceState TraceState { get; set; }
    public EnemyAttackState AttackState { get; set; }

    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();

        fsm = new EnemyFSM();
        SleepState = new EnemySleepState(this, fsm);
        SleepDie1 = new EnemySleepDie1(this, fsm);
        SleepDie2 = new EnemySleepDie2(this, fsm);
        AwakeState = new EnemyAwakeState(this, fsm);
        AwakeDie1 = new EnemyAwakeDie1(this, fsm);
        AwakeDie2 = new EnemyAwakeDie2(this, fsm);
        IdleState = new EnemyIdleState(this, fsm);
        PatrolState = new EnemyPatrolState(this, fsm);
        TraceState = new EnemyTraceState(this, fsm);
        AttackState = new EnemyAttackState(this, fsm);
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

    void Update()
    {
        fsm.LogicUpdate();
    }

    void FixedUpdate()
    {
        fsm.PhysicsUpdate();
    }

    public void WakeUp()
    {
        fsm.ChangeState(AwakeState);
    }

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
        if (IsResisted(type))
        {
            return;
        }

        var currentState = fsm.CurrentState;
        if (currentState == SleepState)
        {
            
        }
        else
        {

        }
    }
}