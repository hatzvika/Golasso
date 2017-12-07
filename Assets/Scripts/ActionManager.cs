using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionManager {

	public enum CardResolution{
		DoNothing = 0,
		Flip = 1,
		Move = 2,
		FlipAndMove = 3,
		MoveAndMove = 4,
		Shuffle = 5
	}

	public static CardResolution[] ComputeActions (int RankA, int RankB){
		CardResolution[] actions = new CardResolution[2];

		if ((RankA == RankB) && RankA == 1){		// Both cards are 1 - Both shuffle the discard back to the deck
			actions [0] = CardResolution.Shuffle;
			actions [1] = CardResolution.Shuffle;
		}
		else if (RankA == RankB) {					// Cards are equal - nothing happens
			actions [0] = CardResolution.DoNothing;
			actions [1] = CardResolution.DoNothing;
		}
		else if (Mathf.Abs(RankA - RankB) >= 6){	// Higher card is cancelled by lower card and does nothing
			actions [0] = CardResolution.DoNothing;
			actions [1] = ActionOfCard (Mathf.Min(RankA, RankB));
		}
		else {										// Higher card is not cancelled
			actions [0] = CardResolution.Move;
			actions [1] = ActionOfCard (Mathf.Min(RankA, RankB));
		}

		return actions;
	}

	private static CardResolution ActionOfCard (int cardRank){
		if (cardRank >= 6) {
			return CardResolution.DoNothing;
		}
		else if (cardRank == 5){
			return CardResolution.MoveAndMove;
		}
		else if (cardRank == 4){
			return CardResolution.FlipAndMove;
		}
		else if (cardRank == 3){
			return CardResolution.Move;
		}
		else if (cardRank == 2){
			return CardResolution.Flip;
		}
		else {  // cardRank == 1
			return CardResolution.Shuffle;
		}
	}
}
