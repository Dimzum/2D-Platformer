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

    private AudioSource source;
    
    public void SetSource(AudioSource aSource) {
        source = aSource;
        source.clip = clip;
    }

    public void Play() {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }
}

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    [SerializeField] Sound[] sounds;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one AudioManager in the scene.");
        } else {
            instance = this;
        }
    }

    private void Start() {
        for (int i = 0; i < sounds.Length; i++) {
            GameObject go = new GameObject("Sound" + i + "_" + sounds[i].name);
            go.transform.SetParent(this.transform);
            sounds[i].SetSource(go.AddComponent<AudioSource>());
        }
    }

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
}
