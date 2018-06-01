using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    /*-------------------- Singleton --------------------*/
    #region Singleton
    public static GameMaster gm;

    [SerializeField] private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives {
        get { return _remainingLives; }
    }

    [SerializeField] private int startingGold = 35;
    [SerializeField] private static int _gold;
    public static int Gold {
        get { return _gold; }
        set { _gold = value; }
    }

    private void Awake() {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }
    #endregion
    /*-------------------- Singleton --------------------*/

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;
    public string respawnCountdownSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";

    public string gameOverSoundName = "GameOver";

    public CameraShake camShake;

    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _upgradeMenu;

    [SerializeField] private WaveSpawner waveSpawner;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;

    // Aduio
    private AudioManager audioManager; // Cache
    private string pickupSoundName = "Pickup";

    private void Start() {
        if (camShake == null) {
            Debug.LogError("No camera shake referenced in Game Master.");
        }

        _remainingLives = maxLives;

        _gold = startingGold;

        // Caching
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("No AudioManager found in the scene.");
        }
    }

    private void Update() {
        // Press Esc for upgrade menu
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleUpgradeMenu();
        }
    }

    private void ToggleUpgradeMenu() {
        _upgradeMenu.SetActive(!_upgradeMenu.activeSelf);
        waveSpawner.enabled = !_upgradeMenu.activeSelf;
        onToggleUpgradeMenu.Invoke(_upgradeMenu.activeSelf);
    }

    public void EndGame() {
        audioManager.PlaySound(gameOverSoundName);

        Debug.Log("GAME OVER!");
        _gameOverUI.SetActive(true);
    }

    /*------------------ PLAYER ------------------*/
    public IEnumerator RespawnPlayer() {
        audioManager.PlaySound(respawnCountdownSoundName);
        yield return new WaitForSeconds(spawnDelay);

        audioManager.PlaySound(spawnSoundName);
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
        // Play some sound
        audioManager.PlaySound(enemy.deathSoundName);

        _gold += enemy.moneyDropAmount;
        audioManager.PlaySound(pickupSoundName);

        // Add Particles
        Transform clone = Instantiate(enemy.deathParticles, enemy.transform.position, Quaternion.identity);
        Destroy(clone.gameObject, 5f);

        // Go camerashake
        camShake.Shake(enemy.shakeAmount, enemy.shakeLength);
        Destroy(enemy.gameObject);
    }
}
