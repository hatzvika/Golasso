using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOnOff : MonoBehaviour {

	public Sprite onImage;
	public Sprite offImage;

	private MusicManager musicManager;
	private TitleImagesInGame titleImagesInGame; // Also handles in-game title effects

	private bool audioOn = true;
	private float volumeOn;

	Button button;

	void Start () {
		musicManager = GameObject.FindObjectOfType<MusicManager> ();

		button = GetComponent<Button> ();
		button.image.overrideSprite = onImage;
		if (musicManager) {
			volumeOn = musicManager.GetMusicVolume ();
		}
	}

	public void ChangeAudioState(){
		audioOn = !audioOn;
		if (audioOn){
			button.image.overrideSprite = onImage;
			musicManager.SetMusicVolume (volumeOn);
		} else{
			button.image.overrideSprite = offImage;
			musicManager.SetMusicVolume (0);
		}
	}

	public bool GetAudioOn(){
		return audioOn;
	}
}
