using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Balthazar : Boss
{

    [Header("External References")]
    public Animator animator;
    public GameObject projectile1;
    public GameObject staff;
    public Transform airTarget;
    public Transform airTarget2;
    private Transform groundTarget;
    public Transform groundTarget1;
    public Transform groundTarget2;
    public Sprite defaultSprite;
    public Sprite tired_sprite;

    [Header("Audio")]
    public AudioClip projectileSound;
    public AudioClip throwSound;


    /* Health and Basics */
    [Header("Boss Stats")]
    public float speed = 10;
    private bool moveToPlayer = true;
    public float moveTime = 3f;             // How long to follow the player
    public float moveInterval = 5f;         // The wait between movements
    public float attackDist = 2;
    private SpriteRenderer sprite;
    public float attackRange = 2;

    /* Animator Control */
    

    /* Knock Up */
    [Header("Attacks")]
    private float timeSinceAttack = 0f;
    public float flyingAttackSpeed = 1f;

    /* Dashing */
    [Header("Dashing")]
    public float dashingPower = 30f;
    public bool isDashing = false;
    private float dashingTime = 0.2f;
    private bool isFacingRight = false;
    private float horizontal;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Rigidbody2D rb;

    /* Combat State */
    [Header("State Control")]
    private bool pickedGround = false;
    public float maxFlyingTime = 6f;
    public string state = "START";
    private bool touchedGround = false;
    private float currentFlyingTime = 0f;
    public float maxTiredTime = 5f;
    private float currentTiredTime = 0f;
    private int tossCount = 0;
    public int maxStaffTosses = 3;
    private float tossTimer = 0f;
    public float maxTossTime = 3;

    private bool hasRaisedArms = false;
    private AudioSource audSource;


    // [Header("")]

    // float moveTimer = 3f;
    private bool isMoving;

    /* Sets the Boss to a state (default state) */
    void Awake()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        audSource = GetComponent<AudioSource>();
    }

    private void Update() {
        Flip();
        if (state != "DEAD") {

            timeSinceAttack += Time.deltaTime;

            if (state == "FLYING") {
                FlyingState();
            } else if (state == "GROUND") {
                GroundState();
            } else if (state == "TIRED") {
                animator.SetBool("isTired", true);
                TiredState();
            } else if (state != "TIRED") {
                animator.SetBool("isTired", false);
            }

            } else {
            /* The boss is dead */
            animator.SetBool("isDead", true);
        }
        Flip();
    }

    private void GroundState()
    {
        /* Pick which ground target to go to */
        if (!pickedGround)
        {
            if (Random.Range(0.0f, 1.0f) >= 0.5f)
            {
                groundTarget = groundTarget1;
            }
            else
            {
                groundTarget = groundTarget2;
            }
            pickedGround = true;
        }

        if (!touchedGround) {
            if ( transform.position != groundTarget.position ) {

                Vector3 direction = groundTarget.position - transform.position;
                transform.position = Vector3.MoveTowards(transform.position, groundTarget.position, speed * Time.deltaTime);
                if (transform.position == groundTarget.position) {
                    touchedGround = true;
                }
            }
        } else {
            /* Fighting on the ground */
            // Debug.Log("Attacking on the ground");
            if (GameObject.FindGameObjectWithTag("Staff") == null && tossTimer > maxTossTime)
            {
                Instantiate(staff);
                animator.SetTrigger("hasThrown");
                audSource.PlayOneShot(throwSound);
                tossCount++;
                if (Random.Range(0.0f, 1.0f) < 0.25)
                {
                    GameObject.FindGameObjectWithTag("Staff").GetComponent<StaffController>().toxic = true;
                }
                tossTimer = 0f;
            } else
            {
                tossTimer += Time.deltaTime;
            }
            if (tossCount >= maxStaffTosses)
            {
                state = "TIRED";
                pickedGround = false;
                tossCount = 0;
                tossTimer = 0f;
            }
        }
    }

    private void TeleportToTransform(Transform t)
    {
        transform.position = t.position;
    }

    /* Dealing with the flying state */
    private void FlyingState()
    {
        /* Check if we should be on the ground instead */
        if ( transform.position != airTarget.position ) {/*Check how far away from the air target we are */

            Vector3 direction = airTarget.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, airTarget.position, speed * Time.deltaTime);

        } else
        {
            animator.SetBool("inSky", true);

            if (timeSinceAttack > flyingAttackSpeed)
            {
                Instantiate(projectile1);
                audSource.PlayOneShot(projectileSound);
                timeSinceAttack = 0f;
            }
            
        }
        timeSinceAttack += Time.deltaTime;
        currentFlyingTime += Time.deltaTime;
        if (currentFlyingTime > maxFlyingTime)
        {
            state = "GROUND";
            animator.SetBool("inSky", false);
            touchedGround = false;
            currentFlyingTime = 0f;
        }
    }

    private void TiredState()
    {
        
        if (currentTiredTime >= maxTiredTime)
        {
            sprite.sprite = defaultSprite;
            state = "FLYING";
            currentTiredTime = 0f;
        } else
        {
            sprite.sprite = tired_sprite;
        }
        currentTiredTime += Time.deltaTime;
    }

    /* Flips the player's sprite */
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

    /* Fires a Projectile at the player */
    public override void LightAttack()
    {
        
    }

    public override void HeavyAttack()
    {

    }

    /* Performs a special attack */
    public override void SpecialAttack()
    {
        /* Large AOE attack */
    }

    /*public IEnumerator TeleportAttack(Vector3 pos, int direction)
    {
        transform.position = pos;
        player.GetComponent<PlatformerMovement>().TakeDamage(false, 80 * direction, 0f);
        yield return new WaitForSeconds(0.1f);
        transform.position = groundTarget.position;
    }*/

    /* Gets the distance to the player */
    public float GetDistanceToPlayer()
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
        } else if (other.gameObject.tag == "Player" && state != "TIRED")
        {
            if (groundTarget == groundTarget1)
            {
                TeleportToTransform(groundTarget2);
                groundTarget = groundTarget2;
            }
            else
            {
                TeleportToTransform(groundTarget1);
                groundTarget = groundTarget1;
            }
            touchedGround = true;
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
        rb.velocity = new Vector2(transform.localScale.x * dashingPower * 0.5f, 0f);
        // rb.AddForce(new Vector2(dashingPower * transform.localScale.x * (direction * 1.2f), 0));
        // trail.emitting = true;
       //  sprite.color = dashReadyColor;
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
