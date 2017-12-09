using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour {

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

		MoveToDeck (handObject, deckObject);
		MoveToDeck (discardObject, deckObject);
		yield return new WaitUntil(() => playerAreas.transform.Find("Discard").childCount == 0);
	}	

	public IEnumerator ParentDiscardCardsToDeckObject(GameObject playerAreas){
		GameObject deckObject = playerAreas.transform.Find ("Deck").gameObject;
		GameObject discardObject = playerAreas.transform.Find ("Discard").gameObject;

		yield return MoveToDeckAnimation (discardObject, deckObject);
		yield return new WaitUntil(() => discardObject.transform.childCount == 0);
	}	

	private IEnumerator MoveToDeckAnimation (GameObject originialObject, GameObject destinationObject)
	{
		Card[] cardList = originialObject.GetComponentsInChildren<Card> ();
		foreach (Card playingCard in cardList) {
			Hashtable param = new Hashtable ();
			param.Add ("Card", playingCard);
			param.Add ("parentToSet", destinationObject);
			param.Add ("showFace", false);

			Vector3 deckPos = destinationObject.transform.position;

			iTween.MoveTo (
				playingCard.gameObject, 
				iTween.Hash ("x", deckPos.x,
					"y", deckPos.y + 0.5, 
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

	public void MoveSelectedAICardToPlayArea(Card selectedCard, GameObject playerBAreas){
		GameObject playedObjectAI = playerBAreas.transform.Find ("Played Card").gameObject;

		selectedCard.transform.SetParent (playedObjectAI.transform);
		selectedCard.ShowFront ();
	}

	public void MovePlayedCardsToDiscard(Card playerCard, Card AICard, GameObject playerAAreas, GameObject playerBAreas){
		playerCard.transform.SetParent (playerAAreas.transform.Find("Discard").transform);
		AICard.transform.SetParent (playerBAreas.transform.Find("Discard").transform);
	}
}
