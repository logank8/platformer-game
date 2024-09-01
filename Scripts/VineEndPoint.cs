using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Last part of a vine
public class VineEndPoint : MonoBehaviour
{
    public Rigidbody2D rb;

    void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Connect to a post/branch that can connect to a vine
    void OnTriggerEnter2D(Collider2D collider) {

        if ((collider.gameObject.tag == "Vine Connect")) {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
    }

    // Detach from VineConnect
    public void Detach() {
        rb.constraints = RigidbodyConstraints2D.None;
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}