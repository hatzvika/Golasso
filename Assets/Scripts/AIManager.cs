using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIManager {

	// Following are the score matrices for card vs. card according to ball position and controlling player.
	// Each line is a card player A can play and each column is a card player B can play.
	// The value is the right row and column is the result for the combination.
	// Later, these values will be summed up and a adecision can be made.
	// Card number plusOne is special and will have a different set of rules.
	private const int noChange = 0;
	private const int plusOne = 1;
	private const int plusTwo = 2;
	private const int minusOne = -1;
	private const int minusTwo = -2;
	private const int scoreGoal = 5;
	private const int concede = -5;

	// Case plusOne - AI Player controlls the ball, AI Player field or middle field
	private static int[] CardA_2_Case_1 = {	plusTwo,	noChange,	noChange,	noChange,	noChange,	noChange,	noChange,	noChange,	noChange,	noChange };
	private static int[] CardA_3_Case_1 = {	plusTwo,	plusOne,	noChange,	noChange,	noChange,	noChange,	noChange,	noChange,	plusTwo,	plusTwo };
	private static int[] CardA_4_Case_1 = {	plusTwo,	plusOne,	plusOne,	noChange,	plusTwo,	plusTwo,	plusTwo,	plusTwo,	plusTwo,	plusTwo };
	private static int[] CardA_5_Case_1 = {	plusTwo,	plusOne,	plusOne, 	minusOne,	noChange,	plusTwo,	plusTwo,	plusTwo,	plusTwo,	plusTwo };
	private static int[] CardA_6_Case_1 = {	plusTwo,	plusOne,	plusOne, 	minusOne, 	minusOne,	noChange, 	minusOne, 	minusOne, 	minusOne, 	minusOne };
	private static int[] CardA_7_Case_1 = {	noChange,	plusOne,	plusOne, 	minusOne, 	minusOne,	plusTwo,  	noChange, 	minusOne, 	minusOne, 	minusOne };
	private static int[] CardA_8_Case_1 = {	noChange, 	minusOne,	plusOne, 	minusOne, 	minusOne,	plusTwo,  	plusTwo,  	noChange, 	minusOne, 	minusOne };
	private static int[] CardA_9_Case_1 = {	noChange, 	minusOne, 	minusOne, 	minusOne,	minusOne,	plusTwo,  	plusTwo,	plusTwo,	noChange, 	minusOne };
	private static int[] CardA_10_Case_1= {	noChange, 	minusOne, 	minusOne, 	minusOne, 	minusOne,	plusTwo,  	plusTwo,	plusTwo,	plusTwo, 	noChange };

	// Case plusTwo - AI Player controlls the ball, human Player field
	private static int[] CardA_2_Case_2 = {	scoreGoal,	noChange,	noChange,	noChange,	noChange,	noChange,	noChange,	noChange,	noChange,	noChange };
	private static int[] CardA_3_Case_2 = {	scoreGoal,	scoreGoal,	noChange,	noChange,	noChange,	noChange,	noChange,	noChange,	scoreGoal,	scoreGoal };
	private static int[] CardA_4_Case_2 = {	scoreGoal,	scoreGoal,	scoreGoal,	noChange,	scoreGoal,	scoreGoal,	scoreGoal,	scoreGoal,	scoreGoal,	scoreGoal };
	private static int[] CardA_5_Case_2 = {	scoreGoal,	scoreGoal,	scoreGoal, 	minusOne,	noChange,	scoreGoal,	scoreGoal,	scoreGoal,	scoreGoal,	scoreGoal };
	private static int[] CardA_6_Case_2 = {	scoreGoal,	scoreGoal,	scoreGoal, 	minusOne, 	minusOne,	noChange, 	minusOne, 	minusOne, 	minusOne, 	minusOne };
	private static int[] CardA_7_Case_2 = {	noChange,	scoreGoal,	scoreGoal, 	minusOne, 	minusOne,	scoreGoal,  noChange, 	minusOne, 	minusOne, 	minusOne };
	private static int[] CardA_8_Case_2 = {	noChange, 	minusOne,	scoreGoal, 	minusOne, 	minusOne,	scoreGoal,  scoreGoal,  noChange, 	minusOne, 	minusOne };
	private static int[] CardA_9_Case_2 = {	noChange, 	minusOne, 	minusOne, 	minusOne, 	minusOne,	scoreGoal,  scoreGoal,	scoreGoal,	noChange, 	minusOne };
	private static int[] CardA_10_Case_2= {	noChange, 	minusOne, 	minusOne, 	minusOne, 	minusOne,	scoreGoal,  scoreGoal,	scoreGoal,	scoreGoal, 	noChange };

	// Case 3 - Human player controlls the ball, human player field or middle field
	private static int[] CardA_2_Case_3 = {	plusOne,	noChange, 	minusOne, 	minusOne, 	minusOne, 	minusOne, 	minusOne,	plusOne,	plusOne,	plusOne };
	private static int[] CardA_3_Case_3 = {	plusOne,	noChange,	noChange, 	minusOne, 	minusOne, 	minusOne, 	minusOne, 	minusOne,	plusOne,	plusOne };
	private static int[] CardA_4_Case_3 = {	plusOne,	noChange,	noChange,	noChange,	plusOne,	plusOne,	plusOne,	plusOne,	plusOne,	plusTwo };
	private static int[] CardA_5_Case_3 = {	plusOne,	noChange,	noChange, 	minusTwo,	noChange,	plusOne,	plusOne,	plusOne,	plusOne,	plusOne };
	private static int[] CardA_6_Case_3 = {	plusOne,	noChange,	noChange, 	minusTwo, 	minusTwo,	noChange, 	minusTwo, 	minusTwo, 	minusTwo, 	minusTwo };
	private static int[] CardA_7_Case_3 = {	noChange,	noChange,	noChange, 	minusTwo, 	minusTwo,	plusOne,  	noChange, 	minusTwo, 	minusTwo, 	minusTwo };
	private static int[] CardA_8_Case_3 = {	noChange, 	noChange,	noChange, 	minusTwo, 	minusTwo,	plusOne,  	plusOne,  	noChange, 	minusTwo, 	minusTwo };
	private static int[] CardA_9_Case_3 = {	noChange, 	noChange, 	minusTwo, 	minusTwo, 	minusTwo,	plusOne,  	plusOne,	plusOne,	noChange, 	minusTwo };
	private static int[] CardA_10_Case_3= {	noChange, 	noChange, 	minusTwo, 	minusTwo, 	minusTwo,	plusOne,  	plusOne,	plusOne,	plusOne, 	noChange };

	// Case 4 - Human player controlls the ball, AI Player field
	private static int[] CardA_2_Case_4 = {	plusOne,	noChange, 	concede, 	concede, 	concede, 	concede, 	concede,	plusOne,	plusOne,	plusOne };
	private static int[] CardA_3_Case_4 = {	plusOne,	noChange,	noChange, 	concede, 	concede, 	concede, 	concede, 	concede,	plusOne,	plusOne };
	private static int[] CardA_4_Case_4 = {	plusOne,	noChange,	noChange,	noChange,	plusOne,	plusOne,	plusOne,	plusOne,	plusOne,	plusTwo };
	private static int[] CardA_5_Case_4 = {	plusOne,	noChange,	noChange, 	concede,	noChange,	plusOne,	plusOne,	plusOne,	plusOne,	plusOne };
	private static int[] CardA_6_Case_4 = {	plusOne,	noChange,	noChange, 	concede, 	concede,	noChange, 	concede, 	concede, 	concede, 	concede };
	private static int[] CardA_7_Case_4 = {	noChange,	noChange,	noChange, 	concede, 	concede,	plusOne,  	noChange, 	concede, 	concede, 	concede };
	private static int[] CardA_8_Case_4 = {	noChange, 	noChange,	noChange, 	concede, 	concede,	plusOne,  	plusOne,  	noChange, 	concede, 	concede };
	private static int[] CardA_9_Case_4 = {	noChange, 	noChange, 	concede, 	concede, 	concede,	plusOne,  	plusOne,	plusOne,	noChange, 	concede };
	private static int[] CardA_10_Case_4= {	noChange, 	noChange, 	concede, 	concede, 	concede,	plusOne,  	plusOne,	plusOne,	plusOne, 	noChange };

	private static int[][] case1 = {
		CardA_2_Case_1,
		CardA_3_Case_1,
		CardA_4_Case_1,
		CardA_5_Case_1,
		CardA_6_Case_1,
		CardA_7_Case_1,
		CardA_8_Case_1,
		CardA_9_Case_1,
		CardA_10_Case_1
	};

	private static int[][] case2 = {
		CardA_2_Case_2,
		CardA_3_Case_2,
		CardA_4_Case_2,
		CardA_5_Case_2,
		CardA_6_Case_2,
		CardA_7_Case_2,
		CardA_8_Case_2,
		CardA_9_Case_2,
		CardA_10_Case_2
	};

	private static int[][] case3 = {
		CardA_2_Case_3,
		CardA_3_Case_3,
		CardA_4_Case_3,
		CardA_5_Case_3,
		CardA_6_Case_3,
		CardA_7_Case_3,
		CardA_8_Case_3,
		CardA_9_Case_3,
		CardA_10_Case_3
	};

	private static int[][] case4 = {
		CardA_2_Case_4,
		CardA_3_Case_4,
		CardA_4_Case_4,
		CardA_5_Case_4,
		CardA_6_Case_4,
		CardA_7_Case_4,
		CardA_8_Case_4,
		CardA_9_Case_4,
		CardA_10_Case_4
	};

	public static Card SelectCardToPlay(string AILevel, List<Card> AIhand, List<Card> deckPlayer, GameManager.Player controllingPlayer, Ball.BallPosition ballPosition){
		if (AILevel == "Hard") {
			Card selectedCard = null;
			int cardEfficiency;
			int bestEfficiency = -1000;
			int[][] efficiencyTable = SelectEfficiencyTable (controllingPlayer, ballPosition);

			foreach (Card cardHand in AIhand){
				// Rank one is a special case
				if (cardHand.GetRank () == 1) {
					cardEfficiency = SelectRankOneEfficiency ();
				} else {
					cardEfficiency = CalculateEfficiency (cardHand, deckPlayer, efficiencyTable);
				}

				if (cardEfficiency > bestEfficiency) {
					bestEfficiency = cardEfficiency;
					selectedCard = cardHand;
				}
			}
			return selectedCard;
		} 

		// On easy mode return a random card from the hand
		return AIhand [Random.Range (0, AIhand.Count)];
	}

	private static int[][] SelectEfficiencyTable(GameManager.Player controllingPlayer, Ball.BallPosition ballPosition){
		if ((controllingPlayer == GameManager.Player.B) && (ballPosition != Ball.BallPosition.PlayerAField)){
			return case1;
		} else if ((controllingPlayer == GameManager.Player.B) && (ballPosition == Ball.BallPosition.PlayerAField)){
			return case2;
		} else if ((controllingPlayer == GameManager.Player.A) && (ballPosition != Ball.BallPosition.PlayerBField)){
			return case3;
		} else if ((controllingPlayer == GameManager.Player.A) && (ballPosition == Ball.BallPosition.PlayerBField)){
			return case4;
		} else {
			Debug.LogWarning ("No efficiency table was selected for he AI. Returning default table");
			return case1;
		}
	}

	private static int CalculateEfficiency (Card cardHand, List<Card> deckPlayer, int[][] efficiencyTable){
		int efficiency = 0;
		int cardHandRank = cardHand.GetRank ();
		foreach (Card cardDeck in deckPlayer){
			int cardDeckRank = cardDeck.GetRank ();
			efficiency = efficiency + case3 [cardHandRank - 2] [cardDeckRank - 1];
			//Debug.Log ("Hand Card: " + cardHandRank + ", Deck Card: " + cardDeckRank + ", Efficiency: " + case3 [cardHandRank - 2] [cardDeckRank - 1]);
		}
		//Debug.Log ("Hand Card: " + cardHandRank + ", Total Efficiency = " + efficiency);
		return efficiency;
	}

	private static int SelectRankOneEfficiency(){
		return 0;
	}
}
