using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lytre : Boss
{

    /* Health and Basics */
    public float speed = 10;
    private bool moveToPlayer = true;
    public float moveTime = 3f;             // How long to follow the player
    public float moveInterval = 5f;         // The wait between movements
    public float attackDist = 2;
    private SpriteRenderer sprite;
    public float attackRange = 2;

    /* Animator COntrol */
    public Animator animator;

    /* Knock Up */
    public float heavyKnockback = 10f;
    public float heavyKnockup = 8f;
    public float SpecialKnockback = 12f;
    public float specialKnockup = 6f;

    [Header("Boss Colors")]
    public Color scaredColor;
    public Color dashReadyColor;
    public Color corneredColor;

    /* Dashing */
    public float dashingPower = 30f;
    public bool isDashing = false;
    private float dashingTime = 0.2f;
    private bool isFacingRight = true;
    private float horizontal;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Rigidbody2D rb;

    /* Combat State */
    private string state = "CHASE";

    // float moveTimer = 3f;
    bool isMoving;

    public float attackDelay = 3f;
    private float timeSinceAttack = 0f;
    private float scaredTimer = 0f;
    private float chaseTimer = 0f;

    /* Audio */
    public AudioClip[] punchClips;
    public AudioClip slamClip;
    private AudioSource audSource;

    /* Sets the Boss to a state (default state) */
    void Awake()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        state = "CHASE";
        audSource = GetComponent<AudioSource>();
    }

    private void Update() {
        Flip();

        if (state == "CHASE" || state == "SCARED") animator.SetBool("isWalking", true);
        else animator.SetBool("isWalking", false);

        if (state != "DEAD") {

            timeSinceAttack += Time.deltaTime;
            /* Chase State (Boss will chase you and then attack you)*/
            if (state == "CHASE") {
                if (chaseTimer >= 1.5f) { /* Boss dashes */
                    StartCoroutine(Dash(GetPlayerDirection() * -1));
                    if (GetDistanceToPlayer() < attackRange) {
                        HeavyAttack();
                    }
                    chaseTimer = 0f;
                } else {
                    chaseTimer += Time.deltaTime;
                }
                MoveToPlayer();
            }

            /* Attack State */
            if (GetDistanceToPlayer() < attackRange && state == "CHASE")
            {
                state = "ATTACK";
            }

            if (GetDistanceToPlayer() > attackRange * 1.5f && state == "ATTACK" && currentHealth > maxHealth * 0.2f)
            {
                float prob = Random.Range(0f, 1f);
                if (prob <= 0.5) {
                    state = "CHASE";
                } else {
                    state = "SCARED";
                }
            }

            /* Aggressive State (Boss will attack aggressively in this state) */

            /* Attack State (boss will try to attack you a bit) */
            if (state == "ATTACK" && timeSinceAttack > attackDelay) {
                float prob = Random.Range(0f, 1f);
                // Debug.Log("Prob: " + prob);
                if (prob <= 0.3333f) {
                    LightAttack();
                } else if (prob < 0.666f) {
                    HeavyAttack();
                } else {
                    SpecialAttack();
                }
                timeSinceAttack = 0;
            }

            if (state == "SCARED") {
                sprite.color = scaredColor;
                // Debug.Log("SCARED");
                /* After being 'Scared' it will attack with a quick combo */
                if (scaredTimer < 0.8f) {
                    scaredTimer += Time.deltaTime;
                    MoveAwayFromPlayer();
                } else {
                    state = "CHASE";
                    scaredTimer = 0f;
                }
            }
            if (state == "CORNER") {
                sprite.color = corneredColor;
                // Debug.Log("CORNERED");
                if (GetDistanceToPlayer() <= 4) {
                    StartCoroutine(Dash(GetPlayerDirection()));
                    HeavyAttack();
                    state = "CHASE";
                }
            }
        } else {
            /* The boss is dead */
            animator.SetBool("isDead", true);
        }
        
    }

    /* FLips the player's sprite */
    public override void Flip()
    {
        if (isFacingRight && GetPlayerDirection() < 0f || !isFacingRight && GetPlayerDirection() > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    /* Performs a light/Quick attack */
    public override void LightAttack()
    {
        if (GetDistanceToPlayer() < attackDist)
        {
            animator.SetFloat("attackType", 0);
            audSource.PlayOneShot(punchClips[Random.Range(0, punchClips.Length)]);
            animator.SetTrigger("attack");
            StartCoroutine(player.GetComponent<PlatformerMovement>().TakeDamage(true, 0f, 0f));
        }
    }

    /* Performs a heavy attack */
    public override void HeavyAttack()
    {
        if (GetDistanceToPlayer() < attackDist)
        {
            animator.SetFloat("attackType", 1);
            audSource.PlayOneShot(slamClip);
            animator.SetTrigger("attack");
            StartCoroutine(player.GetComponent<PlatformerMovement>().TakeDamage(true, heavyKnockback, heavyKnockup));
        }
    }

    /* Performs a special attack */
    public override void SpecialAttack()
    {
        if (GetDistanceToPlayer() < attackDist)
        {
            animator.SetFloat("attackType", 2);
            audSource.PlayOneShot(punchClips[Random.Range(0, punchClips.Length)]);
            animator.SetTrigger("attack");
            StartCoroutine(player.GetComponent<PlatformerMovement>().TakeDamage(true, SpecialKnockback, specialKnockup));
        }
    }

    /* Gets the distance to the player */
    new public float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.position);
    }

    /* Moves away from the player */
    public override void MoveAwayFromPlayer()
    {
        transform.Translate(new Vector3(GetPlayerDirection() * -1, 0, 0) * speed * Time.deltaTime);
    }

    /* Collisions with objects */
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Wall") {
            state = "CORNER";
        }
    }

    /* Moves towards the player */
    public override void MoveToPlayer()
    {
        // if (!moveToPlayer) return;
        transform.Translate(new Vector3((player.position - transform.position).x, 0, 0) * speed * Time.deltaTime);
        // moveTimer -= Time.deltaTime;

        // if (moveTimer <= 0) {
        //     moveToPlayer = false;
        //     moveTimer = moveInterval;
        // }
    }

    // void MoveToPoint();

    /* Get's the direction of the player */
    int GetPlayerDirection()
    {
        if (player.position.x < transform.position.x) {
            return -1;
        } else if (player.position.x == transform.position.x) {
            return 0;
        } else {
            return 1;
        }
    }
    /* Dashes in a direction */
    public override IEnumerator Dash(float direction)
    {
        // Debug.Log("Dashing!");
        // float originalGravity = rb.gravityScale;
        // rb.gravityScale = 0f;
        // rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        rb.AddForce(new Vector2(dashingPower * transform.localScale.x * (direction * 1.2f), 0));
        // trail.emitting = true;
        sprite.color = dashReadyColor;
        yield return new WaitForSeconds(dashingTime);
        // trail.emitting = false;
        // rb.gravityScale = originalGravity;
    }

    /* Takes Damage from an object */
    public override IEnumerator TakeDamage(float damage)
    {
        // Debug.Log("Boss Takes damage");
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
        currentHealth -= damage;
        if (currentHealth <= 0) {
            state = "DEAD";
        }
    }

}
