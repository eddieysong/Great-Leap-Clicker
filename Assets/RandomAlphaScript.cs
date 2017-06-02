using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAlphaScript : MonoBehaviour {

	[SerializeField]
	private float minAlpha = 0.4f, maxAlpha = 0.8f, changeSpeed = 1f, changeTime = 3f;

	private SpriteRenderer spRenderer;
	private float targetAlpha;

	// Use this for initialization
	void Start () {
		spRenderer = GetComponent<SpriteRenderer> ();
		StartCoroutine (ChangeTargetAlpha ());
	}
	
	// Update is called once per frame
	void Update () {
		Color newColor = spRenderer.color;
		newColor.a += spRenderer.color.a < targetAlpha ? changeSpeed / 60 : -changeSpeed / 60;
		spRenderer.color = newColor;
	}

	IEnumerator ChangeTargetAlpha() {
		while (true) {
			targetAlpha = Random.Range (minAlpha, maxAlpha);
			yield return new WaitForSeconds (changeTime);
		}
	}
}
