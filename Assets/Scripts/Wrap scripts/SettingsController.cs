using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

	public Button generalButton;
	public Button soundButton;
	public GameObject generalWidgets;
	public GameObject soundWidgets;

	public Slider musicVolumeSlider;
	public Slider effectsVolumeSlider;
	public Slider animationSpeedSlider;
	public Text animationSpeedTooltip;
	public ToggleGroup AILevelToggleGroup;
	public Toggle markActionOnCardsToggle;

	public Sprite firstButtonOn;
	public Sprite firstButtonOff;
	public Sprite otherButtonsOn;
	public Sprite otherButtonsOff;

	private MusicManager musicManager;
	private bool markActionsOnCards;

	// Use this for initialization
	void Start () {
		musicManager = GameObject.FindObjectOfType<MusicManager> ();
		markActionsOnCards = markActionOnCardsToggle.isOn;
	}

	void OnEnable (){
		SwitchToButton (generalButton);
	}
	
	public void MusicVolumeChanged () {
		musicManager.SetVolume (musicVolumeSlider.value);
	}

	public void EffectsVolumeChanged () {
		Debug.Log (effectsVolumeSlider.value);
	}

	public void AnimationSpeedChanged () {
		if (animationSpeedSlider.value == 3){
			animationSpeedTooltip.text = "No Animation";
		}else if (animationSpeedSlider.value == 2){
			animationSpeedTooltip.text = "Fast";
		}else if (animationSpeedSlider.value == 1){
			animationSpeedTooltip.text = "Medium";
		}else {
			animationSpeedTooltip.text = "Slow";
		}
	}

	public void AILevelChanged(){
		string level = "";
		foreach (Toggle toggle in AILevelToggleGroup.ActiveToggles ()) {
			level = toggle.GetComponentInChildren<Text> ().text;
		}

		if (level == "Easy"){
			Debug.Log ("Easy Peasy");
		} else {
			Debug.Log ("Hardy");
		}
	}

	public void MarkOnCardsChanged(){
		markActionsOnCards = !markActionsOnCards;
		Text label = markActionOnCardsToggle.GetComponentInChildren<Text> ();
		if (markActionsOnCards){
			label.text = "Mark";
		} else{
			label.text = "Do Not Mark";
		}
	}

	public void SaveAndExit(){
		Debug.Log ("Saving to prefs manager");
		PlayerPrefsManager.SetMasterVolume (musicVolumeSlider.value);
		// levelManager.LoadLevel ("Start Screen");
	}

	public void SetDefaults(){
		musicVolumeSlider.value = 0.8f;
	}

	public void SwitchToButton (Button buttonToSwitchTo){
		if (buttonToSwitchTo == generalButton){
			generalButton.GetComponent<Image> ().sprite = firstButtonOn;
			soundButton.GetComponent<Image> ().sprite = otherButtonsOff;
			generalWidgets.SetActive (true);
			soundWidgets.SetActive (false);
		}else{
			generalButton.GetComponent<Image> ().sprite = firstButtonOff;
			soundButton.GetComponent<Image> ().sprite = otherButtonsOn;
			generalWidgets.SetActive (false);
			soundWidgets.SetActive (true);
		}
	}
}
