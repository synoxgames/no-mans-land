using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffController : MonoBehaviour
{

    public GameObject player; /* aim at the player */
    public GameObject boss;
    public Transform staffSprite;
    public float speed = 7;
    public float spin = 800;
    private Transform startPos;
    private Vector3 targetPos;
    private bool reachedTarget = false;
    private bool hasHitPlayer = false;
    public Rigidbody2D rb;
    [Header("Stats")]
    public float distanceBehindPlayer = 2;
    public bool toxic = false;
    // bool dir = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startPos = GameObject.FindGameObjectWithTag("Boss").transform;
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos.position;
        if (Random.Range(0.0f, 1.0f) > 0.666f)
        {
            distanceBehindPlayer *= 1.5f;
        }
        targetPos.x = player.transform.position.x + (GetDirectionX(player.transform.position) * distanceBehindPlayer);

    }

    public int GetDirectionX(Vector3 t)
    {
        if (transform.position.x < t.x)
        {
            return 1;
        }
        else if (transform.position.x > t.x)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public float GetDistanceX(Vector3 a, Vector3 b)
    {
        /* Making sure it is positive :3 */
        return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2));
    }

    public float GetDistanceY(Vector3 a, Vector3 b)
    {
        return (a.y - b.y);
    }

    public int GetDirectionY(Vector3 t)
    {
        if (transform.position.y < t.y)
        {
            return 1;
        }
        else if (transform.position.y > t.y)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public void MoveToTarget()
    {
        transform.Translate(new Vector3(GetDirectionX(targetPos) * speed * Time.deltaTime, 0f, 0f));
    }

    public void ReturnToBoss()
    {
        transform.Translate(new Vector3(GetDirectionX(startPos.position) * speed * Time.deltaTime, 0f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleController.instance.killedPlayer) return;
        if (reachedTarget)
        {
            ReturnToBoss();
            if (GetDistanceX(startPos.position, transform.position) <= 0.3f)
            {
                Destroy(gameObject);
            }
        } else
        {
            MoveToTarget();
            if (GetDistanceX(targetPos, transform.position) <= 0.3f)
            {
                reachedTarget = true;
            }
        }

        staffSprite.Rotate(-transform.forward * spin * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" )
        {
            if (!hasHitPlayer)
            {
                /* StartCoroutine(GameObject.FindGameObjectWithTag("Boss").GetComponent<Balthazar>().TeleportAttack(transform.position, GetDirectionX(player.transform.position) * -1));*/
                StartCoroutine(player.GetComponent<PlatformerMovement>().TakeDamage(true, 8 * GetDirectionX(player.transform.position), 0f));
                hasHitPlayer = true;
            } else
            {
                if (toxic)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}
