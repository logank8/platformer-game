using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlatform : MonoBehaviour
{

    // TODO: maybe consider what it would look like if the temporary platform had control over its own regeneration.
    //          probably better

    bool waitingForReactivate;
    int waitTime;
    int waitCounter;

    Rigidbody2D rb;

    // all we need here is a timer implemented - then we disable the collider, animate falling (use sprite stuff probably)
    //   and then after a few secs animate return and enable collider

    // Start is called before the first frame update
    void Start()
    {
    }


    void OnDisable() {
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
