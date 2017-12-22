using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMarker : MonoBehaviour {

	public List<Sprite> actionMarker;

	public enum ActionSprite {
		Higher = 0,
		Lower = 1,
		LowerCancel = 2,
		Cancel = 3,
		Number = 4
	}

	private SettingsController settingController;
	private Image image;

	void Start(){
		settingController = GameObject.FindObjectOfType<SettingsController> ();
		image = GetComponentInChildren<Image> ();	
		image.enabled = false;
	}

	public void SetMarker (ActionSprite actionSprite){
		if (settingController.GetMarkActionsOnCards ()) {
			image.enabled = true;
			image.sprite = actionMarker [(int)actionSprite];
		}
	}
	public void RemoveMarker(){
		if (settingController.GetMarkActionsOnCards ()) {
			image.enabled = false;
		}
	}
}
