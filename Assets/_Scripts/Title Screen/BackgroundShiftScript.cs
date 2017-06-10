using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShiftScript : MonoBehaviour {

	[SerializeField]
	private float maxShiftX, maxShiftY, maxSpeed = 1, changeDirectionTime = 3;

	private Vector2 originalPos, targetPos;

	// Use this for initialization
	void Start () {
		originalPos = transform.position;
		StartCoroutine (ChangeDirection ());
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector2.MoveTowards (transform.position, targetPos, maxSpeed/60);
	}

	IEnumerator ChangeDirection() {
		while (true) {
			targetPos.x = Random.Range (originalPos.x - maxShiftX, originalPos.x + maxShiftX);
			targetPos.y = Random.Range (originalPos.y - maxShiftY, originalPos.y + maxShiftY);
//			Debug.Log (targetPos);
			yield return new WaitForSeconds (changeDirectionTime);
		}
	}
}
