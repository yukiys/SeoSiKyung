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
    public bool isDying = false;

    public Rigidbody2D rd;
    public SpriteRenderer sr;
    public Collider2D cd;
    public Animator anim;
    public Transform player;


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
        if (isDying) return;
        if (IsResisted(type)) return;
        if (--currentHp > 0) return;

        var currentState = fsm.CurrentState;
        if (currentState == SleepState)
        {
            // if (type == AttackType.Slash) fsm.ChangeState(Slash_Sleep);
            if (type == AttackType.Bludgeon) fsm.ChangeState(Bludgeon_Sleep);
            else if (type == AttackType.Pierce) fsm.ChangeState(Pierce_Sleep);
            // else if (type == AttackType.Fire) fsm.ChangeState(Fire_Sleep);
            // else if (type == AttackType.Ice) fsm.ChangeState(Ice_Sleep);
        }
        else
        {
            // if (type == AttackType.Slash) fsm.ChangeState(Slash);
            if (type == AttackType.Bludgeon) fsm.ChangeState(Bludgeon);
            else if (type == AttackType.Pierce) fsm.ChangeState(Pierce);
            // else if (type == AttackType.Fire) fsm.ChangeState(Fire);
            // else if (type == AttackType.Ice) fsm.ChangeState(Ice);
        }
    }
}