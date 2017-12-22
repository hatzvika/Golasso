using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchOver : MonoBehaviour {

	public Text TeamAScore;
	public Text TeamBScore;

	public Sprite winSprite;
	public Sprite drawSprite;
	public Sprite defeatSprite;

	public Image resultHeader;
	public Text resultDescription;
	public Text resultComment;

	private int scoreA;
	private int scoreB;

	void Start () {
		scoreA = ScoreManager.GetScoreA ();
		scoreB = ScoreManager.GetScoreB ();

		if (scoreA > scoreB){
			resultHeader.sprite = winSprite;
			resultComment.text = "CONGRATULATIONS!";
			resultDescription.text = "You won the game";
		} else if (scoreA == scoreB){
			resultHeader.sprite = drawSprite;
			resultComment.text = "THE GAME ENDED IN A DRAW!";
			resultDescription.text = "No team could best its rival";
		} else {
			resultHeader.sprite = defeatSprite;
			resultComment.text = "You LOST";
			resultDescription.text = "Maybe next time your team will do better!";
		}

		TeamAScore.text = scoreA.ToString();			
		TeamBScore.text = scoreB.ToString();			
	}
}
