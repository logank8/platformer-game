using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Last part of a vine
public class SceneControl : MonoBehaviour
{

    public void PressStart() {
        Debug.Log("Level 1 started");
        SceneManager.LoadScene("Level1");
    }

    public void PressExit() {
        Application.Quit();
        Debug.Log("End");
    }
    public void MainMenuReturn() {
        Debug.Log("Main menu");
        SceneManager.LoadScene("Main Menu");
    }
} 