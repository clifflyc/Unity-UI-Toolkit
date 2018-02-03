using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIBase : MonoBehaviour {

	public Graphic[] elements;
	

	// aniamtions
	public enum UICommonAnimation {
		None,
		LowerAlpha,
		RaiseAlpha,
		BounceUp,
		BounceDown,
		Enlarge,
		Unenlarge
	}

	const float AnimationDuration = 0.2f;
	const float AnimationDistance = 10f;

	public UICommonAnimation hoverAnimation;
	public UICommonAnimation unhoverAnimation;
	public UICommonAnimation pressAnimation;
	public UICommonAnimation unpressAnimation;

	// events
	[System.Serializable]
	public class UIEventVoid:UnityEvent {
		
	}

	public UIEventVoid OnHover;
	public UIEventVoid OnUnhover;
	public UIEventVoid OnPress;
	public UIEventVoid OnUnpress;

	Vector3[] originalLocations;
	AnimationCurve animationCurve = AnimationCurve.EaseInOut (0, 0, 1, 1);

	protected bool hidden = false;

	Queue<IEnumerator> animationQueue = new Queue<IEnumerator> ();
	bool animationQueueWorkerAlive = false;

	protected bool animating { get { return animationQueue.Count == 0; } }

	//TODO start as hidden?

	protected virtual void Start () {
		originalLocations = new Vector3[elements.Length];
		for (int i = 0; i < elements.Length; i++) {
			originalLocations [i] = elements [i].transform.localPosition;
		}
	}

	//---------------------//

	public virtual void OnPointerHover (GameObject source) {
		if (!hidden) {
			OnHover.Invoke ();
			QueueAnimation (_OnPointerHover ());
		}
	}

	IEnumerator _OnPointerHover () {
		yield return PlayCommonAnimation (hoverAnimation);
	}

	public virtual void OnPointerUnhover (GameObject source) {
		if (!hidden) {
			OnUnhover.Invoke ();
			QueueAnimation (_OnPointerUnhover ());
		}
	}

	IEnumerator _OnPointerUnhover () {
		yield return PlayCommonAnimation (unhoverAnimation);
	}

	public virtual void OnPointerPress (GameObject source) {
		if (!hidden) {
			OnPress.Invoke ();
			QueueAnimation (_OnPointerPress ());
		}
	}

	IEnumerator _OnPointerPress () {
		yield return PlayCommonAnimation (pressAnimation);
	}

	public virtual void OnPointerUnpress (GameObject source) {
		if (!hidden) {
			OnUnpress.Invoke ();
			QueueAnimation (_OnPointerUnpress ());
		}
	}

	IEnumerator _OnPointerUnpress () {
		yield return PlayCommonAnimation (unpressAnimation);
	}

	IEnumerator PlayCommonAnimation (UICommonAnimation commonAnimation) {
		switch (commonAnimation) {
		case UICommonAnimation.BounceUp:
			yield return A_BounceUp ();
			break;
		case UICommonAnimation.BounceDown:
			yield return A_BounceDown ();
			break;
		case UICommonAnimation.RaiseAlpha:
			yield return A_RaiseAlpha ();
			break;
		case UICommonAnimation.LowerAlpha:
			yield return A_LowerAlhpa ();
			break;
		case UICommonAnimation.Enlarge:
			yield return A_Enlarge ();
			break;
		case UICommonAnimation.Unenlarge:
			yield return A_Unenlarge ();
			break;
		}
	}

	//----------------------//

	public void Show () {
		foreach (UIBase element in GetComponentsInChildren<UIBase>()) {
			element.QueueAnimation (element._Show ());
		}
	}

	IEnumerator _Show () {
		yield return A_RiseIn ();
		hidden = false;
	}

	public void Hide () {
		foreach (UIBase element in GetComponentsInChildren<UIBase>()) {
			element.QueueAnimation (element._Hide ());
		}
	}


	IEnumerator _Hide () {
		yield return A_FallOut ();
		hidden = true;
	}

	//---------------------//

	IEnumerator A_RiseIn () {
		float lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsOffset (Vector3.down * AnimationDistance * (1 - frac));
			SetElementsAlpha (frac);
			yield return null;
			lifetime += Time.deltaTime;
		}
		SetElementsOffset (Vector3.zero);
		SetElementsAlpha (1);
	}

	IEnumerator A_FallOut () {
		float lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsOffset (Vector3.down * AnimationDistance * frac);
			SetElementsAlpha (1 - frac);
			yield return null;
			lifetime += Time.deltaTime;
		}
		SetElementsOffset (Vector3.down * AnimationDistance);
		SetElementsAlpha (0);
	}

	IEnumerator A_BounceDown () {
		float lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsOffset (Vector3.up * AnimationDistance * (1 - frac));
			yield return null;
			lifetime += Time.deltaTime * 2;
		}
		SetElementsOffset (Vector3.zero);
	}

	IEnumerator A_BounceUp () {
		float lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsOffset (Vector3.up * AnimationDistance * frac);
			yield return null;
			lifetime += Time.deltaTime * 2;
		}
		SetElementsOffset (Vector3.up * AnimationDistance);
	}

	IEnumerator A_LowerAlhpa () {
		float lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsAlpha (1 - 0.1f * frac);
			yield return null;
			lifetime += Time.deltaTime;
		}
		SetElementsAlpha (0.9f);
	}

	IEnumerator A_RaiseAlpha () {
		float lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsAlpha (0.9f + 0.1f * frac);
			yield return null;
			lifetime += Time.deltaTime;
		}
		SetElementsAlpha (1);
	}

	IEnumerator A_Enlarge () {
		float lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsScale (1 + 0.1f * frac);
			yield return null;
			lifetime += Time.deltaTime;
		}
		SetElementsScale (1.1f);
	}

	IEnumerator A_Unenlarge () {
		float lifetime = 0;
		while (lifetime < AnimationDuration) {
			float frac = animationCurve.Evaluate (lifetime / AnimationDuration);
			SetElementsScale (1 + 0.1f * (1 - frac));
			yield return null;
			lifetime += Time.deltaTime;
		}
		SetElementsScale (1);
	}

	//--------//

	protected void QueueAnimation (IEnumerator animation) {
		// no duplicates
//		foreach (IEnumerator a in animationQueue) {
//			if (a.ToString () == animation.ToString ()) {
//				return;
//			}
//		}
		animationQueue.Enqueue (animation);
		if (!animationQueueWorkerAlive) {
			StartCoroutine (AnimationQueueWorker ());
		}
	}

	IEnumerator AnimationQueueWorker () {
		animationQueueWorkerAlive = true;
		while (animationQueue.Count > 0) {
			yield return animationQueue.Dequeue ();
		}
		animationQueueWorkerAlive = false;
	}

	protected void SetElementsAlpha (float alpha) {
		for (int i = 0; i < elements.Length; i++) {
			elements [i].CrossFadeAlpha (alpha, 0, true);
		}
	}

	protected void SetElementsScale (float scale) {
		for (int i = 0; i < elements.Length; i++) {
			elements [i].transform.localScale = new Vector3 (scale, scale, scale);
		}
	}

	protected void SetElementsOffset (Vector3 offset) {
		for (int i = 0; i < elements.Length; i++) {
			elements [i].transform.localPosition = originalLocations [i] + offset;
		}
	}

}
