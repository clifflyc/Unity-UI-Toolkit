using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointerEventRedirect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
	public UIBase redirectTo;

	public void OnPointerEnter (PointerEventData eventData) {
		redirectTo.OnPointerEnter (gameObject);
	}

	public void OnPointerExit (PointerEventData eventData) {
		redirectTo.OnPointerExit (gameObject);

	}

	public void OnPointerClick (PointerEventData eventData) {
		redirectTo.OnPointerClick (gameObject);
	}
}
