using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Flying enemy movement components
// * strongly contrasted (and fast ? ) acceleration and deceleration
// * idle around , patrol back and forth (make this more oblong in the future, but for now only edit xvals)
// ** player spotted - attack style
// ** move to get a diagonal attack from the top
// ** at top - record player position
// ** accelerate towards recorded playerPos
// ** mirror ray off of bottom of ground - like pool
// ** wait 1 sec, check if player is still in sight, then repeat attack 


// the only thing I want it to be able to run into is the player - might have to do this either with layers or trigger


public class FlyingEnemyMovement : MonoBehaviour
{
    private float speed;

    Rigidbody2D rb;

    private GameObject player;

    // patrolling
    private float right;

    public float yBase;
    private Vector3 patrolOrigin;

    public Vector2 target;
    public State state;

    // set a max speed

    public enum State {
        Enter,
        Patrol,
        Attack
    }

    private float leftDiff;
    private float rightDiff;
    public float maxSpeed;
    public float minSpeed;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        right = 1;

        target = new Vector2(transform.position.x, yBase);
        leftDiff = 3f;
        rightDiff = 3f;

        speed = 5f;

        player = GameObject.Find("Player");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Projectile") {
            Debug.Log("Projectile hit");
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
        if (collider.gameObject.tag == ("Player"))
        {
            PlayerStats playerStat = collider.gameObject.GetComponent<PlayerStats>();
            playerStat.Damage(1);
        } 
    }


    // Update is called once per frame
    void Update()
    {

        // check for player sighting
        if (state == State.Patrol || state == State.Attack) {
            if (Vector2.Distance(player.transform.position, transform.position) <= 15f) {
                Debug.Log("Player spotted");
                if (state == State.Patrol) {
                    // call attack coroutine
                    StartCoroutine(EnterAttackMode());
                } else {
                    if (speed < maxSpeed) {
                        speed += Mathf.Min(0.5f, maxSpeed - speed);
                    }
                    if (rb.velocity.y > 0) {
                        if (speed > minSpeed) {
                            speed -= Mathf.Min(0.5f, speed - minSpeed);
                        }
                    }
                }
            } else if (state == State.Attack) {
                state = State.Patrol;

                // TODO: need to figure out how to get thru obstacles on y level and x level ... figure this out later ?
                target = transform.position;
                patrolOrigin = transform.position;
            }
        }

        if (transform.position.Equals(target)) {
            switch (state) {
                case State.Enter:
                    // begin patrolling  - set target
                    patrolOrigin.x = transform.position.x;
                    patrolOrigin.y = transform.position.y;

                    target.x = patrolOrigin.x + rightDiff;
                    target.y = transform.position.y;
                    state = State.Patrol;
                    break;
                case State.Patrol:
                    // switch x
                    if (transform.position.x < patrolOrigin.x) {
                        target.x = patrolOrigin.x + rightDiff;
                    } else {
                        target.x = patrolOrigin.x - leftDiff;
                    }
                    break;
                // if Attack and target == transform pos
                // if velocity < 0 go up ? like invert direction pos/neg ?
                // if velocity > 0 move to idle movement animation ??? I guess ???
                default:
                    break;
            }
        }

        // Debug: keeping y level
        if (target.y > yBase) {
            target.y = yBase;
        }

        

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // enforce y pos boundary on top and bottom
    }

    // 
    private IEnumerator EnterAttackMode() {

        Vector2 playerPos = player.transform.position;
        
        yield return new WaitForSeconds(1.5f);
        
        state = State.Attack;

        target = playerPos;
    }

}
