using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

	public enum BallPosition{
		PlayerAField = 0,
		Middle = 1,
		PlayerBField = 2
	} 

	private GameManager.Player controllingTeam;
	private BallPosition ballPosition;

	public Sprite teamASprite;
	public Sprite teamBSprite;

	public void SetBallPosition(BallPosition newBallPosition){
		ballPosition = newBallPosition;
		float newAnchoredYPos;
		if (ballPosition == BallPosition.PlayerAField) {
			newAnchoredYPos = -280f;
		} else if (ballPosition == BallPosition.Middle) {
			newAnchoredYPos = 280f;
		}
		else{
			newAnchoredYPos = 0f;
		}
		GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0.5f, newAnchoredYPos);
	}

	public void SetControllingTeam(GameManager.Player newControllingTeam){
		controllingTeam = newControllingTeam;
		if (controllingTeam == GameManager.Player.A){
			GetComponent<Image>().sprite = teamASprite;
		}
		else{
			GetComponent<Image>().sprite = teamBSprite;
		}
	}

	public void FlipBall(){
		if (controllingTeam == GameManager.Player.A) {
			SetControllingTeam (GameManager.Player.B);
		}
		else{
			SetControllingTeam (GameManager.Player.A);
		}
	}
}
