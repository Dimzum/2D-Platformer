using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;
    public AudioClip clip;

    [Range(0, 1f)] public float volume = .7f;
    [Range(.5f, 1.5f)] public float pitch = 1f;

    [Range(0f, .5f)] public float randomVolume = .1f;
    [Range(0f, .5f)] public float randomPitch = .1f;

    public bool loop = false;

    private AudioSource source;
    
    public void SetSource(AudioSource aSource) {
        source = aSource;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play() {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }

    public void Stop() {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    [SerializeField] Sound[] sounds;

    private void Awake() {
        if (instance != null) {
            if (instance != this) {
                Destroy(this.gameObject);
            }
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start() {
        for (int i = 0; i < sounds.Length; i++) {
            GameObject go = new GameObject("Sound" + i + "_" + sounds[i].name);
            go.transform.SetParent(this.transform);
            sounds[i].SetSource(go.AddComponent<AudioSource>());
        }

        PlaySound("Music");
    }

    // Used to test StopSound(string)
    /*private void Update() {
        if (Time.time > 5f) {
            StopSound("Music");
        }
    }*/

    public void PlaySound(string aName) {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name == aName) {
                sounds[i].Play();
                return;
            }
        }

        //No sound with aName
        Debug.LogWarning("AudioManger: Sound not found in array: " + aName);
    }

    public void StopSound(string aName) {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name == aName) {
                sounds[i].Stop();
                return;
            }
        }

        //No sound with aName
        Debug.LogWarning("AudioManger: Sound not found in array: " + aName);
    }
}
