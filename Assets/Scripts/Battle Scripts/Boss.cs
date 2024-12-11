using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    public Transform player;
    public float maxHealth;
    public float currentHealth;
    

    /* Sets the Boss to a state (default state) */
    void Awake()
    {
    }

    private void Update()
    {
        
    }

    /* FLips the player's sprite */
    public abstract void Flip();

    /* Performs a light/Quick attack */
    public abstract void LightAttack();

    /* Performs a heavy attack */
    public abstract void HeavyAttack();

    /* Performs a special attack */
    public abstract void SpecialAttack();

    /* Gets the distance to the player */
    public float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.position);
    }

    /* Moves away from the player */
    public abstract void MoveAwayFromPlayer();

    /* Collisions with objects */
    void OnCollisionEnter2D(Collision2D other)
    {
    }

    /* Moves towards the player */
    public abstract void MoveToPlayer();

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
    public abstract IEnumerator Dash(float direction);

    /* Takes Damage from an object */
    public abstract IEnumerator TakeDamage(float damage);

}
