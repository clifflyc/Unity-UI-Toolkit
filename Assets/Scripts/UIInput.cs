using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIInput : UILabel {
	
	InputField inputField;

	protected override string stringElement { get { return inputField.text; } set { inputField.text = value; } }

	protected override void Start () {
		base.Start ();
		foreach (Graphic graphic in elements) {
			InputField inputField = graphic.GetComponent<InputField> ();
			if (inputField) {
				this.inputField = inputField;
			}
		}
	}
}
