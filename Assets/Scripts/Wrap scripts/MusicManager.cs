using System.Collections;
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
			audioSource.loop = true;
			audioSource.Play ();
			lastPlayedClip = sceneNumber;
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetVolume (float volume){
		audioSource.volume = volume;
	}
}
