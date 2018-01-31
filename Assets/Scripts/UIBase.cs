using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIBase : MonoBehaviour {
	const float AnimationDuration = 0.2f;
	static List<UIBase> animatingElements = new List<UIBase> ();
	static bool allowNewAnimations = true;

	public Graphic[] elements;
	//	public Image background;

	Vector3[] originalLocations;
	AnimationCurve animationCurve;
	bool hidden = false;
	//TODO start as hidden?

	void Start () {
		originalLocations = new Vector3[elements.Length];
		for (int i = 0; i < elements.Length; i++) {
			originalLocations [i] = elements [i].transform.localPosition;
		}
		animationCurve = AnimationCurve.EaseInOut (0, 0, 1, 1);
	}

	void Update () {
		if (name == "UIBase") {
			if (Input.GetKey (KeyCode.S)) {
				Show ();
			} else if (Input.GetKey (KeyCode.H)) {
				Hide ();
			}
		}
	}

	void StartAnimating () {
		allowNewAnimations = false;
		if (animatingElements.Contains (this)) {
			Debug.LogWarning ("Double animating on " + name);
		} else {
			animatingElements.Add (this);
		}
	}

	void StopAnimating () {
		if (animatingElements.Contains (this)) {
			animatingElements.Remove (this);
			if (animatingElements.Count == 0) {
				allowNewAnimations = true;
			}
		} else {
			Debug.LogWarning ("Zero animating on " + name);
		}
	}

	public void OnPointerEnter (GameObject source) {
		if (allowNewAnimations && !hidden) {
			StartCoroutine (_OnPointerEnter ());

		}
	}

	IEnumerator _OnPointerEnter () {
		for (int i = 0; i < elements.Length; i++) {
			elements [i].CrossFadeAlpha (0.9f, AnimationDuration, true);
		}
		yield return null;
	}

	public void OnPointerExit (GameObject source) {
		if (allowNewAnimations && !hidden) {
			StartCoroutine (_OnPointerExit ());
		}
	}

	IEnumerator _OnPointerExit () {
		for (int i = 0; i < elements.Length; i++) {
			elements [i].CrossFadeAlpha (1, AnimationDuration, true);
		}
		yield return null;
	}

	public void OnPointerClick (GameObject source) {
		if (allowNewAnimations && !hidden) {
			StartCoroutine (_OnPointerClick ());
		}
	}

	IEnumerator _OnPointerClick () {
		float lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsOffset (Vector3.up * 5 * frac);

			yield return null;
			lifetime += Time.deltaTime * 2;
		}
		lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsOffset (Vector3.up * 5 * (1 - frac));
			yield return null;
			lifetime += Time.deltaTime * 2;
		}
		SetElementsOffset (Vector3.zero);
	}

	public void Show () {
		if (allowNewAnimations) {
			foreach (UIBase element in GetComponentsInChildren<UIBase>()) {
				element.StartCoroutine (element._Show ());
			}
		}
	}

	IEnumerator _Show () {
		StartAnimating ();

		float lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsOffset (Vector3.down * 50 * (1 - frac));
			SetElementsAlpha (frac);
			yield return null;
			lifetime += Time.deltaTime;
		}
		SetElementsOffset (Vector3.zero);
		SetElementsAlpha (1);

		StopAnimating ();
		hidden = false;
	}

	public void Hide () {
		if (allowNewAnimations) {
			foreach (UIBase element in GetComponentsInChildren<UIBase>()) {
				element.StartCoroutine (element._Hide ());
			}
		}
	}


	IEnumerator _Hide () {
		StartAnimating ();

		float lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsOffset (Vector3.down * 50 * frac);
			SetElementsAlpha (1 - frac);
			yield return null;
			lifetime += Time.deltaTime;
		}
		SetElementsOffset (Vector3.down * 50);
		SetElementsAlpha (0);

		StopAnimating ();
		hidden = true;
	}


	protected void SetElementsAlpha (float alpha) {
		for (int i = 0; i < elements.Length; i++) {
			elements [i].CrossFadeAlpha (alpha, 0, true);
		}
	}

	protected void SetElementsOffset (Vector3 offset) {
		for (int i = 0; i < elements.Length; i++) {
			elements [i].transform.localPosition = originalLocations [i] + offset;
		}
	}
}
