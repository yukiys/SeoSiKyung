using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Rigidbody2D rd;
    public SpriteRenderer sr;
    public Collider2D cd;
    public Transform player;
    public float speed = 2f;
    private bool isAwake = false;
    private bool isMoving = false;

    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (isAwake && isMoving && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rd.linearVelocity = speed * direction;

            sr.flipX = (direction.x < 0);
        }
        else rd.linearVelocity = Vector2.zero;
    }

    public void WakeUp()
    {
        
    }
}
