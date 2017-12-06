using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIManager {
	public static Card SelectCardToPlay(List<Card> AIhand, List<Card> discardAI, List<Card> discardPlayer){
		// In the meantime, return a random card from the hand. Later, a real UI should be added.
		return AIhand [Random.Range (0, AIhand.Count)];
	}
}
