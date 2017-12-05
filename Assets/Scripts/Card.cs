using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
	// These variables are set when creating the deck at GameManager
	private int rank;
	private Sprite frontCard; 
	private Sprite backCard;

	// Disable/Enable Draggable (only cards in player's hand should be draggable)
	public void EnableDraggable(bool isDraggable){
		Draggable draggable = GetComponent<Draggable> ();

		if (isDraggable){
			draggable.enabled = true;
		}
		else{
			draggable.enabled = false;
		}
	}

	// Show front side
	public void ShowFront(){
		GetComponent<Image> ().sprite = frontCard;
	}

	// Show back side
	public void ShowBack(){
		GetComponent<Image> ().sprite = backCard;
	}

	// Setters and Getters
	public void SetRank(int r){
		rank = r;
	}

	public int GetRank(){
		return rank;
	}

	public void SetFrontCard(Sprite front){
		frontCard = front;
	}

	public Sprite GetFrontCard(){
		return frontCard;
	}

	public void SetBackCard(Sprite back){
		backCard = back;
	}

	public Sprite GetBackCard(){
		return backCard;
	}
}