using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// This script is a little complicated because of a workaround for a bug.
// When a card is being set to its parent's parent, it briefly changes position
// (like abstract small flicker ) and then it returns to follow the mouse pointer.
// To solve it, I used a duplicate card which is generated at the start of the drag
// and is destroyed at the end of the drag. The original is hidden during the dragging
// and its parent is set to the canvas.

[RequireComponent(typeof(Image))]
public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

	public bool dragOnSurfaces = true;

	[HideInInspector]
	public Transform parentToreturnTo = null;

	private GameObject m_DraggingIcon;
	private RectTransform m_DraggingPlane;

	private GameManager gameManager;
	private DropArea dropArea;
	private bool cardPlayedToDropArea;

	void Start(){
		dropArea = GameObject.FindObjectOfType<DropArea> ();
		gameManager = GameObject.FindObjectOfType<GameManager> ();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		parentToreturnTo = transform.parent;

		var canvas = FindInParents<Canvas>(gameObject);
		if (canvas == null)
			return;

		// We have clicked something that can be dragged.
		// What we want to do is create an icon for this.
		m_DraggingIcon = new GameObject("icon");

		m_DraggingIcon.transform.SetParent(canvas.transform, false);

		// Set it at the last on the list, so it will appear on top of everything.
		m_DraggingIcon.transform.SetAsLastSibling();

		// Give the icon duplicate the image of the dragged item
		var image = m_DraggingIcon.AddComponent<Image>();

		image.sprite = GetComponent<Image>().sprite;
		image.SetNativeSize();

		// stop blocking raycasts, so that the drop area can see something is dropped on it
		image.raycastTarget = false;

		if (dragOnSurfaces)
			m_DraggingPlane = transform as RectTransform;
		else
			m_DraggingPlane = canvas.transform as RectTransform;

		SetDraggedPosition(eventData);

		// Hide the original card to prevent the brief flicker bug and only then change its parent
		GetComponent<Image> ().enabled = false;
		transform.SetParent (canvas.transform, false);

		// Highlight the drop area for the player
		dropArea.Highlight (true);

		// If the card is dragged to the drop area, this value is changed to true and triggers action executions for this round
		cardPlayedToDropArea = false;
	}

	public void OnDrag(PointerEventData data)
	{
		if (m_DraggingIcon != null)
			SetDraggedPosition(data);
	}

	private void SetDraggedPosition(PointerEventData data)
	{
		if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
			m_DraggingPlane = data.pointerEnter.transform as RectTransform;

		var rt = m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
		{
			rt.position = globalMousePos;
			rt.rotation = m_DraggingPlane.rotation;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		// Destroy the icon duplicate
		if (m_DraggingIcon != null)
			Destroy(m_DraggingIcon);

		// Return the original card to the selected parent and unhide it.
		transform.SetParent (parentToreturnTo);
		GetComponent<Image> ().enabled = true;

		dropArea.Highlight (false);

		if (cardPlayedToDropArea){
			gameManager.ExecuteRound (GetComponent<Card>());
		}
	}

	static public T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null) return null;
		var comp = go.GetComponent<T>();

		if (comp != null)
			return comp;

		Transform t = go.transform.parent;
		while (t != null && comp == null)
		{
			comp = t.gameObject.GetComponent<T>();
			t = t.parent;
		}
		return comp;
	}

	public void SetCardPlayedToDropArea(bool newValue){
		cardPlayedToDropArea = newValue;
	}
}
