using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	public void Quit() {
        Debug.Log("Exiting the Game!");
        Application.Quit();
    }

    // Returns the player the the main menu
    public void PlayAgain() {
        //Application.LoadLevel(Application.loadedLevel);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }
}
