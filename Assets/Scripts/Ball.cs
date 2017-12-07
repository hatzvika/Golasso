using System.Collections;
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

	public bool MoveBallAction (GameManager.Player movingPlayer){
		bool goal = false;
		// Move only if the controlling team is the one moving
		if (movingPlayer == controllingTeam){
			if (movingPlayer == GameManager.Player.A){
				if (ballPosition == BallPosition.PlayerAField){
					SetBallPosition (BallPosition.Middle);
				} else if (ballPosition == BallPosition.Middle){
					SetBallPosition (BallPosition.PlayerBField);
				} else if (ballPosition == BallPosition.PlayerBField){
					SetBallPosition (BallPosition.PlayerBGoal);
					goal = true;
				}
			}
			else{  // movingPlayer == PlayerB
				if (ballPosition == BallPosition.PlayerBField){
					SetBallPosition (BallPosition.Middle);
				} else if (ballPosition == BallPosition.Middle){
					SetBallPosition (BallPosition.PlayerAField);
				} else if (ballPosition == BallPosition.PlayerAField){
					SetBallPosition (BallPosition.PlayerAGoal);
					goal = true;
				}
			}
		} else{
			SetControllingTeam(movingPlayer);
		}
		return goal;
	}
}
