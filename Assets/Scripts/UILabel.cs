using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UILabel : UIBase {

	[System.Serializable]
	public class UIEventString:UnityEvent<string> {

	}

	public UIEventString OnHoverString;
	public UIEventString OnUnhoverString;
	public UIEventString OnClickString;

	Text label;

	protected virtual string stringElement { get { return label.text; } set { label.text = value; } }

	protected override void Start () {
		base.Start ();
		foreach (Graphic graphic in elements) {
			if (graphic is Text) {
				label = (Text)graphic;
			}
		}
	}

	public void SetString (string s) {
		stringElement = s;
	}

	public override void OnPointerEnter (GameObject source) {
		base.OnPointerEnter (source);
		if (!animating && !hidden) {
			OnHoverString.Invoke (stringElement);
		}
	}


	public override void OnPointerExit (GameObject source) {
		base.OnPointerExit (source);
		if (!animating && !hidden) {
			OnUnhoverString.Invoke (stringElement);

		}
	}

	public override void OnPointerClick (GameObject source) {
		base.OnPointerClick (source);
		if (!animating && !hidden) {
			OnClickString.Invoke (stringElement);
		}
	}
}