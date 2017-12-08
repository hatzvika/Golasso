using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

	private Text textScoreA, textScoreB;

	void Start () {
		textScoreA = GameObject.FindGameObjectWithTag ("Team A Score").GetComponent<Text>();
		textScoreB = GameObject.FindGameObjectWithTag ("Team B Score").GetComponent<Text>();
		updateScoreTexts (); // This line is for the win/draw/lose screens
	}

	public void updateScoreTexts(){
		int scoreA = ScoreManager.GetScoreA ();
		int scoreB = ScoreManager.GetScoreB ();

		textScoreA.text = scoreA.ToString ();
		textScoreB.text = scoreB.ToString ();
	}
}
