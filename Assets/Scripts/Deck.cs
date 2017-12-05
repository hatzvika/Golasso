using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck<T> {
	List<T> cards;
	List<T> discard;
	List<T> hand;

	public Deck(List<T> cards)
	{
		this.cards = cards;
		discard = new List<T>();
		hand = new List<T>();
	}

	// Return a list of drawn Cards from deck
	public List<T> Draw(int numberToDraw =  1)
	{
		if (numberToDraw  > cards.Count)
			numberToDraw = cards.Count;
		for (int i =0;i<numberToDraw;++i)
		{
			hand.Add(cards[0]);
			cards.RemoveAt(0);
		}
		//hand.Sort((x, y) => x.ToString().CompareTo(y.ToString()));
		return hand;
	}

	public void DiscardCard(T card)
	{
		discard.Insert(0, card);
		hand.Remove (card);
	}

	public void ShuffleDiscard()
	{
		foreach(T OneCard in discard)
		{
			cards.Add(OneCard);
		}
		discard.Clear();
		Shuffle();
	}

	public void ShuffleAll(){
		foreach(T OneCard in hand)
		{
			cards.Add(OneCard);
		}
		hand.Clear ();

		foreach(T OneCard in discard)
		{
			cards.Add(OneCard);
		}
		discard.Clear ();

		Shuffle ();

	}

	// Shuffle the Current Deck of cards - used after ShuflleDiscard and ShuflleAll
	public void Shuffle()
	{
		for (int i = cards.Count - 1; i > 0;--i)
		{
			int j = Random.Range(0, i + 1);
			T card = cards[j];
			cards[j] = cards[i];
			cards[i] = card;
		}
	}
}