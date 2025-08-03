using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rd;
    SpriteRenderer sr;
    Collider2D cd;
    Transform player;
    Animator anim;
    public float speed = 2f;
    private bool isAwake = false;
    private bool isMoving = false;

    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isAwake && isMoving && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rd.linearVelocity = speed * direction;
            sr.flipX = (direction.x < 0);
            anim.SetBool("isMoving", true);
        }
        else
        {
            rd.linearVelocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
    }

    public void OnHit()
    {
        anim.SetBool("isHit", true);
    }

    public void WakeUp()
    {
        isAwake = true;
        isMoving = true;
    }
}
