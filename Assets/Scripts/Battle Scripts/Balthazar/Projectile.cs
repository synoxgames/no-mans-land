using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [Header("Projectile Stats")]
    public float speed = 5f;
    public float knockBack = 5f;
    public float knockUp = 5f;

    private GameObject player;
    private Rigidbody2D rb;
    private Transform startPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startPos = GameObject.FindGameObjectWithTag("Boss").transform;
        rb = player.GetComponent<Rigidbody2D>();

        transform.position = startPos.position;

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.localScale.x * speed * Time.deltaTime * -1, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == player.transform.tag)
        {
            Debug.Log("Taking Damage");
            StartCoroutine(player.GetComponent<PlatformerMovement>().TakeDamage(true, knockBack, knockUp));
            // player.GetComponent<PlatformerMovement>().TakeDamage(knockBack, knockUp);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
