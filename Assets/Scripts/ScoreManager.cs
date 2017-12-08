using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ScoreManager{

	private static int scoreA, scoreB;

	public static void ResetScore(){
		scoreA = 0;
		scoreB = 0;
	}

	public static void GoalScored(GameManager.Player scoringPlayer){
		if (scoringPlayer == GameManager.Player.A){
			scoreA++;
		} else{
			scoreB++;
		}
	}

	public static int GetScoreA(){
		return scoreA;
	}

	public static int GetScoreB(){
		return scoreB;
	}
}
