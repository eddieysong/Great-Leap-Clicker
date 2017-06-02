using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// dependencies for saving and loading
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour
{
	// game logic variables
	private double totalFood = 0;
	private double totalSpent = 0;
	private double foodPerClick = 1;
	private double foodPerSecond = 0;

	private long numRedBooks = 0;
	private double totalMultiplier = 1;

	private long numDiamonds = 0;

	// configuration variables
	private bool autoSaveEnabled = true;
	private int secondsBetweenAutoSaving = 30;


	// handles to other controllers
	private UIController uiController;
	private PanelController[] upgradePanels;

	// helper flag
	private bool justStarted;


	void Awake ()
	{
		// store handles to other objects
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
		upgradePanels = GameObject.Find ("Main Upgrade Interface/Viewport/Content").transform.GetComponentsInChildren<PanelController> ();
	}

	void Start()
	{		
		// flag to stop OnApplicationPause to load the game again
		justStarted = true;

		// load game from file
		Load ();

		// starts auto-saving coroutine
		StartCoroutine ("AutoSave");

		// selects the upgrades tab by default
		GameObject.Find("Upgrade Tab").GetComponent<TabController>().ButtonClick();
	}

	void Update ()
	{
		// Main income logic here, triggered every frame
		totalFood += foodPerSecond * Time.deltaTime * totalMultiplier;

		// testing shortcuts
		if (Input.GetKeyDown (KeyCode.Space)) {
			totalFood += foodPerSecond * 3600 * totalMultiplier;
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			uiController.NewMessagePanel ();
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			numDiamonds += 10;
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
		totalSpent += cost;
	}

	public void UpdateIncome()
	{
		// reset income numbers to be tallied again
		foodPerClick = 1;
		foodPerSecond = 0;

		// updates based on info from all panels
		foreach (PanelController panel in upgradePanels) {
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

	}

	// updates the total multiplier
	public void CalcTotalMultiplier ()
	{
		totalMultiplier = 1.0;
		totalMultiplier *= (1 + (double)numRedBooks / 10);
		Debug.Log ("current multiplier: " + FormatDouble(totalMultiplier));
		uiController.UpdateUpgradePanels ();
	}

	// takes a double and returns a simplified string representation
	public string FormatDouble (double number)
	{
		if (number < 1000) {
			return number.ToString ("F2") + " g";
		} else if (number < 1000000) {
			return (number / 1000).ToString ("F2") + " kg";
		} else if (number < 1000000000) {
			return (number / 1000000).ToString ("F2") + " t";
		} else if (number < 1000000000000) {
			return (number / 1000000000).ToString ("F2") + " Kt";
		} else if (number < 1000000000000000) {
			return (number / 1000000000000).ToString ("F2") + " Mt";
		} else if (number < 1000000000000000000) {
			return (number / 1000000000000000).ToString ("F2") + " Gt";
		} else if (number < 1000000000000000000000.0) {
			return (number / 1000000000000000000).ToString ("F2") + " Tt";
		} else if (number < 1000000000000000000000000.0) {
			return (number / 1000000000000000000000.0).ToString ("F2") + " Pt";
		} else if (number < 1000000000000000000000000000.0) {
			return (number / 1000000000000000000000000.0).ToString ("F2") + " Et";
		} else if (number < 1000000000000000000000000000000.0) {
			return (number / 1000000000000000000000000000.0).ToString ("F2") + " Zt";
		} else if (number < 1000000000000000000000000000000000.0) {
			return (number / 1000000000000000000000000000000.0).ToString ("F2") + " Yt";
		} else if (number < System.Math.Pow(10, 36)) {
			return (number / System.Math.Pow(10, 33)).ToString ("F2") + " KY";
		} else if (number < System.Math.Pow(10, 39)) {
			return (number / System.Math.Pow(10, 36)).ToString ("F2") + " MY";
		} else if (number < System.Math.Pow(10, 42)) {
			return (number / System.Math.Pow(10, 39)).ToString ("F2") + " GY";
		} else if (number < System.Math.Pow(10, 45)) {
			return (number / System.Math.Pow(10, 42)).ToString ("F2") + " TY";
		} else if (number < System.Math.Pow(10, 48)) {
			return (number / System.Math.Pow(10, 45)).ToString ("F2") + " PY";
		} else if (number < System.Math.Pow(10, 51)) {
			return (number / System.Math.Pow(10, 48)).ToString ("F2") + " EY";
		} else if (number < System.Math.Pow(10, 54)) {
			return (number / System.Math.Pow(10, 51)).ToString ("F2") + " ZY";
		} else if (number < System.Math.Pow(10, 57)) {
			return (number / System.Math.Pow(10, 54)).ToString ("F2") + " YY";
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


	// returns a string representation of the total multiplier
	public string FormatMultiplier (double number) {
		if (number < 1000) {
			return number.ToString ("F2");
		} else {
			return FormatLong (Convert.ToInt64 (number));
		}
	}



	// reset game
	// player loses all upgrade levels, skills, etc.
	// gains redbooks according to total food produced
	public void ResetGame () {

		long redBooksGained = CalcRedBooksGained ();
		IncreaseRedBooks (redBooksGained);
		foreach (PanelController panel in upgradePanels) {
			panel.Level = 0;
		}
		totalFood = 0;
		totalSpent = 0;

		Save ();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// calculates the number of red books to be gained by resetting, based on total food produced and levels
	public long CalcRedBooksGained () {

		long redBooksGained = 0;
		redBooksGained +=  Convert.ToInt64(Math.Floor (Math.Pow (1.5, (Math.Log10 ((totalFood + totalSpent) / 1000000000)))));

		double totalLevels = 0;
		foreach (PanelController panel in upgradePanels) {
			totalLevels += panel.Level;
		}
		redBooksGained += Convert.ToInt64 (Math.Floor (totalLevels / 500));
		return redBooksGained;
	}




	// save and load functions
	public void Save()
	{
		Debug.Log ("Saving game...");

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/PlayerData.dat");

		PlayerData data = new PlayerData ();
		data.totalFood = totalFood;
		data.totalSpent = totalSpent;
		data.numRedBooks = numRedBooks;
		data.numDiamonds = numDiamonds;
		data.timeStamp = DateTime.Now;
		foreach (PanelController panel in upgradePanels) {
			data.upgradeLevels.Add (panel.Id, panel.Level);
		}

		bf.Serialize (file, data);
		file.Close ();

		Debug.Log ("Game saved!");
	}

	public void Load() 
	{
		Debug.Log ("Loading game...");

		if (File.Exists (Application.persistentDataPath + "/PlayerData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();

			// set variables to match values loaded from save file
			totalFood = data.totalFood;
			totalSpent = data.totalSpent;
			numRedBooks = data.numRedBooks;
			numDiamonds = data.numDiamonds;
			foreach (PanelController panel in upgradePanels) {
				panel.Level = data.upgradeLevels [panel.Id];
			}


			// updates multiplier and income
			CalcTotalMultiplier ();
			UpdateIncome ();

			DateTime timeStamp = data.timeStamp;

			Debug.Log ((DateTime.Now - timeStamp).TotalSeconds);
			CalcOfflineEarning ((DateTime.Now - timeStamp).TotalSeconds);

			Debug.Log ("Game loaded!");
		} else {
			Debug.Log ("No save file found!");
		}
	}

	public void CalcOfflineEarning(double totalSeconds) {
		if (totalSeconds > 10) {
			// calculate offline earning
			double offlineEarning = foodPerSecond * totalSeconds * totalMultiplier;
			totalFood += offlineEarning;

			if (offlineEarning > 0) {
				// msg panel to state offline earning
				MessagePanelController offlineEarningPanel = uiController.NewMessagePanel ();
				offlineEarningPanel.SetTitle ("Offline Earning");
				offlineEarningPanel.SetBody ("While you were away, your workers earned <color=#ff0000ff>"
					+ FormatDouble (offlineEarning) + "</color> of food.\nLong Live the Chairman!");
				offlineEarningPanel.SetButtonText ("Long Live!");
				offlineEarningPanel.SetImage ("Sprites/Upgrade Icons/wheat");
			}
		}
	}

	public void DeleteSaveFile() 
	{
		File.Delete (Application.persistentDataPath + "/PlayerData.dat");
		Debug.Log ("Save game deleted!");
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// the game saves data on pause/exit, loads on resume (except when game just started)
	void OnApplicationPause(bool isPaused)
	{
		Debug.Log ("OnApplicationPause() called, isPaused = " + isPaused.ToString());
		if (isPaused) {
			Save ();
		} else if (justStarted) {
			justStarted = false;
			Load ();
		}
	}

	// the game saves data on pause/exit
	void OnApplicationQuit()
	{
		Debug.Log ("OnApplicationQuit() called");
		Save ();
	}

	// the game saves data periodically
	IEnumerator AutoSave() {
		while (autoSaveEnabled == true) {
			yield return new WaitForSeconds (secondsBetweenAutoSaving);
			Save ();
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

	public long NumDiamonds {
		get {
			return this.numDiamonds;
		}
	}

	public double TotalMultiplier {
		get {
			return this.totalMultiplier;
		}
	}

}

// class to hold save file data
[Serializable]
class PlayerData
{
	public double totalFood;
	public double totalSpent;
	public long numRedBooks;
	public long numDiamonds;
	//public int numDiamonds;

	public DateTime timeStamp;

	public Dictionary<int, int> upgradeLevels = new Dictionary<int, int> ();
}
