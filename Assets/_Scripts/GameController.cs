using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// dependencies for saving and loading
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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



	void Awake ()
	{
		// store handles to other objects
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
	}

	void Start()
	{		
		// load game from file
		Load ();
	}
	
	// Update is called once per frame
	void Update ()
	{

		// Main income logic here, triggered every frame
		totalFood += foodPerSecond * Time.deltaTime * totalMultiplier;
//		Debug.Log (Time.deltaTime);
//		Debug.Log ("total food: " + FormatDouble(totalFood));

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

		if (Input.GetKeyDown (KeyCode.F5)) {
			Save ();
		}

		if (Input.GetKeyDown (KeyCode.F9)) {
			Load ();
		}

		if (Input.GetKeyDown (KeyCode.End)) {
			DeleteSaveFile ();
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

		// updates based on info from all panels
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
		if (numRedBooks < 0) {
			numRedBooks = 0;
		}
		CalcTotalMultiplier ();
		uiController.UpdateUpgradePanels ();
	}

	// updates the total multiplier
	public void CalcTotalMultiplier ()
	{
		totalMultiplier = 1.0;
		totalMultiplier *= (1 + (double)numRedBooks / 10);
		Debug.Log ("current multiplier: " + FormatDouble(totalMultiplier));
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

	// save and load functions
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/PlayerData.dat");

		PlayerData data = new PlayerData ();
		data.totalFood = totalFood;
		data.numRedBooks = numRedBooks;
		foreach (PanelController panel in GameObject.Find("Main Upgrade Interface/Viewport/Content").transform.GetComponentsInChildren<PanelController>()) {
			data.upgradeLevels.Add (panel.Id, panel.Level);
		}

		bf.Serialize (file, data);
		file.Close ();

	}

	public void Load() 
	{
		if (File.Exists(Application.persistentDataPath + "/PlayerData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();

			// set variables to match values loaded from save file
			totalFood = data.totalFood;
			numRedBooks = data.numRedBooks;
			foreach (PanelController panel in GameObject.Find("Main Upgrade Interface/Viewport/Content").transform.GetComponentsInChildren<PanelController>()) {
				panel.Level = data.upgradeLevels[panel.Id];
			}

			// updates multiplier and income
			CalcTotalMultiplier ();
//			UpdateIncome ();
		}
	}

	public void DeleteSaveFile() 
	{
		File.Delete (Application.persistentDataPath + "/PlayerData.dat");
	}

	// the game saves data on pause/exit
	void OnApplicationPause(bool isPaused)
	{
		Debug.Log ("OnApplicationPause() called, isPaused = " + isPaused.ToString());
		if (isPaused) {
			Save ();
		}
	}

	void OnApplicationQuit()
	{
		Debug.Log ("OnApplicationQuit() called");
		Save ();
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

[Serializable]
class PlayerData
{
	public double totalFood;
	public long numRedBooks;
	//public int numDiamonds;

	public Dictionary<int, int> upgradeLevels = new Dictionary<int, int> ();
}
