using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public enum Player{
		A = 0,
		B = 1
	}

	public Card cardPrefab;
	public List<Sprite> frontSprites;
	public List<Sprite> backSprites;

	private Deck<Card> deckA;
	private Deck<Card> deckB;

	AnimationManager animationManager;
	ScoreManager scoreManager;
	LevelManager levelManager;
	Ball ball;

	Player startingPlayer;
	int currentHalf = 1;

	void Start ()
	{
		animationManager = GameObject.FindObjectOfType<AnimationManager>();
		scoreManager = GameObject.FindObjectOfType<ScoreManager>();
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		ball = GameObject.FindObjectOfType<Ball>();

		// Find the objects in which cards are parented during the game
		GameObject deckObjectA = GameObject.FindGameObjectWithTag ("Deck A");
		GameObject deckObjectB = GameObject.FindGameObjectWithTag ("Deck B");

		// Prepare the initial parent for the deck cards and initial both decks with their correct card backs
		deckA = CreateDeck (deckObjectA, Player.A);
		deckB = CreateDeck (deckObjectB, Player.B);
			
		ResetBoard (true); // reset for first half
	}

	public void ResetBoard (bool forFirstHalf)
	{
		// Disable the player's hand (if any) draggable script before shuffling
		HandDraggable (false);

		ShuffleAllCards ();

		// Make the player's hand draggable
		HandDraggable (true);

		// Reset the score to 0:0
		if (forFirstHalf) {
			scoreManager.ResetScore ();
			currentHalf = 1;
			PickStartingPlayer ();
		} else {
			if (startingPlayer == Player.A){
				startingPlayer = Player.B;
			} else {
				startingPlayer = Player.A;
			}
		}

		// Reset the ball to the middle of the field, controlled by starting player
		ball.SetControllingTeam (startingPlayer);
		ball.SetBallPosition (Ball.BallPosition.Middle);
	}

	void PickStartingPlayer ()
	{
		int nextFirstPlayer = Random.Range (0, 2);
		if (nextFirstPlayer == 0) {
			startingPlayer = Player.A;
		}
		else {
			startingPlayer = Player.B;
		}
	}

	// This function starts a game round after a player chose his card and played it to the drop area
	public void ExecuteRound(Card PlayerACard){
		
		// Disable dragging of cards in drop area and in hand
		DropArea dropArea = GameObject.FindObjectOfType<DropArea> ();
		dropArea.GetComponentInChildren<Draggable> ().enabled = false;
		HandDraggable (false);

		// AIManager selects a card
		Card selectedAICard = AIManager.SelectCardToPlay (deckB.GetHand(), deckB.GetDiscard (), deckA.GetDiscard());

		// AnimationManager moves the card to the played area
		animationManager.MoveSelectedAICardToPlayArea (selectedAICard);
		StartCoroutine(Waiter());

		// ActionManager reutrns 2 actions - first for higher rank and then for lower rank
		int RankA = PlayerACard.GetRank ();
		int RankB = selectedAICard.GetRank ();
		ActionManager.CardResolution[] actions = ActionManager.ComputeActions (RankA, RankB);

		// Resolve Actions
		ResolveActions (actions, RankA, RankB);

		// Move played cards to discard
		deckA.DiscardCard (PlayerACard);
		deckB.DiscardCard (selectedAICard);
		animationManager.MovePlayedCardsToDiscard (PlayerACard, selectedAICard);

		// Draw a new card, check for end and release dragging on hand
		if ((deckA.Draw (1).Count == 0) || (deckB.Draw (1).Count == 0)) {

			// End of half or game
			if (currentHalf == 1) {
				currentHalf++;
				ResetBoard (false); // Reset for second half
			} else {
				EndGame(); 
			}
		} else {
			animationManager.DrawToHand (deckA.GetHand (), Player.A);
			animationManager.DrawToHand (deckB.GetHand (), Player.B);
			HandDraggable (true);
		}

	}

	private void ResolveActions (ActionManager.CardResolution[] actions, int RankA, int RankB)
	{
		bool goalScored = false;

		// Get first and second player identity
		Player firstPlayer = Player.B;
		Player secondPlayer = Player.A;
		if (RankA >= RankB) {
			firstPlayer = Player.A;
			secondPlayer = Player.B;
		}

		// Resolve first action (can only be Move, Shuffle or DoNothing)
		ActionManager.CardResolution firstAction = actions [0];
		ActionManager.CardResolution secondAction = actions [1];

		if (firstAction == ActionManager.CardResolution.Move){					// Move forward or flip.
			goalScored = ball.MoveBallAction (firstPlayer);
			if (goalScored) {
				ScoreGoal (firstPlayer);
				StartCoroutine(Waiter());
				if (secondAction == ActionManager.CardResolution.Shuffle) {			// Flip
					ShuffleDiscard (secondPlayer);
				}
				return;
			}
		} else if (firstAction == ActionManager.CardResolution.Shuffle){		// Shuffle.
			ShuffleDiscard (firstPlayer);
		}
		StartCoroutine(Waiter());

		// Resolve second action (can be MoveAndMove, FlipAndMove, Move, Flip, Shuffle or DoNothing)
		if (secondAction == ActionManager.CardResolution.MoveAndMove) {			// Move and Move
			goalScored = ball.MoveBallAction (secondPlayer);
			if (goalScored) {
				ScoreGoal (secondPlayer);
				StartCoroutine(Waiter());
				return;
			}
			goalScored = ball.MoveBallAction (secondPlayer);
			if (goalScored) {
				ScoreGoal (secondPlayer);
			}
		} else if (secondAction == ActionManager.CardResolution.FlipAndMove) {	// Flip and Move
			ball.SetControllingTeam (secondPlayer);
			goalScored = ball.MoveBallAction (secondPlayer);
			if (goalScored) {
				ScoreGoal (secondPlayer);
			}
		} else if (secondAction == ActionManager.CardResolution.Move) {			// Move
			goalScored = ball.MoveBallAction (secondPlayer);
			if (goalScored) {
				ScoreGoal (secondPlayer);
			}
		} else if (secondAction == ActionManager.CardResolution.Flip) {			// Flip
			ball.SetControllingTeam (secondPlayer);
		} else if (secondAction == ActionManager.CardResolution.Shuffle) {			// Flip
			ShuffleDiscard (secondPlayer);
		}
		StartCoroutine(Waiter());
	}

	void ScoreGoal (Player scoringPlayer)
	{
		ball.SetBallPosition (Ball.BallPosition.Middle);
		scoreManager.GoalScored (scoringPlayer);
		if (scoringPlayer == Player.A) {
			ball.SetControllingTeam (Player.B);
		} else {
			ball.SetControllingTeam (Player.A);
		}
	}

	void ShuffleAllCards (){
		// Return the actual card objects to their deck object
		animationManager.ParentAllCardsToDeckObject ();

		deckA.ShuffleAll ();
		// Draw 3 cards for the player and set their parent to the player's hand object
		deckA.Draw (3);
		animationManager.DrawToHand (deckA.GetHand(), Player.A);

		deckB.ShuffleAll ();
		// Draw 3 cards for the player and set their parent to the player's hand object
		deckB.Draw (3);
		animationManager.DrawToHand (deckB.GetHand(), Player.B);
	}

	void ShuffleDiscard (Player shufflingPlayer){
		// Return the actual card objects to their deck object
		animationManager.ParentDiscardCardsToDeckObject (shufflingPlayer);

		if (shufflingPlayer == Player.A) {
			deckA.ShuffleDiscard ();
		} else { // Player B
			deckB.ShuffleDiscard ();
		}
	}

	private void HandDraggable(bool draggable){
		foreach (Card card in deckA.GetHand()) {
			card.GetComponent<Draggable> ().enabled = draggable;
		}
	}

	private void EndGame(){
		int scorePlayer = scoreManager.GetScoreA ();
		int scoreAI = scoreManager.GetScoreB ();

		if (scorePlayer > scoreAI){
			levelManager.LoadLevel ("Win");
		} else if (scorePlayer == scoreAI){
			levelManager.LoadLevel ("Draw");
		} else {
			levelManager.LoadLevel ("Lose");
		}
	}

	private Deck<Card> CreateDeck (GameObject deckParent, Player player)
	{
		List<Card> cards = new List<Card> ();

		// There are 10 cards in a deck
		for (int rank = 1; rank <= 10; ++rank) {
			Card newCard = Instantiate (cardPrefab, deckParent.transform);

			// Initialize Card parameters
			if (player == Player.A) {
				newCard.name = "Card_" + rank.ToString () + "A";
			} else{
				newCard.name = "Card_" + rank.ToString () + "B";
			}
			newCard.SetRank (rank);
			newCard.SetFrontCard (frontSprites [rank - 1]);
			if (player == Player.A){
				newCard.SetBackCard (backSprites[0]);
			}
			else{
				newCard.SetBackCard (backSprites[1]);
			}
			newCard.ShowBack ();

			// Set all cards to not draggable. Only cards which are drawn for Player A become draggable.
			newCard.EnableDraggable (false);

			// Add to deck
			cards.Add (newCard);
		}
		Deck<Card> deck = new Deck<Card> (cards);
		return deck;
	}

	IEnumerator Waiter(){
		yield return new WaitForSeconds(2f);
	}
}					