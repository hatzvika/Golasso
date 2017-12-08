using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOnOff : MonoBehaviour {

	public Sprite onImage;
	public Sprite offImage;

	private MusicManager musicManager;

	private bool audioOn = true;
	private float volumeOn;

	Button button;

	void Start () {
		musicManager = GameObject.FindObjectOfType<MusicManager> ();

		button = GetComponent<Button> ();
		button.image.overrideSprite = onImage;
		volumeOn = musicManager.GetVolume ();
	}

	public void ChangeAudioState(){
		audioOn = !audioOn;
		if (audioOn){
			button.image.overrideSprite = onImage;
			musicManager.SetVolume (volumeOn);
		} else{
			button.image.overrideSprite = offImage;
			musicManager.SetVolume (0);
		}
	}
}
