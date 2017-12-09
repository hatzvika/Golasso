using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalImage : MonoBehaviour {

	private float animationTime = 2;
	Image image;
	AudioSource audioSource;

	void Start () {
		image = GetComponent<Image> ();
		image.enabled = false;

		audioSource = GetComponent<AudioSource> ();
	}

	public IEnumerator AnimateGoalImage(){
		audioSource.Play ();

		image.enabled = true;
		iTween.ShakeScale (gameObject, new Vector3 (0.2f, 0.2f, 0f), animationTime);
		yield return new WaitForSeconds (animationTime);
		image.enabled = false;
	}
}
