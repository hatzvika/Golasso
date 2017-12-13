using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleImagesInGame : MonoBehaviour {

	public List<Sprite> titles;
	public List<AudioClip> audioClips;

	public enum TitleImages {
		FirstHalf = 0,
		SecondHalf = 1,
		Goal = 2
	}

	private float animationTime = 2;
	Image image;

	void Start () {
		image = GetComponent<Image> ();
		image.enabled = false;

	}

	public IEnumerator ShowTitleImage(TitleImages titleImage){

		//Play the titles' sound
		AudioSource audioSource = GetComponent<AudioSource> ();
		AudioOnOff audioOnOff = GameObject.FindObjectOfType<AudioOnOff> ();

		if (audioOnOff.GetAudioOn ()) {
			AudioClip audioClip = audioClips [(int)titleImage];
			audioSource.clip = audioClip;
			audioSource.Play ();
		}

		// Show and animate the title
		image.enabled = true;
		image.sprite = titles [(int)titleImage];

		if (titleImage == TitleImages.Goal) {
			iTween.ShakeScale (gameObject, new Vector3 (0.2f, 0.2f, 0f), animationTime);
		} else {
			iTween.ScaleTo (gameObject, iTween.Hash("x", 1.2f, "y", 1.2f, "time", animationTime/2, "easeType","easeInOutQuad"));
		}
		yield return new WaitForSeconds (animationTime);
		image.enabled = false;

	}
}
