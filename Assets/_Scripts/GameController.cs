using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// game logic variables
	public float totalFood = 0f;
	public float foodPerClick = 1.0f;
	public float foodPerSecond = 0f;

	// configuration variables

	// handles to other controllers
	private UIController uiController;


	// Use this for initialization
	void Start () {

		// store handles to other objects
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();



	}
	
	// Update is called once per frame
	void Update () {

		// Main income logic here, triggered every frame
		totalFood += foodPerSecond * Time.deltaTime;
//		Debug.Log (Time.deltaTime);

	}
		
	public void Click() {
		totalFood += foodPerClick;
	}

	public void IncreaseFoodPerSec(float amount) {
		foodPerSecond += amount;
	}
}
