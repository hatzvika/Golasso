using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler {

	public void OnDrop(PointerEventData eventData){
		// This function activates before the OnEndDrag function in the Draggabe script.
		// Thus, the parentToReturnTo can be modified before going to the original parent.

		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();
		if (d != null) {
			d.parentToreturnTo = transform;
		}
	}
}
