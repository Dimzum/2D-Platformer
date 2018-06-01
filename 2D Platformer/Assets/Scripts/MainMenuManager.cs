using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    [SerializeField] string hoverOverSound = "ButtonHover";
    [SerializeField] string pressedButtonSound = "ButtonPress";

    AudioManager audioManager;

    private void Start() {
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("No audioManager found.");
        }
    }

    public void StartGame() {
        audioManager.PlaySound(pressedButtonSound);
        SceneManager.LoadScene(1);
    }

    public void HowToPlay() {
        audioManager.PlaySound(pressedButtonSound);

    }

    public void Credits() {
        audioManager.PlaySound(pressedButtonSound);

    }

    public void QuitGame() {
        audioManager.PlaySound(pressedButtonSound);
        Debug.Log("Exiting the Game.");
        Application.Quit();
    }

    public void OnMouseOver() {
        audioManager.PlaySound(hoverOverSound);
    }

}
