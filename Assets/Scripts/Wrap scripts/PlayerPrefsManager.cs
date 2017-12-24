using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager {

	// General settings
	const string ANIMATION_SPEED_KEY = "animation_speed";
	const string AI_LEVEL_KEY = "AI_level";
	const string MARK_ACTIONS_ON_CARDS_KEY = "mark_actions_on_cards";

	// Sound settings
	const string MUSIC_VOLUME_KEY = "master_volume";
	const string EFFECTS_VOLUME_KEY = "effects_volume";

	// Defaults, when no prefs are saved yet
	private const int DEFAULT_ANIMATION_SPEED = 2; // 0 = slow, 1 = medium, 2 = fast, 3 = no animation
	private const string DEFAULT_AI_LEVEL = "Easy";
	private const int DEFAULT_MARK_ACTIONS = 1; // 0 = false, 1 = true (no bools in playerprefs)
	private const float DEFAULT_MUSIC_VOLUME = 0.8f;
	private const float DEFAULT_EFFECTS_VOLUME = 0.8f;

	// Animation Speed
	public static void SetAnimationSpeed(int animationSpeed){
		if (animationSpeed >= 0 && animationSpeed <= 3) {
			PlayerPrefs.SetInt (ANIMATION_SPEED_KEY, animationSpeed);
		}
		else {
			Debug.LogError ("Animation speed out of range");
		}
	}

	public static float GetAnimationSpeed(){
		if (PlayerPrefs.HasKey(ANIMATION_SPEED_KEY)){
			return PlayerPrefs.GetInt (ANIMATION_SPEED_KEY);	
		} else {
			return DEFAULT_ANIMATION_SPEED;
		}

	}

	// AI Level
	public static void SetAILevel(string AILevel){
		PlayerPrefs.SetString (AI_LEVEL_KEY, AILevel);
	}

	public static string GetAILevel(){
		if (PlayerPrefs.HasKey(AI_LEVEL_KEY)){
			return PlayerPrefs.GetString (AI_LEVEL_KEY);	
		} else {
			return DEFAULT_AI_LEVEL;
		}

	}

	// Mark Actions
	public static void SetMarkActionsOnCard(int markActions){
		if (markActions >= 0 && markActions <= 1) {
			PlayerPrefs.SetInt (MARK_ACTIONS_ON_CARDS_KEY, markActions);
		}
		else {
			Debug.LogError ("Mark Actions out of range");
		}
	}

	public static float GetMarkActionsOnCard(){
		if (PlayerPrefs.HasKey(MARK_ACTIONS_ON_CARDS_KEY)){
			return PlayerPrefs.GetInt (MARK_ACTIONS_ON_CARDS_KEY);	
		} else {
			return DEFAULT_MARK_ACTIONS;
		}

	}

	// Music Volume
	public static void SetMusicVolume(float volume){
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat (MUSIC_VOLUME_KEY, volume);
		}
		else {
			Debug.LogError ("Music volume out of range");
		}
	}

	public static float GetMusicVolume(){
		if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY)){
			return PlayerPrefs.GetFloat (MUSIC_VOLUME_KEY);	
		} else {
			return DEFAULT_MUSIC_VOLUME;
		}
	}

	// Effects Volume
	public static void SetEffectsVolume(float volume){
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat (EFFECTS_VOLUME_KEY, volume);
		}
		else {
			Debug.LogError ("Effects out of range");
		}
	}

	public static float GetEffectsVolume(){
		if (PlayerPrefs.HasKey(EFFECTS_VOLUME_KEY)){
			return PlayerPrefs.GetFloat (EFFECTS_VOLUME_KEY);	
		} else {
			return DEFAULT_EFFECTS_VOLUME;
		}

	}
}
