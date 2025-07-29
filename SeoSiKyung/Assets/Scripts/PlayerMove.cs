using UnityEngine;

public class PlayerMove : MonoBehaviour
{   
    public float Speed;
    public Rigidbody2D rd;
    public SpriteRenderer sr;
    public Collider2D cd;
    public Vector2 moveVelocity;
    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * Speed;
    }
    void FixedUpdate()
    {
        rd.MovePosition(rd.position + moveVelocity * Time.fixedDeltaTime);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

}
