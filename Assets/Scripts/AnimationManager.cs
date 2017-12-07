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
	GameObject playedObjectA;
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
		playedObjectA = GameObject.FindGameObjectWithTag ("Played A");
		playedObjectB = GameObject.FindGameObjectWithTag ("Played B");
	}

	public void DrawToHand (List<Card> hand, GameManager.Player player){
		foreach (Card playingcard in hand) {
			if (player == GameManager.Player.A) {
				playingcard.transform.SetParent (handObjectA.transform);
				playingcard.ShowFront ();
			}
			else{
				playingcard.transform.SetParent (handObjectB.transform);
				playingcard.ShowBack ();
			}
		}
	}

	public void ParentAllCardsToDeckObject(GameManager.Player shufflingPlayer, bool excludeRankOne){
		if (shufflingPlayer == GameManager.Player.A) {
			// Move all deck A cards back to the deck
			MoveToDeck (handObjectA, deckObjectA);
			MoveToDeck (discardObjectA, deckObjectA);
			MoveToDeck (playedObjectA, deckObjectA);
			if (excludeRankOne){
				GameObject cardOne = GameObject.Find("Card_1A");
				cardOne.transform.SetParent (playedObjectA.transform);
				cardOne.GetComponent<Card> ().ShowFront ();
			}
		} else if (shufflingPlayer == GameManager.Player.B) {
			// Move all deck B cards back to the deck
			MoveToDeck (handObjectB, deckObjectB);
			MoveToDeck (discardObjectB, deckObjectB);
			MoveToDeck (playedObjectB, deckObjectB);
			if (excludeRankOne){
				GameObject cardOne = GameObject.Find("Card_1B");
				cardOne.transform.SetParent (playedObjectB.transform);
				cardOne.GetComponent<Card> ().ShowFront ();
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate (deckObjectA.GetComponent<RectTransform>());
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
