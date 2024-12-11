using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;
    public float attackSpeed = 0.25f;

    private bool isFacingRight = true;
    private SpriteRenderer sprite;

    public int lives = 3;

    public float attackDamage = 10;
    public float attackRange = 2;

    public Transform boss;
    Animator anim;

    float x;
    float nextHit = 0.0f;
    /* These variable are used in this script only */


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        if (BattleController.instance.killedPlayer) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        anim.SetFloat("X", x);
        anim.SetBool("isWalking", horizontal != 0);

        if (Input.GetButton("Horizontal")) {
            x = horizontal;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        /* Player Light Attack */
        if (Input.GetMouseButtonDown(0))
        {
            if (boss.GetComponent<Boss>().GetDistanceToPlayer() < attackRange && Time.time > nextHit)
            {
                StartCoroutine(boss.GetComponent<Boss>().TakeDamage(attackDamage));
                nextHit = Time.time + attackSpeed;
            }
        } else if (Input.GetMouseButtonDown(1)) /* Heavy Attack */
        {
            if (boss.GetComponent<Boss>().GetDistanceToPlayer() < attackRange)
            {
                StartCoroutine(boss.GetComponent<Boss>().TakeDamage(attackDamage));
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 movePos = new Vector2(horizontal * speed, rb.velocity.y);
        rb.velocity = movePos;
    }

    /*Checks if the player is touching ground or not */
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    /* Lets the player take damage */
    public IEnumerator TakeDamage(bool takeDamage, float knockback, float knockup)
    {
        Debug.Log("Lives: " + lives);
        if (knockback != 0.0f && knockup != 0.0f) {
            // rb.velocity = new Vector2(knockback, knockup);
            rb.AddForce(new Vector2(knockback, knockup), ForceMode2D.Impulse);
        }

        if (takeDamage) lives -= 1;

        // sprite.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1f));
        // sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        // sprite.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time * 0.3f, 1f));
        // sprite.color = Color.white;
        
    }

}
