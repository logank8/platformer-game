using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillPoint : MonoBehaviour
{
    GameObject parent;

    void Start() {
        parent = transform.parent.gameObject;
    }


    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Enemy killed");
            // TODO: do bounce effect similar to bouncy ground but less bounce
            Destroy(parent);
            Destroy(gameObject);
        }
        
    }

}