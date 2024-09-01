using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bullet;

    private Camera cam;

    private float timePassedFiring;
    private bool firing;
    private const float firingWaitTime = 500f;

    public int posOffset;
    PlayerStats playerScript;

    public float angle;


    void Start() {
        cam = Camera.main;
        timePassedFiring = 0;
        firing = false;

        angle = 0;
    }

    void Update() {
        playerScript = GetComponentInParent<PlayerStats>();
        int rockCount = playerScript.getRocks();

        // Rotating towards mouse location on screen
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0f;

        Vector3 objectPos = cam.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        // Time interval between shoots
        if (firing) {
            timePassedFiring++;
            if (timePassedFiring >= firingWaitTime) {
                firing = false;
            }
        }

        // Fire rocks if you have them + player input 
        if (rockCount > 0) {
            if (Input.GetAxis("Fire1") != 0) {
                Debug.Log("Firing");
                if (!firing) {
                    GameObject obj = Instantiate(bullet, transform.position, transform.rotation);
                    Projectile proj = obj.GetComponent<Projectile>();
                    proj.Throw(mousePos);

                    firing = true;
                    timePassedFiring = 0;
                    playerScript.changeRockCountBy(-1);
                }
            }
        }
        
    }


}