using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartVolume : MonoBehaviour {

	private MusicManager musicManager;

	// Use this for initialization
	void Start () {
		musicManager = GameObject.FindObjectOfType<MusicManager> ();

		if (musicManager){
			if (PlayerPrefs.HasKey ("master_volume")) {
				float volume = PlayerPrefsManager.GetMusicVolume ();
				musicManager.SetMusicVolume (volume);
			} else{
				musicManager.SetMusicVolume (0.8f);
			}
		}
	}
}
