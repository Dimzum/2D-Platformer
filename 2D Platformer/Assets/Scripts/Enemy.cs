using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour {

    [System.Serializable]
    #region Enemy stats class
    public class EnemyStats {
        public int maxHealth = 100;
        public int damage = 20;

        private int _currtHealth;
        public int CurrHealth {
            get { return _currtHealth; }
            set { _currtHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init() {
            CurrHealth = maxHealth;
        }
    }
    #endregion

    public EnemyStats stats = new EnemyStats();

    public Transform deathParticles;

    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;

    [Header("Optional: ")] [SerializeField] private StatusIndicator statusIndicator;

    public string deathSoundName = "Explosion";

    public int moneyDropAmount;

    private void Start() {
        stats.Init();

        moneyDropAmount = Random.Range(10, 15);

        if (statusIndicator != null) {
            statusIndicator.SetHealth(stats.CurrHealth, stats.maxHealth);
        }

        if (deathParticles == null) {
            Debug.LogError("No death particles referenced on enemy.");
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;
    }

    void OnUpgradeMenuToggle(bool active) {
        // Disable enemy's movement
        GetComponent<EnemyAI>().enabled = !active;
        //GetComponent<Seeker>().enabled = !active;
    }

    private void OnDestroy() {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }

    public void DamageEnemy(int damage) {
        stats.CurrHealth -= damage;

        if (stats.CurrHealth <= 0) {
            //Debug.Log("KILL Enemy");
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null) {
            statusIndicator.SetHealth(stats.CurrHealth, stats.maxHealth);
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
