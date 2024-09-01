using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Power up ideas - putting these here so I don't forget. for now, we'll just have speed increases + higher jumps
 - double jump
 - dash
 - flight
 - wall climb/jump
 - could also have a melee weapon that breaks easily - idk kind of a powerup
 - bigger/stronger rocks
 - indestructible
**/

public class PlayerPowerUps : MonoBehaviour
{
    public Rigidbody2D rb;

    PlayerMovement pmv;

    void Start() {
        pmv = gameObject.GetComponent<PlayerMovement>();
    }

    public void SpeedPowerUp() {
        pmv.speed += 3f;
    }

    public void JumpPowerUp() {
        pmv.jumpForce += 3f;
    }

}