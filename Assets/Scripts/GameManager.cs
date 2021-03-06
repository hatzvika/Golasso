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

	private GameObject playerAAreas;
	private GameObject playerBAreas;

	private CardsAnimationManager cardsAnimationManager;
	private ScoreDisplay scoreDisplay;
	private LevelManager levelManager;
	private TitleImagesInGame titleImagesInGame;
	private SettingsController settingsController;
	private Ball ball;

	private Player startingPlayer;
	int currentHalf = 1;

	void Start ()
	{
		cardsAnimationManager = GameObject.FindObjectOfType<CardsAnimationManager>();
		scoreDisplay = GameObject.FindObjectOfType<ScoreDisplay>();
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		titleImagesInGame = GameObject.FindObjectOfType<TitleImagesInGame> ();
		settingsController = GameObject.FindObjectOfType<SettingsController> ();
		ball = GameObject.FindObjectOfType<Ball>();

		// Find the objects in which cards are parented during the game
		playerAAreas = GameObject.Find ("Player A Areas").gameObject;
		playerBAreas = GameObject.Find ("Player B Areas").gameObject;

		// Prepare the initial parent for the deck cards and initial both decks with their correct card backs
		deckA = CreateDeck (playerAAreas, Player.A);
		deckB = CreateDeck (playerBAreas, Player.B);
			
		StartCoroutine(ResetBoard (true)); // reset for first half
	}

	public IEnumerator ResetBoard (bool forFirstHalf)
	{
		// Disable the player's hand (if any) draggable script before shuffling
		HandDraggable (false);

		// Reset the score to 0:0
		if (forFirstHalf) {
			ScoreManager.ResetScore ();
			scoreDisplay.updateScoreTexts ();
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

		yield return (ShuffleAllCards ());

		// Show first/second half text
		if (forFirstHalf) {
			yield return titleImagesInGame.ShowTitleImage (TitleImagesInGame.TitleImages.FirstHalf);	
		} else {
			yield return titleImagesInGame.ShowTitleImage (TitleImagesInGame.TitleImages.SecondHalf);
		}

		// Make the player's hand draggable
		HandDraggable (true);

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
	// This function is an asynchronous coRoutine because all the animations need to finish before the code resumes
	public IEnumerator ExecuteRound(Card PlayerACard){
		
		// Disable dragging of cards in drop area and in hand
		DropArea dropArea = GameObject.FindObjectOfType<DropArea> ();
		dropArea.GetComponentInChildren<Draggable> ().enabled = false;
		HandDraggable (false);

		// AIManager selects a card
		// The AI needs both the player's hand and deck in a single list
		List<Card> deckAndHandPlayer = new List<Card>();
		deckAndHandPlayer.AddRange (deckA.GetCards ());
		deckAndHandPlayer.AddRange (deckA.GetHand ());
		Card selectedAICard = AIManager.SelectCardToPlay (settingsController.GetAILevel(), deckB.GetHand(), deckAndHandPlayer, ball.GetControllingTeam(), ball.GetBallPosition());

		// AnimationManager moves the card to the played area
		yield return cardsAnimationManager.MoveSelectedAICardToPlayArea (selectedAICard, playerBAreas);

		// ActionManager reutrns 2 actions - first for higher rank and then for lower rank
		int RankA = PlayerACard.GetRank ();
		int RankB = selectedAICard.GetRank ();
		ActionManager.CardResolution[] actions = ActionManager.ComputeActions (RankA, RankB);

		// Resolve Actions
		yield return ResolveActions (actions, RankA, RankB);

		// Move played cards to discard
		deckA.DiscardCard (PlayerACard);
		deckB.DiscardCard (selectedAICard);
		yield return cardsAnimationManager.MovePlayedCardsToDiscard (PlayerACard, selectedAICard, playerAAreas, playerBAreas);

		// Draw a new card (or more, in case of shuffling when there are less than 3 cards in hand), check for end and release dragging on hand
		int numToDrawA = 3 - deckA.GetHand ().Count;
		int numToDrawB = 3 - deckB.GetHand ().Count;
		if ((deckA.Draw (numToDrawA).Count == 0) || (deckB.Draw (numToDrawB).Count == 0)) {

			// End of half or game
			if (currentHalf == 1) {
				currentHalf++;
				yield return ResetBoard (false); // Reset for second half
			} else {
				EndGame(); 
			}
		} else {
			yield return cardsAnimationManager.DrawToHand (deckA.GetHand (), playerAAreas, Player.A);
			yield return cardsAnimationManager.DrawToHand (deckB.GetHand (), playerBAreas, Player.B);
			HandDraggable (true);
		}

	}

	// This function is an asynchronous coRoutine because all the animations need to finish before the code resumes
	private IEnumerator ResolveActions (ActionManager.CardResolution[] actions, int RankA, int RankB)
	{
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
			yield return ball.MoveBallAction (firstPlayer, ActionMarker.ActionSprite.Higher);
			if (ball.GoalScored()) {
				yield return ScoreGoal (firstPlayer);
				if (secondAction == ActionManager.CardResolution.Shuffle) {		// Shuffle Second if needed
					yield return ShuffleDiscard (secondPlayer, ActionMarker.ActionSprite.Lower);
				}
				yield break;
			}
		} else if (firstAction == ActionManager.CardResolution.Shuffle){		// Shuffle.
			yield return ShuffleDiscard (firstPlayer, ActionMarker.ActionSprite.Lower);
		} else if (RankA == RankB){												// Do nothing - same card
			yield return DoNothingAction (firstPlayer, secondPlayer, ActionMarker.ActionSprite.Number, ActionMarker.ActionSprite.Number);
			yield break; // So lower action do nothing wouldn't happen
		} else {																// Do nothing - higher cancelled
			yield return DoNothingAction (firstPlayer, secondPlayer, ActionMarker.ActionSprite.Cancel, ActionMarker.ActionSprite.LowerCancel);
		}

		// Resolve second action (can be MoveAndMove, FlipAndMove, Move, Flip, Shuffle or DoNothing)
		if (secondAction == ActionManager.CardResolution.MoveAndMove) {			// Move and Move
			yield return ball.MoveBallAction (secondPlayer, ActionMarker.ActionSprite.Lower);
			if (ball.GoalScored()) {
				yield return ScoreGoal (secondPlayer);
				yield break;
			}
			yield return ball.MoveBallAction (secondPlayer, ActionMarker.ActionSprite.Lower);
			if (ball.GoalScored()) {
				yield return ScoreGoal (secondPlayer);
				yield break;
			}
		} else if (secondAction == ActionManager.CardResolution.FlipAndMove) {	// Flip and Move
			//Flip
			yield return ball.SetControllingTeamAnimation (secondPlayer, ActionMarker.ActionSprite.Lower);
			//Move
			yield return ball.MoveBallAction (secondPlayer, ActionMarker.ActionSprite.Lower);
			if (ball.GoalScored()) {
				yield return ScoreGoal (secondPlayer);
			}
		} else if (secondAction == ActionManager.CardResolution.Move) {			// Move
			yield return ball.MoveBallAction (secondPlayer, ActionMarker.ActionSprite.Lower);
			if (ball.GoalScored()) {
				yield return ScoreGoal (secondPlayer);
			}
		} else if (secondAction == ActionManager.CardResolution.Flip) {			// Flip
			yield return ball.SetControllingTeamAnimation (secondPlayer, ActionMarker.ActionSprite.Lower);
		} else if (secondAction == ActionManager.CardResolution.Shuffle) {		// Shuffle
			yield return ShuffleDiscard (secondPlayer, ActionMarker.ActionSprite.Lower);
		} else {																// Do nothing - lower card doesn't have an action
			yield return DoNothingAction (firstPlayer, secondPlayer, ActionMarker.ActionSprite.Lower, ActionMarker.ActionSprite.Lower);
		}
	}

	private IEnumerator ScoreGoal (Player scoringPlayer)
	{
		// Updtae score
		ScoreManager.GoalScored (scoringPlayer);
		scoreDisplay.updateScoreTexts ();

		// Animate the goalasso! image
		yield return titleImagesInGame.ShowTitleImage(TitleImagesInGame.TitleImages.Goal);

		// Move the ball to the center and switch controlling team
		ball.SetBallPosition (Ball.BallPosition.Middle);
		if (scoringPlayer == Player.A) {
			ball.SetControllingTeam (Player.B);
		} else {
			ball.SetControllingTeam (Player.A);
		}
	}

	IEnumerator ShuffleAllCards (){
		// Return the actual card objects to their deck object
		yield return cardsAnimationManager.ParentAllCardsToDeckObject (playerAAreas);
		yield return cardsAnimationManager.ParentAllCardsToDeckObject (playerBAreas);

		deckA.ShuffleAll ();
		// Draw 3 cards for the player and set their parent to the player's hand object
		deckA.Draw (3);
		yield return (cardsAnimationManager.DrawToHand (deckA.GetHand(), playerAAreas, Player.A));

		deckB.ShuffleAll ();
		// Draw 3 cards for the player and set their parent to the player's hand object
		deckB.Draw (3);
		yield return (cardsAnimationManager.DrawToHand (deckB.GetHand(), playerBAreas, Player.B));
	}

	IEnumerator ShuffleDiscard (Player shufflingPlayer, ActionMarker.ActionSprite action){
		// Return the actual card objects to their deck object
		if (shufflingPlayer == Player.A) {
			yield return cardsAnimationManager.ParentDiscardCardsToDeckObject (playerAAreas, action);
			deckA.ShuffleDiscard ();
		} else { // Player B
			yield return cardsAnimationManager.ParentDiscardCardsToDeckObject (playerBAreas, action);
			deckB.ShuffleDiscard ();
		}
	}

	private void HandDraggable(bool draggable){
		foreach (Card card in deckA.GetHand()) {
			card.GetComponent<Draggable> ().enabled = draggable;
		}
	}

	private void EndGame(){
		// Set the menu to match over and load the UI scene.
		FirstMenuManager.SetFirstMenuName ("Match Over");
		levelManager.LoadLevel ("Start Screen");
	}

	private Deck<Card> CreateDeck (GameObject playerArea, Player player)
	{
		List<Card> cards = new List<Card> ();

		// There are 10 cards in a deck
		for (int rank = 1; rank <= 10; ++rank) {
			GameObject deckObject = playerArea.transform.Find ("Deck").gameObject;

			Card newCard = Instantiate (cardPrefab, deckObject.transform);

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

	private IEnumerator DoNothingAction(Player firstPlayer, Player secondPlayer, ActionMarker.ActionSprite firstAction, ActionMarker.ActionSprite secondAction){
		ActionMarker actionMarkerA;
		ActionMarker actionMarkerB;

		if (firstPlayer == Player.A) {
			actionMarkerA = GameObject.Find ("Player A Areas").GetComponentInChildren<ActionMarker> ();
			actionMarkerB = GameObject.Find ("Player B Areas").GetComponentInChildren<ActionMarker> ();
		} else{
			actionMarkerA = GameObject.Find ("Player B Areas").GetComponentInChildren<ActionMarker> ();
			actionMarkerB = GameObject.Find ("Player A Areas").GetComponentInChildren<ActionMarker> ();
		}

		if (secondAction == ActionMarker.ActionSprite.Lower){	// Lower card has no action
			actionMarkerB.SetMarker (secondAction);
		} else{													// Higher card is cancelled or same number
			actionMarkerA.SetMarker (firstAction);
			actionMarkerB.SetMarker (secondAction);
		}

		yield return new WaitForSeconds (settingsController.GetAnimationSpeed());

		actionMarkerA.RemoveMarker ();
		actionMarkerB.RemoveMarker ();
	}
}					