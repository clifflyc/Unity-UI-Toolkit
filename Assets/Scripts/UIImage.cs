using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImage : UIBase {

	Image image;

	protected virtual Sprite spriteElement { get { return image.sprite; } set { image.sprite = value; } }

	protected override void Start () {
		base.Start ();
		foreach (Graphic graphic in graphicElements) {
			if (graphic is Image) {
				if (image) {
					Debug.LogWarning ("Don't put multiple Images on a single UIImage");
				}
				image = (Image)graphic;
			}
		}
	}

	public void SetSprite (Sprite s) {
		spriteElement = s;
	}
}
