using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Rigidbody2D rb;

    public int hearts;

    public PauseManager pauseManager;

    private bool damageable;

    [SerializeField]
    private int coins;

    [SerializeField]
    private int rocks;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();

        hearts = 3;
        damageable = true;

        coins = 0;

        rocks = 0;
    }

    void OnTriggerEnter2D(Collider2D collider) {

        // Coin collection
        if (collider.gameObject.tag == ("Coin"))
        {
            coins++;
            Destroy(collider.gameObject);
        }
        // Rock collection
        if (collider.gameObject.tag == "Rock") {
            rocks++;
            Destroy(collider.gameObject);
        }
    }

/** Damage/Health **/
    void FixedUpdate() {

        // Fall damage kills the player
        if (transform.position.y <=-6.5) {
            this.Damage(hearts);
        }
    }

    // Damage
    // Take damageAmnt as negative hearts
    public void Damage(int damageAmnt) {
        if (damageable) 
        {
            hearts -= damageAmnt;
            damageable = false;
            if (hearts <= 0) {
                // DIE !!!!!
                // Transition to death menu:
                // * halt all movement - make all rigidbodies static , any animations change to frozen state'

                // instantiate death menu
                pauseManager.DeathEvent();

            }

            StartCoroutine(ReenableDamage());
        }
    }

    // Called from Damage()
    // Disables damage for a small amount of time after initially taking damage
    private IEnumerator ReenableDamage() {
        
        yield return new WaitForSeconds(1f);
        
        damageable = true;

    }

    // Healing
    // Assumes healAmnt >= 0
    public void Heal(int healAmnt) {
        hearts += healAmnt;
        if (hearts > 3) {
            hearts = 3;
        }
    }

/** Rocks **/   

    // Static method, returns rock count
    public int getRocks() {
        return rocks;
    }

    // Edit rock amount
    // Can add and subtract with pos/neg
    public void changeRockCountBy(int rockAmnt) {
        rocks += rockAmnt;
        if (rocks < 0) {
            rocks = 0;
        }
    }

}