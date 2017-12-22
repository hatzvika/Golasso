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

	public Sprite teamASprite;
	public Sprite teamBSprite;

	private GameManager.Player controllingTeam;
	private SettingsController settingController;
	private BallPosition ballPosition;

	void Start(){
		settingController = GameObject.FindObjectOfType<SettingsController> ();
	}

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

	public IEnumerator SetControllingTeamAnimation(GameManager.Player newControllingTeam, ActionMarker.ActionSprite action){
		float ballAnimationTime = settingController.GetAnimationSpeed ();

		ActionMarker actionMarker;
		if (newControllingTeam == GameManager.Player.A) {
			actionMarker = GameObject.Find ("Player A Areas").GetComponentInChildren<ActionMarker> ();
		}else{
			actionMarker = GameObject.Find ("Player B Areas").GetComponentInChildren<ActionMarker> ();
		}
		actionMarker.SetMarker (action);
		yield return new WaitForSeconds (ballAnimationTime / 2);
		controllingTeam = newControllingTeam;
		if (controllingTeam == GameManager.Player.A){
			GetComponent<Image>().sprite = teamASprite;
		}
		else{
			GetComponent<Image>().sprite = teamBSprite;
		}
		yield return new WaitForSeconds (ballAnimationTime / 2);
		actionMarker.RemoveMarker ();
	}

	// This function is an asynchronous coRoutine because all the animations need to finish before the code resumes
	public IEnumerator MoveBallAction (GameManager.Player movingPlayer, ActionMarker.ActionSprite action){
		float ballAnimationTime = settingController.GetAnimationSpeed ();
		float moveBy = 2.1f;

		// Move only if the controlling team is the one moving
		if (movingPlayer == controllingTeam){
			if (movingPlayer == GameManager.Player.A){
				ActionMarker actionMarker = GameObject.Find ("Player A Areas").GetComponentInChildren<ActionMarker> ();
				actionMarker.SetMarker (action);
				if (ballPosition == BallPosition.PlayerAField){
					iTween.MoveBy(gameObject, iTween.Hash("y" , moveBy, "time", ballAnimationTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.Middle;
					yield return new WaitForSeconds (ballAnimationTime);
				} else if (ballPosition == BallPosition.Middle){
					iTween.MoveBy(gameObject, iTween.Hash("y" , moveBy, "time", ballAnimationTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.PlayerBField;
					yield return new WaitForSeconds (ballAnimationTime);
				} else if (ballPosition == BallPosition.PlayerBField){
					iTween.MoveBy(gameObject, iTween.Hash("y" , moveBy/2, "time", ballAnimationTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.PlayerBGoal;
					yield return new WaitForSeconds (ballAnimationTime);
				}
				actionMarker.RemoveMarker ();
			}
			else{  // movingPlayer == PlayerB
				ActionMarker actionMarker = GameObject.Find ("Player B Areas").GetComponentInChildren<ActionMarker> ();
				actionMarker.SetMarker (action);
				if (ballPosition == BallPosition.PlayerBField){
					iTween.MoveBy(gameObject, iTween.Hash("y" , -moveBy, "time", ballAnimationTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.Middle;
					yield return new WaitForSeconds (ballAnimationTime);
				} else if (ballPosition == BallPosition.Middle){
					iTween.MoveBy(gameObject, iTween.Hash("y" , -moveBy, "time", ballAnimationTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.PlayerAField;
					yield return new WaitForSeconds (ballAnimationTime);
				} else if (ballPosition == BallPosition.PlayerAField){
					iTween.MoveBy(gameObject, iTween.Hash("y" , -(moveBy/2), "time", ballAnimationTime, "easeType","easeInOutQuad"));
					ballPosition = BallPosition.PlayerAGoal;
					yield return new WaitForSeconds (ballAnimationTime);
				}
				actionMarker.RemoveMarker ();
			}
		} else{
			yield return SetControllingTeamAnimation(movingPlayer, action);
		}
	}

	// This is used by GameManager.ResolveActions script, because once the
	// MoveBallAction function became coroutine, I couldn't return the goalScored boll.
	public bool GoalScored (){
		if (ballPosition == BallPosition.PlayerAGoal || ballPosition == BallPosition.PlayerBGoal){
			return true;
		} else{
			return false;
		}
	}
}
