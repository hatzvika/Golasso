using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalImage : MonoBehaviour {

	private float animationTime = 2;
	Image image;

	void Start () {
		image = GetComponent<Image> ();
		image.enabled = false;

	}

	public IEnumerator AnimateGoalImage(){
		AudioSource audioSource = GetComponent<AudioSource> ();
		AudioOnOff audioOnOff = GameObject.FindObjectOfType<AudioOnOff> ();

		if (audioOnOff.GetAudioOn ()) {
			audioSource.Play ();
		}

		image.enabled = true;
		iTween.ShakeScale (gameObject, new Vector3 (0.2f, 0.2f, 0f), animationTime);
		yield return new WaitForSeconds (animationTime);
		image.enabled = false;
	}
}
