using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    [SerializeField] string mouseHoverSound = "ButtonHover";

    [SerializeField] string buttonPressedSound = "ButtonPress";

    AudioManager audioManager;

    private void Start() {
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("No AudioManager found in the scene.");
        }
    }

    public void Quit() {
        audioManager.PlaySound(buttonPressedSound);

        Debug.Log("Exiting the Game!");
        Application.Quit();
    }

    public void MainMenu() {
        audioManager.PlaySound(buttonPressedSound);
        SceneManager.LoadScene(0);
    }

    // Returns the player the the main menu
    public void PlayAgain() {
        audioManager.PlaySound(buttonPressedSound);

        //Application.LoadLevel(Application.loadedLevel);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(1);  // Loads Level_1
    }

    public void OnMouseOver() {
        audioManager.PlaySound(mouseHoverSound);
    }
}
