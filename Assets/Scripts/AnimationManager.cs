using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour {

	GameObject deckObjectA;
	GameObject deckObjectB;
	GameObject handObjectA;
	GameObject handObjectB;
	GameObject discardObjectA;
	GameObject discardObjectB;
	GameObject playedObjectB;

	void Start ()
	{
		// Find all the objects in which cards are parented during the game
		deckObjectA = GameObject.FindGameObjectWithTag ("Deck A");
		deckObjectB = GameObject.FindGameObjectWithTag ("Deck B");
		handObjectA = GameObject.FindGameObjectWithTag ("Hand A");
		handObjectB = GameObject.FindGameObjectWithTag ("Hand B");
		discardObjectA = GameObject.FindGameObjectWithTag ("Discard A");
		discardObjectB = GameObject.FindGameObjectWithTag ("Discard B");
		playedObjectB = GameObject.FindGameObjectWithTag ("Played B");
	}

	public IEnumerator DrawToHand (List<Card> hand, GameManager.Player player){
		foreach (Card playingCard in hand) {
			Hashtable param = new Hashtable();
			if (player == GameManager.Player.A) {
				if (playingCard.transform.parent == deckObjectA.transform) {
					param.Add ("Card", playingCard);
					param.Add ("parentToSet", handObjectA);
					param.Add ("showFace", true);

					Vector3 handPos = handObjectA.transform.position;

					//playingCard.transform.position = (new Vector2 (3f, -0.5f));
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
			else{
				if (playingCard.transform.parent == deckObjectB.transform) {
					param.Add ("Card", playingCard);
					param.Add ("parentToSet", handObjectB);
					param.Add ("showFace", false);

					Vector3 handPos = handObjectB.transform.position;

					//playingCard.transform.position = (new Vector2 (3f, 6.5f));
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
		yield return null;
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

	public void ParentAllCardsToDeckObject(){
		// Move all deck A cards back to the deck
		MoveToDeck (handObjectA, deckObjectA);
		MoveToDeck (discardObjectA, deckObjectA);
			
		// Move all deck B cards back to the deck
		MoveToDeck (handObjectB, deckObjectB);
		MoveToDeck (discardObjectB, deckObjectB);
	}	

	public IEnumerator ParentDiscardCardsToDeckObject(GameManager.Player shufflingPlayer){
		if (shufflingPlayer == GameManager.Player.A) {
			yield return MoveToDeckAnimation (discardObjectA, deckObjectA);
			yield return new WaitUntil(() => discardObjectA.transform.childCount == 0);
		} else if (shufflingPlayer == GameManager.Player.B) {
			yield return MoveToDeckAnimation (discardObjectB, deckObjectB);
			yield return new WaitUntil(() => discardObjectB.transform.childCount == 0);
		}

	}	

	private IEnumerator MoveToDeckAnimation (GameObject originialObject, GameObject destinationObject)
	{
		Card[] cardList = originialObject.GetComponentsInChildren<Card> ();
		foreach (Card playingCard in cardList) {
			Hashtable param = new Hashtable();
			if (playingCard.transform.parent == discardObjectA.transform) {
				param.Add ("Card", playingCard);
				param.Add ("parentToSet", deckObjectA);
				param.Add ("showFace", false);

				Vector3 deckPos = deckObjectA.transform.position;

				//playingCard.transform.position = (new Vector2 (3f, -0.5f));
				iTween.MoveTo (
					playingCard.gameObject, 
					iTween.Hash ("x", deckPos.x,
						"y", deckPos.y+0.5, 
						"time", 0.5,
						"easeType", "easeInOutQuad",
						"oncomplete", "SetCardParentAfterAnimation",
						"oncompletetarget", gameObject,
						"oncompleteparams", param));
				yield return new WaitForSeconds (0.2f);
			} else {
				if (playingCard.transform.parent == discardObjectB.transform) {
					param.Add ("Card", playingCard);
					param.Add ("parentToSet", deckObjectB);
					param.Add ("showFace", false);

					Vector3 deckPos = deckObjectB.transform.position;

					//playingCard.transform.position = (new Vector2 (3f, 6.5f));
					iTween.MoveTo (
						playingCard.gameObject, 
						iTween.Hash ("x", deckPos.x,
							"y", deckPos.y+0.5, 
							"time", 0.5,
							"easeType", "easeInOutQuad",
							"oncomplete", "SetCardParentAfterAnimation",
							"oncompletetarget", gameObject,
							"oncompleteparams", param));
					yield return new WaitForSeconds (0.2f);
				}
			}
		}
	}

	private void MoveToDeck(GameObject originialObject, GameObject destinationObject){
		Card[] cardList = originialObject.GetComponentsInChildren<Card> ();
		foreach (Card card in cardList) {
			card.ShowBack ();
			card.transform.SetParent (destinationObject.transform);
		}
	}

	public void MoveSelectedAICardToPlayArea(Card selectedCard){
		selectedCard.transform.SetParent (playedObjectB.transform);
		selectedCard.ShowFront ();
	}

	public void MovePlayedCardsToDiscard(Card playerCard, Card AICard){
		playerCard.transform.SetParent (discardObjectA.transform);
		AICard.transform.SetParent (discardObjectB.transform);
	}
}
