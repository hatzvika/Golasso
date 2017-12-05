using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
	private List<Card> handA;
	private List<Card> handB;

	AnimationManager animationManager;
	ScoreManager scoreManager;
	Ball ball;

	Player startingPlayer;
	int currentHalf = 1;

	void Start ()
	{
		animationManager = GameObject.FindObjectOfType<AnimationManager>();
		scoreManager = GameObject.FindObjectOfType<ScoreManager>();
		ball = GameObject.FindObjectOfType<Ball>();

		// Find the objects in which cards are parented during the game
		GameObject deckObjectA = GameObject.FindGameObjectWithTag ("Deck A");
		GameObject deckObjectB = GameObject.FindGameObjectWithTag ("Deck B");

		// Prepare the initial parent for the deck cards and initial both decks with their correct card backs
		deckA = CreateDeck (deckObjectA, Player.A);
		deckB = CreateDeck (deckObjectB, Player.B);
			
		GameReset ();
	}

	public void GameReset ()
	{
		animationManager.ParentAllCardsToDeckObject ();

		// Shuffle all the cards, including discards back to the decks
		deckA.ShuffleAll ();
		deckB.ShuffleAll ();

		// Draw 3 cards for the player and set their parent to the player's hand object
		handA = deckA.Draw (3);
		animationManager.DrawToHand (handA, Player.A);

		// Draw 3 cards for the AI and set their parent to the AI's hand object
		handB = deckB.Draw (3);
		animationManager.DrawToHand (handB, Player.B);

		// Reset the score to 0:0
		scoreManager.ResetScore ();

		currentHalf = 1;

		// Reset the ball to the middle of the field with a random starting player
		PickStartingPlayer ();
		ball.SetControllingTeam (startingPlayer);
		ball.SetBallPosition (Ball.BallPosition.PlayerBField);
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

	private Deck<Card> CreateDeck (GameObject deckParent, Player player)
	{
		List<Card> cards = new List<Card> ();

		// There are 10 cards in a deck
		for (int rank = 1; rank <= 10; ++rank) {
			Card newCard = Instantiate (cardPrefab, deckParent.transform);

			// Initialize Card parameters
			newCard.name = "Card_" + rank.ToString ();
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
}					