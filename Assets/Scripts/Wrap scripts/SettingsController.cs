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
	private float animationSpeed;

	// Use this for initialization
	void Start () {
		musicManager = GameObject.FindObjectOfType<MusicManager> ();
		markActionsOnCards = markActionOnCardsToggle.isOn;
	}

	void OnEnable (){
		SwitchToButton (generalButton);
		AnimationSpeedChanged (); // Set the initial animationSpeed to match the slider.
	}
	
	public void MusicVolumeChanged () {
		musicManager.SetMusicVolume (musicVolumeSlider.value);
	}

	public void EffectsVolumeChanged () {
		musicManager.SetEffectsVolume(effectsVolumeSlider.value);
	}

	public void AnimationSpeedChanged () {
		if (animationSpeedSlider.value == 3){
			animationSpeedTooltip.text = "No Animation";
			animationSpeed = 0f;
		}else if (animationSpeedSlider.value == 2){
			animationSpeedTooltip.text = "Fast";
			animationSpeed = 0.7f;
		}else if (animationSpeedSlider.value == 1){
			animationSpeedTooltip.text = "Medium";
			animationSpeed = 1.4f;
		}else {
			animationSpeedTooltip.text = "Slow";
			animationSpeed = 2.0f;
		}
	}

	public float GetAnimationSpeed(){
		return animationSpeed;
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

	public bool GetMarkActionsOnCards (){
		return markActionsOnCards;
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
