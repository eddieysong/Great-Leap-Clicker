using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	// game logic variables
	public double totalFood = 0f;
	public double foodPerClick = 1.0f;
	public double foodPerSecond = 0f;

	// configuration variables

	// handles to other controllers
	private UIController uiController;


	// Use this for initialization
	void Start ()
	{

		// store handles to other objects
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();



	}
	
	// Update is called once per frame
	void Update ()
	{

		// Main income logic here, triggered every frame
		totalFood += foodPerSecond * Time.deltaTime;
//		Debug.Log (Time.deltaTime);

		if (Input.GetKey (KeyCode.P)) {
			totalFood += 1000000;
		}

	}

	public void Click ()
	{
		totalFood += foodPerClick;
	}

	public void IncreaseFoodPerSec (double amount)
	{
		foodPerSecond += amount;
	}

	public string NumberFormat (double number)
	{
		if (number < 1000) {
			return number.ToString ("F2") + " kg";
		} else if (number < 1000000) {
			return (number / 1000).ToString ("F2") + " t";
		} else if (number < 1000000000) {
			return (number / 1000000).ToString ("F2") + " kt";
		} else if (number < 1000000000000) {
			return (number / 1000000000).ToString ("F2") + " mt";
		} else if (number < 1000000000000000) {
			return (number / 1000000000000).ToString ("F2") + " gt";
		} else if (number < 1000000000000000000) {
			return (number / 1000000000000000).ToString ("F2") + " tt";
		} else {
			return "1*";
		}

	}
}
