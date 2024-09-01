using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
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


    public GameObject tempPlatform;
    public bool onTempPlat;
    public float timePassedTempPlat;
    public const float tempPlatformDelay = 1500f;

    // Start is called before the first frame update
    void Start()
    {
        speed = 8.5f;
        jumpForce = 10.0f;
        jumpHeight = 1.5f;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        onTempPlat = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == ("Ground")) || (collision.gameObject.tag == ("Temporary Platform")))
        {
            grounded = true;
        }

        // NOTE: will probably eventually have to change this from tilemap to its own prefab
        if (collision.gameObject.tag == ("Bouncy Ground")) {
            float bounceForce = 45f;
            rb.velocity = transform.up * bounceForce;
        }

        if (collision.gameObject.tag == ("Temporary Platform")) {
            tempPlatform = collision.gameObject;
            onTempPlat = true;
            timePassedTempPlat = 0;
            
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Projectile") {
            Debug.Log("Projectile hit");
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            grounded = false;
        }

        if (collision.gameObject.tag == "Temporary Platform") {
            grounded = false;
            onTempPlat = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (onTempPlat) {
            timePassedTempPlat++;
            if (timePassedTempPlat >= tempPlatformDelay) {
                Destroy(tempPlatform); // TODO: eventually change to deactivate
                onTempPlat = false;
            }
        }
    }

    void FixedUpdate()
    {

        
    }
}
