using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    #region Singleton
    public static GameMaster gm;

    [SerializeField] private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives {
        get { return _remainingLives; }
    }

    private void Awake() {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }
    #endregion

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;

    public CameraShake camShake;

    [SerializeField] private GameObject _gameOverUI;

    // Cache
    private AudioManager audioManager;

    private void Start() {
        if (camShake == null) {
            Debug.LogError("No camera shake referenced in Game Master.");
        }

        _remainingLives = maxLives;

        // Caching
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("No AudioManager found in the scene");
        }
    }

    public void EndGame() {
        Debug.Log("GAME OVER!");
        _gameOverUI.SetActive(true);
    }

    /*------------------ PLAYER ------------------*/
    public IEnumerator RespawnPlayer() {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform; // spawn particle effect
        Destroy(clone.gameObject, 2f);
    }

    public static void KillPlayer(Player p) {
        Destroy(p.gameObject);
        _remainingLives -= 1;
        if (_remainingLives <= 0) {
            gm.EndGame();
        } else {
            gm.StartCoroutine(gm.RespawnPlayer());
        }
    }

    /*------------------ ENEMY ------------------*/
    public static void KillEnemy(Enemy enemy) {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy enemy) {
        Transform clone = Instantiate(enemy.deathParticles, enemy.transform.position, Quaternion.identity);
        Destroy(clone.gameObject, 5f);
        camShake.Shake(enemy.shakeAmount, enemy.shakeLength);
        Destroy(enemy.gameObject);
    }
}
