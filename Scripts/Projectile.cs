using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public float despawnTimer;

    // Throwing rocks 
    // Slightly affected by gravity
    // TODO: add allowance for powerup somewhere here
    public void Throw(Vector3 mousePos) {
        rb.AddForce(Vector3.Normalize(mousePos) * speed);
        Debug.Log(mousePos);

        StartCoroutine(RockDespawn());
    }

    private IEnumerator RockDespawn() {
        
        yield return new WaitForSeconds(despawnTimer);

        Destroy(this.gameObject);
    }

    

}