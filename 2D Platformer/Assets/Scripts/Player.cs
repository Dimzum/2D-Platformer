using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [System.Serializable]
    #region Player stats class
    public class PlayerStats {
        public int maxHealth = 100;

        private int _currHealth;
        public int currHealth {
            get { return _currHealth; }
            set { _currHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init() {
            currHealth = maxHealth;
        }
    }
    #endregion

    public PlayerStats stats = new PlayerStats();

    public int fallBoundary = -20;

    [SerializeField] private StatusIndicator statusIndicator;

    private void Start() {
        stats.Init();

        if (statusIndicator == null) {
            Debug.LogError("No status indicator referenced on Player");
        } else {
            statusIndicator.SetHealth(stats.currHealth, stats.maxHealth);
        }
    }

    private void Update() {
        if (transform.position.y <= fallBoundary) {
            DamagePlayer(int.MaxValue);
        }
    }

    public void DamagePlayer(int damage) {
        stats.currHealth -= damage;

        if (stats.currHealth <= 0) {
            //Debug.Log("KILL PLAYER");
            GameMaster.KillPlayer(this);
        }

        statusIndicator.SetHealth(stats.currHealth, stats.maxHealth);
    }
}
