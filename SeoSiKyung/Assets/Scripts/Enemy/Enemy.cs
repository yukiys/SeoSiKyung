using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float speed = 2f;
    public int currentHP;
    public List<string> resistances;

    public Rigidbody2D rd;
    public SpriteRenderer sr;
    public Collider2D cd;
    public Animator anim;
    public Transform player;


    public EnemyFSM fsm { get; set; }
    public EnemySleepState SleepState { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyPatrolState PatrolState { get; set; }
    public EnemyTraceState TraceState { get; set; }

    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();

        fsm = new EnemyFSM();
        SleepState = new EnemySleepState(this, fsm);
        IdleState = new EnemyIdleState(this, fsm);
        PatrolState = new EnemyPatrolState(this, fsm);
        TraceState = new EnemyTraceState(this, fsm);
    }

    void Start()
    {
        EnemyData data = GameManager.instance.GetEnemyData(enemyName);
        if (data != null)
        {
            currentHP = data.maxHP;
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
        fsm.ChangeState(IdleState);
    }

    public void OnHit()
    {
        anim.SetTrigger("OnHit");
    }
}
