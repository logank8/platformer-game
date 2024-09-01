using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{

    public GameObject enemySpawn;
    public Vector3 savedSpawnPoint;


    // Detects if player has reached spawnpoint for enemy
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") 
        {
            Instantiate(enemySpawn, savedSpawnPoint, transform.rotation);
            Destroy(gameObject);
        }
        
    }

}