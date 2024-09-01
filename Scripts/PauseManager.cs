using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    public GameObject deathMenu;

    public GameObject pauseMenu;

    private bool paused;

    private GameObject activeMenu;

    void Start() {
        paused = false;
        activeMenu = null;
    }

    public void Freeze()
    {
        var activeObjects = FindObjectsOfType(typeof(FreezableObject));

        foreach(object obj in activeObjects) {
            ((FreezableObject) obj).Freeze();
        }
    }

    public void Unfreeze() 
    {
        var activeObjects = FindObjectsOfType(typeof(FreezableObject));

        foreach(object obj in activeObjects) {
            ((FreezableObject) obj).Unfreeze();
        }
    }

    public void DeathEvent() {
        Freeze();
        Instantiate(deathMenu);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (!paused) {
                Freeze();
                activeMenu = Instantiate(pauseMenu);
                paused = true;
            } else {
                paused = false;
                Unfreeze();
                Destroy(activeMenu);
                activeMenu = null;
            }
            
        }
    }
}
