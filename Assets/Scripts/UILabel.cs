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

	public override void OnPointerHover (GameObject source) {
		base.OnPointerHover (source);
		if (!hidden) {
			OnHoverString.Invoke (stringElement);
		}
	}


	public override void OnPointerUnhover (GameObject source) {
		base.OnPointerUnhover (source);
		if (!hidden) {
			OnUnhoverString.Invoke (stringElement);

		}
	}

	public override void OnPointerPress (GameObject source) {
		base.OnPointerPress (source);
		if (!hidden) {
			OnClickString.Invoke (stringElement);
		}
	}
}