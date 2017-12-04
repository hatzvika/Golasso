using UnityEngine;

public class Card : MonoBehaviour
{
	private int rank;

	public void setRank(int r){
		rank = r;
	}

	public int getRank(){
		return rank;
	}
}