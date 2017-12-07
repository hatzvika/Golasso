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

	public void GoalScored(GameManager.Player scoringPlayer){
		if (scoringPlayer == GameManager.Player.A){
			scoreA++;
		} else{
			scoreB++;
		}

		updateScoreTexts ();
	}

	private void updateScoreTexts(){
		textScoreA.text = scoreA.ToString ();
		textScoreB.text = scoreB.ToString ();
	}

	public int GetScoreA(){
		return scoreA;
	}

	public int GetScoreB(){
		return scoreB;
	}
}
