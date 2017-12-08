﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartVolume : MonoBehaviour {

	private MusicManager musicManager;

	// Use this for initialization
	void Start () {
		musicManager = GameObject.FindObjectOfType<MusicManager> ();

		if (musicManager){
			if (PlayerPrefs.HasKey ("master_volume")) {
				float volume = PlayerPrefsManager.GetMasterVolume ();
				musicManager.SetVolume (volume);
			} else{
				musicManager.SetVolume (0.8f);
			}
		}
	}
}
