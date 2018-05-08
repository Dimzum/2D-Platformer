using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [System.Serializable]
    #region Enemy stats class
    public class EnemyStats {
        public float health = 100f;
    }
    #endregion

    public EnemyStats stats = new EnemyStats();

    public void DamageEnemy(int damage) {
        stats.health -= damage;

        if (stats.health <= 0) {
            //Debug.Log("KILL Enemy");
            GameMaster.KillEnemy(this);
        }
    }
}
