using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Walking variables
    [SerializeField, Tooltip("Speed (units/sec)")]
    public float speed;
    public float minSpeedWalk;

    public float maxSpeedWalk;

    // Jump variables
    [SerializeField, Tooltip("Jump Force (units/sec)")]
    public float jumpForce;
    private float jumpHeight;

    [SerializeField, Tooltip("Ground check (boolean)")]
    private bool grounded;

    [SerializeField]
    PlayerStats stats;
    Rigidbody2D rb;
    

    // Vine variables
    private Collider2D holding;

    private const float vineWait = 300f;
    private float vineCollidedTime;
    private Collider2D vineCollider;
    private bool vineCheck;

    // Temporary platform variables
    public GameObject tempPlatform;
    public bool onTempPlat;
    public float timePassedTempPlat;
    public const float tempPlatformDelay = 1000f;

    // Keeps track of the temporary platforms that have been destroyed 
    //  & how long they have been waiting to regenerate
    public List<DisabledPlatform> disabledPlatforms;
    public struct DisabledPlatform {
        public GameObject platform;
        public int waiting;
    }

    Animator animator;


    // Start is called before the first frame update
    // TODO: maybe eventually sort these into diff init statements
    void Start()
    {
        speed = 9.5f;
        jumpForce = 10.0f;
        jumpHeight = 1.5f;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        holding = null;
        vineCheck = false;

        onTempPlat = false;

        disabledPlatforms = new List<DisabledPlatform>();

        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collider) {

        // Attach to vine
        if ((collider.gameObject.tag == ("Vine")) && (holding == null)) {
            Debug.Log("Vine collided");
            
            vineCheck = true;
            vineCollidedTime = 0;
            vineCollider = collider;
        }
    }


    // Update is called once per frame
    async void Update()
    {

        // new grounded check - test
        RaycastHit2D groundRay = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f), Vector2.down, 1.5f, LayerMask.GetMask("Default"));
        if (groundRay.collider != null) {
            // Player on ground
            if (groundRay.collider.gameObject.tag == "Ground") 
            {
                grounded = true;

            
            } // Player on temporary platform
            else if ((!onTempPlat) && (groundRay.collider.gameObject.tag == "Temporary Platform")) 
            {
                tempPlatform = groundRay.collider.gameObject;
                onTempPlat = true;
                timePassedTempPlat = 0;

            
            } // Player on bouncy platform
            else if (groundRay.collider.gameObject.tag == "Bouncy Ground")
            {
                float bounceForce = 45f;
                rb.velocity = transform.up * bounceForce;

            } // Player not on anything
            else 
            {
                grounded = false;
            }
        } else {
            grounded = false; 
            onTempPlat = false;
        }



        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        if (Input.GetAxis("Horizontal") != 0) {
            if (speed < maxSpeedWalk) {
                speed += Mathf.Min(0.15f, maxSpeedWalk - speed);
            }
        } else {
            if (speed > minSpeedWalk) {
                speed -= Mathf.Min(0.15f, speed - minSpeedWalk);
            }
        }
        rb.AddForce(new Vector2(horizontal, 0));
        transform.position += new Vector3(horizontal, 0, 0);
        // add velocity to this - like start out at a certain speed and add a bit more if already walking but put a cap on it
   
        // If player is attached to vine - check for detach input
        if (holding != null) {
            if (Input.GetAxis("Jump") != 0) {
                rb.velocity = holding.GetComponent<Rigidbody2D>().velocity;
                rb.AddForce(rb.velocity * 1.5f, ForceMode2D.Impulse);

                holding.gameObject.GetComponent<VinePiece>().PlayerDetach();

                holding = null;
                // TODO: set timer here for appprox 3 seconds that you can't reattach to the vine ? or maybe don't idk
            } else {
                // TODO: change this slightly for the offset, not sure how yet - save some difference value ?
                transform.position = holding.transform.position;
                // TODO: allow player to change position relative to scale of the vine piece its attached to
                //          if the player goes up/down far enough to get to a new vine piece, reattach

            }
        }

        // Handle if player is collided with a vine - check for key input to grab onto the vine
        if (vineCheck) {
            if (vineCollidedTime > vineWait) {
                vineCheck = false;
            } else if (Input.GetKeyDown(KeyCode.C)) {
                vineCheck = false;
                Debug.Log("Input received");

                VinePiece vine = vineCollider.gameObject.GetComponent<VinePiece>();
                Debug.Log(vine);
                if (vine.PlayerAttach()) {
                    transform.position = vineCollider.transform.position;
                    holding = vineCollider;
                }
                
            } else {
                vineCollidedTime += 1;
            }
        }

        // Check for current temporary platform interaction
        if (onTempPlat) {
            timePassedTempPlat++;
            if (timePassedTempPlat >= tempPlatformDelay) {
                disabledPlatforms.Add(new DisabledPlatform {
                    platform = tempPlatform,
                    waiting = 0
                });
                tempPlatform.SetActive(false);
                onTempPlat = false;
            }
        }

        // Check temporary platforms that have been disabled
        int platCount = disabledPlatforms.Count;
        for(int i = 0; i < platCount; i++) {
            if (i >= disabledPlatforms.Count) {
                break;
            }
            Debug.Log("Checking platforms");
            DisabledPlatform pf = disabledPlatforms[i];

            float wait = (float) pf.waiting;            
            if (wait >= tempPlatformDelay) {
                Debug.Log("Check failed");
                disabledPlatforms.Remove(pf);
                i--;
                pf.platform.SetActive(true);
            } else {
                Debug.Log("Check succeeded");
                pf.waiting = ((int) wait) + 1;
                disabledPlatforms[i] = pf;
            }
        }
    }


    void FixedUpdate()
    {

        /** 
        jump - good for now 
            want to make more sensitive for short/long jump
            might want to add double jump or dash later
        **/
        if ((Input.GetAxis("Jump") != 0) && (grounded)) {
            // jump velocity = sqrt ( -2 * gravity * jumpHeight)
            if (rb.velocity.y >= -0.5) {
                jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            
        }
    }
}
