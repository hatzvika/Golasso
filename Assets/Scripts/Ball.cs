﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

	public enum BallPosition{
		// The values are the actual anchoredPosition values for the ball when in named part of the field.
		PlayerAGoal = -370,
		PlayerAField = -280,
		Middle = 0,
		PlayerBField = 280,
		PlayerBGoal = 370
	} 

	private GameManager.Player controllingTeam;
	private BallPosition ballPosition;

	public Sprite teamASprite;
	public Sprite teamBSprite;

	public void SetBallPosition(BallPosition newBallPosition){
		ballPosition = newBallPosition;
		float newAnchoredYPos = (float) ballPosition;

		GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0f, newAnchoredYPos);
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

	// This function is an asynchronous coRoutine because all the animations need to finish before the code resumes
	public IEnumerator MoveBallAction (GameManager.Player movingPlayer){
		float moveTime = 2;
		float moveBy = 2.1f;

		// Move only if the controlling team is the one moving
		if (movingPlayer == controllingTeam){
			if (movingPlayer == GameManager.Player.A){
				if (ballPosition == BallPosition.PlayerAField){
					iTween.MoveBy(gameObject, iTween.Hash("y" , moveBy, "time", moveTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.Middle;
					yield return new WaitForSeconds (moveTime);
				} else if (ballPosition == BallPosition.Middle){
					iTween.MoveBy(gameObject, iTween.Hash("y" , moveBy, "time", moveTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.PlayerBField;
					yield return new WaitForSeconds (moveTime);
				} else if (ballPosition == BallPosition.PlayerBField){
					iTween.MoveBy(gameObject, iTween.Hash("y" , moveBy/2, "time", moveTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.PlayerBGoal;
					yield return new WaitForSeconds (moveTime);
				}
			}
			else{  // movingPlayer == PlayerB
				if (ballPosition == BallPosition.PlayerBField){
					iTween.MoveBy(gameObject, iTween.Hash("y" , -moveBy, "time", moveTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.Middle;
					yield return new WaitForSeconds (moveTime);
				} else if (ballPosition == BallPosition.Middle){
					iTween.MoveBy(gameObject, iTween.Hash("y" , -moveBy, "time", moveTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.PlayerAField;
					yield return new WaitForSeconds (moveTime);
				} else if (ballPosition == BallPosition.PlayerAField){
					iTween.MoveBy(gameObject, iTween.Hash("y" , -(moveBy/2), "time", moveTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.PlayerAGoal;
					yield return new WaitForSeconds (moveTime);
				}
			}
		} else{
			SetControllingTeam(movingPlayer);
		}
	}

	public bool GoalScored (){
		if (ballPosition == BallPosition.PlayerAGoal || ballPosition == BallPosition.PlayerBGoal){
			return true;
		} else{
			return false;
		}
	}
}
