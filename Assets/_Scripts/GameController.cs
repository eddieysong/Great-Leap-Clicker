using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	// game logic variables
	public double totalFood = 0f;
	public double foodPerClick = 1.0f;
	public double foodPerSecond = 0f;

	public long numRedBooks = 0;
	public double totalMultiplier = 1.0;

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
		totalFood += foodPerSecond * Time.deltaTime * totalMultiplier;
//		Debug.Log (Time.deltaTime);

		if (Input.GetKey (KeyCode.P)) {
			totalFood += 1000000;
		}

		if (Input.GetKey (KeyCode.U)) {
			IncreaseRedBooks (1000000000000);
		}

	}

	public void Click ()
	{
		totalFood += foodPerClick * totalMultiplier;
	}

	public void IncreaseFoodPerSec (double amount)
	{
		foodPerSecond += amount;
	}

	// each red book increases production by 10%, stacks additively
	public void IncreaseRedBooks (long amount)
	{
		numRedBooks += amount;
		CalcTotalMultiplier ();
		foreach (PanelController panel in GameObject.Find("Viewport/Content").transform.GetComponentsInChildren<PanelController>()) {
			panel.RefreshPanelText ();
		}
	}

	// updates the total multiplier
	public void CalcTotalMultiplier ()
	{
		totalMultiplier = 1.0;
		totalMultiplier *= (1 + (double)numRedBooks / 10);
		Debug.Log (totalMultiplier);
	}

	// takes a double and returns a simplified string representation
	public string NumberFormat (double number)
	{
		if (number < 1000) {
			return number.ToString ("F2") + " kg";
		} else if (number < 1000000) {
			return (number / 1000).ToString ("F2") + " t";
		} else if (number < 1000000000) {
			return (number / 1000000).ToString ("F2") + " kt";
		} else if (number < 1000000000000) {
			return (number / 1000000000).ToString ("F2") + " Mt";
		} else if (number < 1000000000000000) {
			return (number / 1000000000000).ToString ("F2") + " Gt";
		} else if (number < 1000000000000000000) {
			return (number / 1000000000000000).ToString ("F2") + " Tt";
		} else if (number < 1000000000000000000000.0) {
			return (number / 1000000000000000000).ToString ("F2") + " Pt";
		} else if (number < 1000000000000000000000000.0) {
			return (number / 1000000000000000000000.0).ToString ("F2") + " Et";
		} else if (number < 1000000000000000000000000000.0) {
			return (number / 1000000000000000000000000.0).ToString ("F2") + " Zt";
		} else if (number < 1000000000000000000000000000000.0) {
			return (number / 1000000000000000000000000000.0).ToString ("F2") + " Yt";
		} else {
			return "1*";
		}

	}
}
