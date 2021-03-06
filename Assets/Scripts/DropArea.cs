﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler {

	private Color originalColor;

	void Start (){
		originalColor = GetComponent<Image> ().color;
	}

	public void OnDrop(PointerEventData eventData){
		// This function activates before the OnEndDrag function in the Draggabe script.
		// Thus, the parentToReturnTo can be modified before going to the original parent.

		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();
		if (d != null) {
			d.parentToreturnTo = transform;
		}
		d.SetCardPlayedToDropArea (true);
	}

	public void Highlight (bool highlight) {
		Image image = GetComponent<Image>();
		if (highlight){
			Color highlightedColor = originalColor;
			highlightedColor.a = 0.7f;
			image.color = highlightedColor;
		}
		else{
			image.color = originalColor;
		}
	}

}
