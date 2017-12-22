using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public AudioClip[] levelMusicChangeArray;
	public AudioClip[] effectsAudioClips;

	public AudioSource musicAudioSource;
	public AudioSource effectsAudioSource;

	// I don't want to restart playing a clip that is already playing
	private int lastPlayedClip = -1;

	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		int sceneNumber = SceneManager.GetActiveScene ().buildIndex;
		AudioClip thisLevelMusic = levelMusicChangeArray [sceneNumber];

		if (thisLevelMusic && lastPlayedClip != sceneNumber) {
			musicAudioSource = GetComponent<AudioSource> ();
			musicAudioSource.clip = thisLevelMusic;
			if (sceneNumber == 0) { // Splash screen
				musicAudioSource.loop = false;
			}
			else{ 
				musicAudioSource.loop = true;
			}
			musicAudioSource.Play ();
			lastPlayedClip = sceneNumber;
		}
	}

	public void SetMusicVolume (float volume){
		musicAudioSource.volume = volume;
	}

	public float GetMusicVolume (){
		return musicAudioSource.volume;
	}

	public void PlayTitleEffect(TitleImagesInGame.TitleImages titleImage){
		AudioClip audioClip = effectsAudioClips [(int)titleImage];
		effectsAudioSource.clip = audioClip;
		effectsAudioSource.Play ();
	}

	public void SetEffectsVolume (float volume){
		effectsAudioSource.volume = volume;
	}

	public float GetEffectsVolume (){
		return effectsAudioSource.volume;
	}

}
