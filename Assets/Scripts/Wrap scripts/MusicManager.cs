﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public AudioClip[] levelMusicChangeArray;

	private AudioSource audioSource;

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
			audioSource = GetComponent<AudioSource> ();
			audioSource.clip = thisLevelMusic;
			if (sceneNumber == 0) { // Splash screen
				audioSource.loop = false;
			}
			else{ 
				audioSource.loop = true;
			}
			audioSource.Play ();
			lastPlayedClip = sceneNumber;
		}
	}

	public void SetVolume (float volume){
		audioSource.volume = volume;
	}

	public float GetVolume (){
		return audioSource.volume;
	}
}
