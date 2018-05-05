using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    #region Singleton
    public static GameMaster gm;

    private void Start() {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }
    #endregion

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;

    public IEnumerator RespawnPlayer() {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform; // spawn particle effect
        Destroy(clone.gameObject, 2f);
    }

    public static void KillPlayer(Player player) {
        Destroy(player.gameObject);
        gm.StartCoroutine(gm.RespawnPlayer());
    }
}
