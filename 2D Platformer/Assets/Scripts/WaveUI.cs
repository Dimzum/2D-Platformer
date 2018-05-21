using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

    [SerializeField] WaveSpawner spawner;
    [SerializeField] Animator waveAnimator;
    [SerializeField] Text waveCoutndownText;
    [SerializeField] Text waveCountText;

    private WaveSpawner.SpawnState prevState;

	// Use this for initialization
	void Start () {
		if (spawner == null) {
            Debug.LogError("No spawner referenced.");
            this.enabled = false;
        }

        if (waveAnimator == null) {
            Debug.LogError("No waveAnimator referenced.");
            this.enabled = false;
        }

        if (waveCoutndownText == null) {
            Debug.LogError("No waveCoutndownText referenced.");
            this.enabled = false;
        }

        if (waveCountText == null) {
            Debug.LogError("No waveCountText referenced.");
            this.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		switch (spawner.State) {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
        }

        prevState = spawner.State;
	}

    void UpdateCountingUI() {
        if (prevState != WaveSpawner.SpawnState.COUNTING) {
            //Debug.Log("Counting");
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
        }

        waveCoutndownText.text = ((int)spawner.WaveCountDown).ToString();
    }

    void UpdateSpawningUI() {
        if (prevState != WaveSpawner.SpawnState.SPAWNING) {
            //Debug.Log("Spawning");
            waveAnimator.SetBool("WaveCountdown", false);
            waveAnimator.SetBool("WaveIncoming", true);

            //waveCountText.text = spawner.NextWave.ToString();
            waveCountText.text = spawner.waves[spawner.NextWave - 1].name;
        }
    }
}
