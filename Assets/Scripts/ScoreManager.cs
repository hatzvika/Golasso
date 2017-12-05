using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	private Text textScoreA, textScoreB;
	private int scoreA, scoreB;

	void Start(){
		textScoreA = GameObject.FindGameObjectWithTag ("Team A Score").GetComponent<Text>();
		textScoreB = GameObject.FindGameObjectWithTag ("Team B Score").GetComponent<Text>();
	}

	public void ResetScore(){
		scoreA = 0;
		scoreB = 0;

		updateScoreTexts ();
	}

	private void updateScoreTexts(){
		textScoreA.text = scoreA.ToString ();
		textScoreB.text = scoreB.ToString ();
	}
}
