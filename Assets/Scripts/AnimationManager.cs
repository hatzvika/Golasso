using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour {

	// These are used to animate the cards to their exact discard cell in the discard grid layout group
	Vector2[] offsetsDiscardA = new Vector2[9];
	Vector2[] offsetsDiscardB = new Vector2[9];

	void Start(){
		// For the discard move animations
		CalculateOffsetDiscards ();
	}

	public IEnumerator DrawToHand (List<Card> hand, GameObject playerAreas, GameManager.Player player){
		
		GameObject deckObject = playerAreas.transform.Find ("Deck").gameObject;
		GameObject handObject = playerAreas.transform.Find ("Hand").gameObject;

		foreach (Card playingCard in hand) {

			// Only draw cards which are still in the deck
			if (playingCard.transform.parent == deckObject.transform) {
				
				// Prepare param hashtable for calling SetCardParentAfterAnimation when iTween finishes
				Hashtable param = new Hashtable ();
				param.Add ("Card", playingCard);
				param.Add ("parentToSet", handObject);
				if (player == GameManager.Player.A) {
					param.Add ("showFace", true);
				} else {
					param.Add ("showFace", false);
				}

				Vector3 handPos = handObject.transform.position;

				iTween.MoveTo (
					playingCard.gameObject, 
					iTween.Hash ("x", handPos.x + 1.2,
						"y", handPos.y + 0.8, 
						"time", 0.5,
						"easeType", "easeInOutQuad",
						"oncomplete", "SetCardParentAfterAnimation",
						"oncompletetarget", gameObject,
						"oncompleteparams", param));
				yield return new WaitForSeconds (0.2f);
			}
		}
	}

	private void SetCardParentAfterAnimation(object ht){
		Hashtable param = (Hashtable)ht;
		Card playingCard = (Card)param ["Card"];
		GameObject parentToSet = (GameObject)param ["parentToSet"];
		bool showFace = (bool)param ["showFace"];

		playingCard.transform.SetParent (parentToSet.transform);
		if (showFace) {
			playingCard.ShowFront ();
		} else {
			playingCard.ShowBack ();
		}
	}

	public IEnumerator ParentAllCardsToDeckObject(GameObject playerAreas){
		// Move all cards back to the deck
		GameObject deckObject = playerAreas.transform.Find ("Deck").gameObject;
		GameObject handObject = playerAreas.transform.Find ("Hand").gameObject;
		GameObject discardObject = playerAreas.transform.Find ("Discard").gameObject;
		GameObject playedCardObject = playerAreas.transform.Find ("Played Card").gameObject;

		MoveToDeck (handObject, deckObject);
		MoveToDeck (discardObject, deckObject);
		MoveToDeck (playedCardObject, deckObject);
		yield return new WaitUntil(() => ((discardObject.transform.childCount == 0) && (playedCardObject.transform.childCount == 0)));
	}	

	public IEnumerator ParentDiscardCardsToDeckObject(GameObject playerAreas){
		GameObject deckObject = playerAreas.transform.Find ("Deck").gameObject;
		GameObject discardObject = playerAreas.transform.Find ("Discard").gameObject;

		bool showFace = false;
		float xOffset = 0;
		float yOffset = 0.5f;
		yield return MoveCardsAnimation (discardObject, deckObject, showFace, xOffset, yOffset);
		yield return new WaitUntil(() => discardObject.transform.childCount == 0);
	}	

	private IEnumerator MoveCardsAnimation (GameObject originialObject, GameObject destinationObject, bool showFace, float xOffset, float yOffset)
	{
		Card[] cardList = originialObject.GetComponentsInChildren<Card> ();
		foreach (Card playingCard in cardList) {
			Hashtable param = new Hashtable ();
			param.Add ("Card", playingCard);
			param.Add ("parentToSet", destinationObject);
			param.Add ("showFace", showFace);

			Vector3 deckPos = destinationObject.transform.position;

			iTween.MoveTo (
				playingCard.gameObject, 
				iTween.Hash ("x", deckPos.x + xOffset,
					"y", deckPos.y + yOffset, 
					"time", 0.5,
					"easeType", "easeInOutQuad",
					"oncomplete", "SetCardParentAfterAnimation",
					"oncompletetarget", gameObject,
					"oncompleteparams", param));
			yield return new WaitForSeconds (0.2f);
		}
	}

	private void MoveToDeck(GameObject originialObject, GameObject destinationObject){
		Card[] cardList = originialObject.GetComponentsInChildren<Card> ();
		foreach (Card card in cardList) {
			card.ShowBack ();
			card.transform.SetParent (destinationObject.transform);
		}
	}

	public IEnumerator MoveSelectedAICardToPlayArea(Card selectedCard, GameObject playerBAreas){

		GameObject playedObjectAI = playerBAreas.transform.Find ("Played Card").gameObject;

		Hashtable param = new Hashtable ();
		param.Add ("Card", selectedCard);
		param.Add ("parentToSet", playedObjectAI);
		param.Add ("showFace", true);

		Vector3 deckPos = playedObjectAI.transform.position;

		iTween.MoveTo (
			selectedCard.gameObject, 
			iTween.Hash ("x", deckPos.x,
				"y", deckPos.y, 
				"time", 0.5,
				"easeType", "easeInOutQuad",
				"oncomplete", "SetCardParentAfterAnimation",
				"oncompletetarget", gameObject,
				"oncompleteparams", param));

		yield return new WaitUntil(() => playedObjectAI.transform.childCount == 1);
	}

	public IEnumerator MovePlayedCardsToDiscard(Card playerCard, Card AICard, GameObject playerAAreas, GameObject playerBAreas){
		GameObject discardObjectA = playerAAreas.transform.Find("Discard").gameObject;
		GameObject discardObjectB = playerBAreas.transform.Find ("Discard").gameObject;
		GameObject playedObjectA = playerAAreas.transform.Find("Played Card").gameObject;
		GameObject playedObjectB = playerBAreas.transform.Find ("Played Card").gameObject;

		bool showFace = true;

		int discardedCountA = discardObjectA.transform.childCount;
		int discardedCountB = discardObjectB.transform.childCount;

		// Do not discard the 10th card, as it is the end of a half or the game
		if (discardedCountA < 9 && discardedCountB < 9) {

			// Find the discard cell's offsets
			Vector2 offset = offsetsDiscardA [discardedCountA];
			float xOffset = offset.x;
			float yOffset = offset.y;

			// FIRST SCALE and only THEN MOVE. I don't know why, but switching won't work.
			// Scale card
			iTween.ScaleTo (
				playerCard.gameObject, 
				iTween.Hash ("x", 0.52f,
					"y", 0.52f, 
					"time", 0.5,
					"easeType", "easeInOutQuad"));
			// move card
			yield return MoveCardsAnimation (playedObjectA, discardObjectA, showFace, xOffset, yOffset);

			// FIRST SCALE and only THEN MOVE. I don't know why, but switching won't work.
			// Scale card
			iTween.ScaleTo (
				AICard.gameObject, 
				iTween.Hash ("x", 0.52f,
					"y", 0.52f, 
					"time", 0.5,
					"easeType", "easeInOutQuad"));

			offset = offsetsDiscardB [discardedCountB];
			xOffset = offset.x;
			yOffset = offset.y;
			// move card
			yield return MoveCardsAnimation (playedObjectB, discardObjectB, showFace, xOffset, yOffset);

			yield return new WaitUntil (() => playedObjectA.transform.childCount == 0);
			// Return the card to it's original scale in the new parent
			playerCard.transform.localScale = new Vector3 (1f, 1f, 1f);
			yield return new WaitUntil (() => playedObjectB.transform.childCount == 0);
			// Return the card to it's original scale in the new parent
			AICard.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
	}

	// These Vecto2 arrays are used to animate the cards to their exact discard cell in the discard grid layout group
	void CalculateOffsetDiscards ()
	{
		GameObject playerAAreas = GameObject.Find ("Player A Areas").gameObject;
		GameObject discardObjectA = playerAAreas.GetComponentInChildren<GridLayoutGroup> ().gameObject;

		// Calculate cell width and hegiht in discard grid layout group 
		Vector3[] corners = new Vector3[4];
		discardObjectA.GetComponent<RectTransform> ().GetWorldCorners (corners);
		float gridWidth = corners [2].x - corners [0].x;
		float gridHeight = corners [2].y - corners [0].y;
		float cellWidth = gridWidth / 3;
		float cellHeight = gridHeight / 3;

		// Offsets for the move card to discard animation for each cell in the layoutgroup.
		offsetsDiscardA [0] = new Vector2 (-cellWidth,	2.5f * cellHeight);
		offsetsDiscardA [1] = new Vector2 (0, 			2.5f * cellHeight);
		offsetsDiscardA [2] = new Vector2 (cellWidth, 	2.5f * cellHeight);
		offsetsDiscardA [3] = new Vector2 (-cellWidth, 	1.5f * cellHeight);
		offsetsDiscardA [4] = new Vector2 (0, 			1.5f * cellHeight);
		offsetsDiscardA [5] = new Vector2 (cellWidth, 	1.5f * cellHeight);
		offsetsDiscardA [6] = new Vector2 (-cellWidth, 	0.5f * cellHeight);
		offsetsDiscardA [7] = new Vector2 (0, 			0.5f * cellHeight);
		offsetsDiscardA [8] = new Vector2 (cellWidth, 	0.5f * cellHeight);

		offsetsDiscardB [0] = new Vector2 (-cellWidth, 	-0.5f * cellHeight);
		offsetsDiscardB [1] = new Vector2 (0, 			-0.5f * cellHeight);
		offsetsDiscardB [2] = new Vector2 (cellWidth, 	-0.5f * cellHeight);
		offsetsDiscardB [3] = new Vector2 (-cellWidth, 	-1.5f * cellHeight);
		offsetsDiscardB [4] = new Vector2 (0, 			-1.5f * cellHeight);
		offsetsDiscardB [5] = new Vector2 (cellWidth, 	-1.5f * cellHeight);
		offsetsDiscardB [6] = new Vector2 (-cellWidth, 	-2.5f * cellHeight);
		offsetsDiscardB [7] = new Vector2 (0, 			-2.5f * cellHeight);
		offsetsDiscardB [8] = new Vector2 (cellWidth, 	-2.5f * cellHeight);
	}
}
