using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour {
    
    public int fallBoundary = -20;

    public string deathSoundName = "DeathVoice";
    public string takeDamageSoundName = "TakeDamageSound";

    private AudioManager audioManager;

    [SerializeField] private StatusIndicator statusIndicator;

    private PlayerStats stats;

    private void Start() {
        stats = PlayerStats.instance;
        stats.CurrHealth = stats.MaxHealth;

        if (statusIndicator == null) {
            Debug.LogError("No status indicator referenced on Player");
        } else {
            statusIndicator.SetHealth(stats.CurrHealth, stats.MaxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("No AudioManager found in the scene.");
        }

        InvokeRepeating("RegenHealth", 1f / stats.healthRegenRate, 1f / stats.healthRegenRate);
    }

    void RegenHealth() {
        stats.CurrHealth += 1;
        statusIndicator.SetHealth(stats.CurrHealth, stats.MaxHealth);
    }

    private void Update() {
        if (transform.position.y <= fallBoundary) {
            DamagePlayer(int.MaxValue);
        }
    }

    void OnUpgradeMenuToggle(bool active) {
        GetComponent<Platformer2DUserControl>().enabled = !active; // Disable player's movement
        Weapon weapon = GetComponentInChildren<Weapon>();
        if (weapon != null) {
            weapon.enabled = !active; // Disable player's weapon
        }
    }

    private void OnDestroy() {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }

    public void DamagePlayer(int damage) {
        stats.CurrHealth -= damage;

        if (stats.CurrHealth <= 0) {
            //Debug.Log("KILL PLAYER");

            // Play death sound
            audioManager.PlaySound(deathSoundName);

            GameMaster.KillPlayer(this);
        } else {
            // Play take damage sound
            audioManager.PlaySound(takeDamageSoundName);
        }

        statusIndicator.SetHealth(stats.CurrHealth, stats.MaxHealth);
    }
}
