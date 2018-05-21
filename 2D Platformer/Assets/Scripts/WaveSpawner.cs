using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int _nextWave = 0;
    public int NextWave {
        get { return _nextWave + 1; }
    }

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float _waveCountDown;
    public float WaveCountDown {
        get { return _waveCountDown; }
    }

    public int nextWaveSpawnTime;
    private float searchCountDown = 1f;

    [SerializeField] private SpawnState _state = SpawnState.COUNTING;
    public SpawnState State {
        get { return _state; }
    }

    // Use this for initialization
    void Start() {
        if (spawnPoints.Length == 0) {
            Debug.LogError("No spawn points referenced.");
        }

        _waveCountDown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update() {
        if (_state == SpawnState.WAITING) {
            if (!EnemyIsAlive()) {
                WaveCompleted();
            }
            else {
                return;
            }
        }

        if (_waveCountDown <= 0) {
            if (_state != SpawnState.SPAWNING) {
                StartCoroutine(SpawnWave(waves[_nextWave]));
            }
        }
        else {
            _waveCountDown -= Time.deltaTime;
        }
    }

    void WaveCompleted() {
        Debug.Log("Wave completed!");

        _state = SpawnState.COUNTING;
        _waveCountDown = timeBetweenWaves;

        if (_nextWave + 1 > waves.Length - 1) {
            _nextWave = 0;
            Debug.Log("All waves complete! Looping...");
        }
        else {
            _nextWave++;
        }
    }

    bool EnemyIsAlive() {
        searchCountDown -= Time.deltaTime;

        if (searchCountDown <= 0f) {
            searchCountDown = 1f;

            if (GameObject.FindGameObjectWithTag("Enemy") == null) {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave wave) {
        Debug.Log("Spawning wave: " + wave.name);
        _state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.count; i++) {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        _state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform enemy) {
        Debug.Log("Spawning enemy: " + enemy.name);

        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, sp.position, sp.rotation);
    }
}
