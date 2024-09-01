using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script should go to a VinePoint to control actions for vine break and regrowth
public class VineControl : MonoBehaviour
{
    public Rigidbody2D rb;

    public int vineLength;

    public GameObject vinePiecePrefab;
    
    // where we grow the vine to connect to
    GameObject connectionPoint;

    // VineEndPoint
    VineEndPoint endPoint;

    public State vineState;

    public enum State {
        Growing,
        Connected,
        Swinging,
        Broken,
    }

    [SerializeField]
    public List<VineInfo> startingPositions;

    // could possibly do hinge and transform and just save it as vine info
    [System.Serializable]
    public struct VineInfo {
        public VinePiece vineChild;
        public Vector3 startPos;

    }

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        vineState = State.Connected;

        endPoint = gameObject.GetComponentInChildren<VineEndPoint>();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Vine Connect") {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
    }

    // Player calls method on the vine piece that it grabs onto, vine piece passes on to parent with VineControl
    // Return false if connection fails, true on success
    public bool PlayerAttach() {
        if (vineState == State.Connected) {
            vineState = State.Swinging;
            endPoint.Detach();
            return true;
        }
        return false;
    }
 
    public void PlayerDetach() {
        vineState = State.Broken;
        // maybe run a coroutine of waiting a few seconds then start to regrow
        HingeJoint2D[] hinges = gameObject.GetComponentsInChildren<HingeJoint2D>();
        foreach(HingeJoint2D hinge in hinges) {
            if (hinge.connectedBody.gameObject.tag == "Vine Point") {
                Debug.Log("Correct hinge");
                hinge.enabled = false;
                // disconnect hinge
            }
        }


        StartCoroutine(VineRegenerate());
    }

    private IEnumerator VineRegenerate() {
        

        yield return new WaitForSeconds(5f);

        // might merge this into the coroutine so it can just happen within
        Decay();
        Regrow();
    }

    // assumed: vineState is Broken
    public void Regrow() {
        vineState = State.Growing;
        
        StartCoroutine(SlowRegrow());
    

        vineState = State.Connected;

    }

    private IEnumerator SlowRegrow() {
        // assumed: all vine children are changed to static
        VinePiece firstChild = startingPositions[0].vineChild;
        firstChild.gameObject.GetComponent<HingeJoint2D>().enabled = true;

        foreach(VineInfo child in startingPositions) {
            child.vineChild.Regrow(transform.TransformPoint(child.startPos));

            yield return new WaitForSeconds(1f);
        }

        // NOTE: still hesitant about this part, maybe should make it an internal method as that seems better practice
        foreach(VineInfo child in startingPositions) {
            child.vineChild.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    // called after hinge is disconnected
    private void Decay() {
        foreach (VinePiece child in gameObject.GetComponentsInChildren<VinePiece>()) {
            child.Decay();
        }
    }


}