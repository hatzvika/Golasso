using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

	[HideInInspector]
	public Transform parentToreturnTo = null;

	//private DropArea dropArea;
	//private Color dropAreaColor;
	//
	//void OnStart(){
	//	dropArea = GameObject.FindObjectOfType<DropArea> ();
	//	dropAreaColor = dropArea.GetComponent<Image> ().color;
	//}

	public void OnBeginDrag(PointerEventData eventData){
		// stop blocking raycasts, so that the drop area can see something is dropped on it
		GetComponent<Image> ().raycastTarget = false;

		parentToreturnTo = transform.parent;
		transform.SetParent (transform.parent.parent, true);

	}

	public void OnDrag(PointerEventData eventData){
		// Convert the mouse position to world untis because the canvas is in world mode.
		float cameraZpos = Camera.main.transform.position.z;
		Vector3 screenPoint = eventData.position;
		screenPoint.z = cameraZpos; // // distance of the plane from the camera
		//GetComponent<RectTransform>().anchoredPosition = eventData.position;
		transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
		//transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData){
		transform.SetParent (parentToreturnTo);

		// Resume getting raycasts after drop
		GetComponent<Image> ().raycastTarget = true;
	}

}
