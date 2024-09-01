using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezableObject : MonoBehaviour
{

    Rigidbody2D rb;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Freeze()
    {
        // TODO: halt any animation
        rb.bodyType = RigidbodyType2D.Static;

        if (gameObject.tag == "Player") {
            gameObject.GetComponent<PlayerMovement>().enabled = false;
        }
    }

    public void Unfreeze() {
        rb.bodyType = RigidbodyType2D.Dynamic;
        if (gameObject.tag == "Player") {
            gameObject.GetComponent<PlayerMovement>().enabled = true;
        }
    }
}
