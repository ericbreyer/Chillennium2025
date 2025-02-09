using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXMANGER : MonoBehaviour {
	// Audio players components.
	public AudioSource Source;
	public AudioClip[] monkeydialogues;
	public AudioClip[] hamdialogues;

	// Singleton instance.
	public static SFXMANGER Instance = null;

	// Initialize the singleton instance.
	private void Awake() {
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null) {
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this) {
			Destroy(gameObject);
		}

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);
	}

	// Play a single clip through the sound effects source.
	public void Play(AudioClip clip) {
		if(clip == null) {
			return;
        }
		Source.Stop();
		Source.clip = clip;
		Source.Play();
	}

	// Play a random clip from an array, and randomize the pitch slightly.
	public void RandomSoundEffect(params AudioClip[] clips) {
		int randomIndex = Random.Range(0, clips.Length);

		Source.clip = clips[randomIndex];
		Source.Play();
	}

	public AudioClip MonkeyDialogue() {
		return monkeydialogues[Random.Range(0, monkeydialogues.Length)];
    }

	public AudioClip HamDialogue() {
		return hamdialogues[Random.Range(0, hamdialogues.Length)];
	}
}
