using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
	Deck<Card> deck;
	public Card cardPrefab;
	public List<Sprite> frontSprites;
	public List<Sprite> backSprites;

	void Awake ()
	{
		List<Card> cards = new List<Card> ();
		GameObject deckA = GameObject.FindGameObjectWithTag ("Deck A");
			
		for (int rank = 1; rank <= 10; ++rank) {
			Card newCard = Instantiate (cardPrefab, deckA.transform);
			newCard.name = "Card_" + rank.ToString ();
			newCard.setRank (rank);
			newCard.GetComponent<Image> ().sprite = backSprites[0];
			cards.Add (newCard);
		}
		deck = new Deck<Card> (cards);
		deck.Shuffle ();

		List<Card> Hand = deck.Draw (3);
		GameObject handA = GameObject.FindGameObjectWithTag ("Game Canvas");
		foreach(Card playingcard in Hand){
			playingcard.transform.SetParent(GameObject.FindGameObjectWithTag("Hand A").transform);
			playingcard.GetComponent<Image> ().sprite = frontSprites[playingcard.getRank()-1];
		}
	}


}					