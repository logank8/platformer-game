using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script should go to a Vine to communicate with player and VineControl
public class VinePiece : MonoBehaviour
{
    public Rigidbody2D rb;

    VineControl vineControl;

    private int vineLength;

    private int vinePos;

    void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();

        vineControl = gameObject.GetComponentInParent<VineControl>();

        // save initial position and hinge parent
    }

  
    // player calls method on the vine piece that it grabs onto, vine piece passes on to parent with VineControl
    // return false if connection fails, true on success
    public bool PlayerAttach() {
        return vineControl.PlayerAttach();
    }

    // Let go of player
    public void PlayerDetach() {
        vineControl.PlayerDetach();
        GetComponent<Collider2D>().enabled = false;
    }

    public void Decay() {
        rb.bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    public void Regrow(Vector3 newPos) {
        transform.position = newPos;
        transform.rotation = transform.parent.rotation;

        rb.bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }



    // Set vine length
    public void SetVineLength(int len) {
        vineLength = len;
    }

    // Set vine position
    public void SetVinePos(int pos) {
        vinePos = pos;
        if (vinePos == vineLength - 1) {
            gameObject.AddComponent(typeof(VineEndPoint));
        }

        rb.bodyType = RigidbodyType2D.Static;
        StartCoroutine(Reanim());
    }

    private IEnumerator Reanim() {
        yield return new WaitForSeconds(3f);

        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}