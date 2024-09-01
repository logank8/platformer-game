using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage player
        if (collision.gameObject.tag == "Player") {
            PlayerStats pstat = collision.gameObject.GetComponent<PlayerStats>();
            pstat.Damage(1);

            collision.otherCollider.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;

            StartCoroutine(ReenableCollider());
        }
    }

    // Called by OnCollisionEnter2D(playercollision)
    // Temporarily disables collider for a few moments and then reenables
    private IEnumerator ReenableCollider() {
        
        yield return new WaitForSeconds(1f);

        Collider2D collider = gameObject.GetComponent<Collider2D>();
        collider.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    // Proj
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Projectile") {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
}
