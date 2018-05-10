using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [System.Serializable]
    #region Enemy stats class
    public class EnemyStats {
        public int maxHealth = 100;
        public int damage = 30;

        private int _currtHealth;
        public int currHealth {
            get { return _currtHealth; }
            set { _currtHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init() {
            currHealth = maxHealth;
        }
    }
    #endregion

    public EnemyStats stats = new EnemyStats();

    [Header("Optional: ")] [SerializeField] private StatusIndicator statusIndicator;

    public Transform deathParticles;

    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;

    private void Start() {
        stats.Init();

        if (statusIndicator != null) {
            statusIndicator.SetHealth(stats.currHealth, stats.maxHealth);
        }

        if (deathParticles == null) {
            Debug.LogError("No death particles referenced on enemy.");
        }
    }

    public void DamageEnemy(int damage) {
        stats.currHealth -= damage;

        if (stats.currHealth <= 0) {
            //Debug.Log("KILL Enemy");
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null) {
            statusIndicator.SetHealth(stats.currHealth, stats.maxHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Player _player = collision.collider.GetComponent<Player>();
        if (_player != null) {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(int.MaxValue);
        }
    }
}
