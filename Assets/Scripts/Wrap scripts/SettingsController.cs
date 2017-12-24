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
	private string AILevel;

	// Use this for initialization
	void Start () {
		musicManager = GameObject.FindObjectOfType<MusicManager> ();
		markActionsOnCards = markActionOnCardsToggle.isOn;

		// Load the settings values from the PlayerPrefsManager.cs
		musicVolumeSlider.value = PlayerPrefsManager.GetMusicVolume ();
		effectsVolumeSlider.value = PlayerPrefsManager.GetEffectsVolume ();
		animationSpeedSlider.value = PlayerPrefsManager.GetAnimationSpeed ();

		AILevel = PlayerPrefsManager.GetAILevel ();
		Toggle[] toggleList = AILevelToggleGroup.GetComponentsInChildren<Toggle> ();
		for (int toggleNum=0; toggleNum < toggleList.GetLength(0); toggleNum++){
			Toggle currentToggle = toggleList [toggleNum];
			if (currentToggle.GetComponentInChildren<Text>().text.ToString() == AILevel){
				toggleList[toggleNum].isOn = true;
			} else {
				toggleList[toggleNum].isOn = false;	
			}
		}

		if (PlayerPrefsManager.GetMarkActionsOnCard () == 1){
			markActionOnCardsToggle.isOn = true;
		} else{
			markActionOnCardsToggle.isOn = false;
		}
	}

	void OnEnable (){
		SwitchToButton (generalButton);
	}
	
	public void MusicVolumeChanged () {
		if (musicManager){
			musicManager.SetMusicVolume (musicVolumeSlider.value);	
		}
	}

	public void EffectsVolumeChanged () {
		if (musicManager) {
			musicManager.SetEffectsVolume (effectsVolumeSlider.value);
		}
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
		foreach (Toggle toggle in AILevelToggleGroup.ActiveToggles ()) {
			AILevel = toggle.GetComponentInChildren<Text> ().text;
		}

		if (AILevel == "Easy"){
			Debug.Log ("Easy Peasy");
		} else {
			Debug.Log ("Hardy");
		}
	}

	public string GetAILevel(){
		return AILevel;
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
		PlayerPrefsManager.SetMusicVolume (musicVolumeSlider.value);
		PlayerPrefsManager.SetEffectsVolume (effectsVolumeSlider.value);
		PlayerPrefsManager.SetAnimationSpeed ((int)animationSpeedSlider.value);
		PlayerPrefsManager.SetAILevel (AILevel);
		if (markActionOnCardsToggle.isOn) {
			PlayerPrefsManager.SetMarkActionsOnCard (1);
		} else {
			PlayerPrefsManager.SetMarkActionsOnCard (0);
		}
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
