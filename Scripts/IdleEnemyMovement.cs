using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEnemyMovement : MonoBehaviour
{

    [SerializeField, Tooltip("Speed (units/sec)")]
    private float speed;

    [SerializeField, Tooltip("Jump Force (units/sec)")]
    private float jumpForce;
    private float jumpHeight;
    Rigidbody2D rb;
    [SerializeField, Tooltip("Ground check (boolean)")]
    private bool grounded;

    private Collider2D holding;
    private float right;

    // Start is called before the first frame update
    void Start()
    {
        jumpForce = 10.0f;
        jumpHeight = 1.5f;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        right = 1;

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == ("Ground")) || (collision.gameObject.tag == ("Temporary Platform")))
        {
            grounded = true;
        } 

        // Collision with solid objects causes change in direction
        if (collision.gameObject.tag != "Player") {
            right *= -1;
            Debug.Log("Enemy collision: " + collision.gameObject.tag + right);
        }
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        // Leaving ground
        if (collision.gameObject.tag == ("Ground"))
        {
            grounded = false;
        }
    }



    void FixedUpdate() {

        // TODO: need to eventually add cliff detection 
        if (grounded) {
            transform.position += transform.right * right * speed * Time.deltaTime;
        }
        
    }
}
