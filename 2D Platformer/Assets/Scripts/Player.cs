using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [System.Serializable]
    #region Player stats class
    public class PlayerStats {
        public float health = 100f;
    }
    #endregion

    public PlayerStats stats = new PlayerStats();

    public int fallBoundary = -20;

    private void Update() {
        if (transform.position.y <= fallBoundary) {
            DamagePlayer(int.MaxValue);
        }
    }

    public void DamagePlayer(int damage) {
        stats.health -= damage;

        if (stats.health <= 0) {
            //Debug.Log("KILL PLAYER");
            GameMaster.KillPlayer(this);
        }
    }
}
