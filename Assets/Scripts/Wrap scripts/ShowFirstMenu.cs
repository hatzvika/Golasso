using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFirstMenu : MonoBehaviour {

	void Start () {
		string menuToShow = FirstMenuManager.GetFirstMenuName ();
		foreach (Transform child in transform){
			if (child.name == menuToShow){
				child.gameObject.SetActive (true);
			} else {
				child.gameObject.SetActive (false);
			}
		}

		// Return to showing first the main interface after a win/draw/lose screen was shown first
		FirstMenuManager.SetFirstMenuName ("Main Interface");
	}
	
}
