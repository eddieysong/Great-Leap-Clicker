using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	// game logic variables
	private double totalFood = 0f;
	private double foodPerClick = 1.0f;
	private double foodPerSecond = 0f;

	private long numRedBooks = 0;
	private double totalMultiplier = 1.0;

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

		if (Input.GetKeyDown (KeyCode.O)) {
			IncreaseRedBooks (100);
		}

		if (Input.GetKeyDown (KeyCode.I)) {
			IncreaseRedBooks (1000000);
		}

		if (Input.GetKeyDown (KeyCode.U)) {
			IncreaseRedBooks (long.MaxValue);
		}

	}

	public void Click ()
	{
		totalFood += foodPerClick * totalMultiplier;
	}

	public void SpendFood (double cost)
	{
		totalFood -= cost;
	}

	public void UpdateIncome()
	{
		// reset income numbers to be tallied again
		foodPerClick = 1;
		foodPerSecond = 0;

		// updates all panels
		foreach (PanelController panel in GameObject.Find("Viewport/Content").transform.GetComponentsInChildren<PanelController>()) {
			if (panel.Id == 0) {
				foodPerClick += panel.CurrentProduction;
			} else {
				foodPerSecond += panel.CurrentProduction;
			}
		}
	}

	// each red book increases production by 10%, stacks additively
	public void IncreaseRedBooks (long amount)
	{
		numRedBooks += amount;
		CalcTotalMultiplier ();
		uiController.UpdateAllPanels ();
	}

	// updates the total multiplier
	public void CalcTotalMultiplier ()
	{
		totalMultiplier = 1.0;
		totalMultiplier *= (1 + (double)numRedBooks / 10);
		Debug.Log (totalMultiplier);
	}

	// takes a double and returns a simplified string representation
	public string FormatDouble (double number)
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
		} else if (number < System.Math.Pow (10, 300)) {
			return number.ToString ("0.00e0");
		} else {
			return "1*";
		}
	}

	// takes a double and returns a simplified string representation
	public string FormatLong (long number)
	{
		if (number < 1000) {
			return number.ToString ("F0");
		} else if (number < 1000000) {
			return ((double)number / 1000).ToString ("F2") + " K";
		} else if (number < 1000000000) {
			return ((double)number / 1000000).ToString ("F2") + " M";
		} else if (number < 1000000000000) {
			return ((double)number / 1000000000).ToString ("F2") + " B";
		} else if (number < 1000000000000000) {
			return ((double)number / 1000000000000).ToString ("F2") + " T";
		} else {
			return number.ToString ("0.00e0");
		}
	}

	// Setters and getters
	public double TotalFood {
		get {
			return this.totalFood;
		}
	}

	public double FoodPerClick {
		get {
			return this.foodPerClick;
		}
	}

	public double FoodPerSecond {
		get {
			return this.foodPerSecond;
		}
	}

	public long NumRedBooks {
		get {
			return this.numRedBooks;
		}
	}

	public double TotalMultiplier {
		get {
			return this.totalMultiplier;
		}
	}
}
