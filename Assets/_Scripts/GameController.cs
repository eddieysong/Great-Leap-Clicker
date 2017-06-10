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
	private double redBookMultPerBook = 0.02;
	private double redBookMultiplier = 1;

	private long numDiamonds = 0; // change back when done testing

	// perk multipliers
	private double perkClickProdMult = 1;
	private double perkAutoProdMult = 1;
	private double perkClickAddPercentAuto = 0; // 0.01 = 1%
	private double perkRedBookGainMult = 1;
	private double perkRedBookMultMult = 1;


	// configuration variables
	private bool autoSaveEnabled = true;
	private int secondsBetweenAutoSaving = 30;


	// handles to other controllers
	private UIController uiController;
	private UpgradeController[] upgradePanels;
	private PerkController[] perkPanels;
	private TutorialController tutorialController;

	// clicking sounds
	[SerializeField]
	private AudioClip clickSound1, clickSound2;
	private AudioSource camAudioSource;


	// helper flag
	// flag to stop OnApplicationPause to load the game again
	private bool justStarted = true;

	// tutorial is on by default
	private bool tutorialOn = true;


	void Awake ()
	{
		// store handles to other objects
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
		upgradePanels = GameObject.Find ("Main Upgrade Interface/Viewport/Content").transform.GetComponentsInChildren<UpgradeController> ();
		perkPanels = GameObject.Find ("Main Perk Interface/Viewport/Content").transform.GetComponentsInChildren<PerkController> ();
		camAudioSource = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AudioSource> ();
		tutorialController = GameObject.Find ("TutorialController").GetComponent<TutorialController> ();
	}

	void Start()
	{
		// load game from file
		Load ();

		if (tutorialOn) {
			tutorialController.enabled = true;
		}

		// starts auto-saving coroutine
		StartCoroutine ("AutoSave");

		// selects the upgrades tab by default
		GameObject.Find("Upgrade Tab").GetComponent<TabController>().ButtonClick();
	}

	void Update ()
	{
		// Main income logic here, triggered every frame
		totalFood += this.FinalFoodPerSecond * Time.deltaTime;

		// testing shortcuts
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log("Tutorial on? " + tutorialOn.ToString());
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			MessagePanelController asd = uiController.NewMessagePanel ();
			asd.SetBody("asd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\nasd\n");
			asd.SetIcon ("Sprites/Upgrade Icons/grapes", Color.white);
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
		totalFood += this.FinalFoodPerClick;
		uiController.ShowClickText ();
		PlayClickSound1 ();
	}

	public void PlayClickSound1() {
		camAudioSource.PlayOneShot (clickSound1);
	}

	public void PlayClickSound2() {
		camAudioSource.PlayOneShot (clickSound2);
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
		foreach (UpgradeController panel in upgradePanels) {
			if (panel.Id == 0) {
				foodPerClick += panel.CurrentProduction;
			} else {
//				Debug.Log (panel.CurrentProduction);
				foodPerSecond += panel.CurrentProduction;
			}
		}
	}

	public void UpdatePerkMultipliers()
	{
		// updates based on info from all panels
		foreach (PerkController panel in perkPanels) {
			switch (panel.Id) {
			case 0:
				perkClickProdMult = 1 + panel.CurrentValue;
				break;
			case 1:
				perkAutoProdMult = 1 + panel.CurrentValue;
				break;
			case 2:
				perkClickAddPercentAuto = panel.CurrentValue;
				break;
			case 3:
				perkRedBookGainMult = 1 + panel.CurrentValue;
				break;
			case 4:
				perkRedBookMultMult = 1 + panel.CurrentValue;
				break;
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
	}

	public void IncreaseDiamonds (long amount)
	{
		numDiamonds += amount;
		if (numDiamonds < 0) {
			numDiamonds = 0;
		}
	}

	// updates the total multiplier
//	public void CalcRedBookMultiplier ()
//	{
//		redBookMultiplier = 
//		Debug.Log ("current multiplier: " + FormatDouble(redBookMultiplier));
//		uiController.UpdateAllPanels ();
//	}

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



	// reset game
	// player loses all upgrade levels, skills, etc.
	// gains redbooks according to total food produced
	public void ResetGame () {

		IncreaseRedBooks (CalcRedBooksGained ());
		foreach (UpgradeController panel in upgradePanels) {
			panel.Level = 0;
		}
		totalFood = 0;
		totalSpent = 0;
		tutorialOn = false;

		Save ();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// calculates the number of red books to be gained by resetting, based on total food produced and levels
	public long CalcRedBooksGained () {

		long redBooksGained = 0;
		redBooksGained +=  Convert.ToInt64(Math.Floor (Math.Pow (1.5, (Math.Log10 ((totalFood + totalSpent) / 1000000000)))));

		double totalLevels = 0;
		foreach (UpgradeController panel in upgradePanels) {
			totalLevels += panel.Level;
		}
		redBooksGained += Convert.ToInt64 (Math.Floor (totalLevels / 250));
		redBooksGained = Convert.ToInt64(redBooksGained * perkRedBookGainMult);
		return redBooksGained;
	}

	// diamond bonuses

	public void LeapForward (int [] info) {
		if (numDiamonds >= info[0]) {
			IncreaseDiamonds (-info [0]);
			totalFood += this.FinalFoodPerSecond * 3600 * info[1] ;
		}
	}

	public void Revolution (int [] info) {
		if (numDiamonds >= info [0]) {
			IncreaseDiamonds (-info [0]);
			IncreaseRedBooks (CalcRedBooksGained () * info [1]);
			totalFood = 0;
			totalSpent = 0;

			Save ();
		}
	}


	// switches to diamond purchase panel
	public void SwitchToDiamondPurchase() {
		// selects the upgrades tab by default
		GameObject.Find("IAP Tab").GetComponent<TabController>().ButtonClick();
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

		data.tutorialOn = tutorialOn;

		data.timeStamp = DateTime.Now;
		foreach (UpgradeController panel in upgradePanels) {
			data.upgradeLevels.Add (panel.Id, panel.Level);
		}
		foreach (PerkController panel in perkPanels) {
			data.perkLevels.Add (panel.Id, panel.Level);
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

			tutorialOn = data.tutorialOn;

			foreach (UpgradeController panel in upgradePanels) {
				panel.Level = data.upgradeLevels [panel.Id];
			}
			foreach (PerkController panel in perkPanels) {
				panel.Level = data.perkLevels [panel.Id];
			}

			// updates multiplier and income
//			CalcRedBookMultiplier ();
			UpdateIncome ();

			DateTime timeStamp = data.timeStamp;

			Debug.Log ("Seconds offline: " + (DateTime.Now - timeStamp).TotalSeconds);
			CalcOfflineEarning ((DateTime.Now - timeStamp).TotalSeconds);

			Debug.Log ("Game loaded!");
		} else {
			Debug.Log ("No save file found!");
		}
	}

	public void CalcOfflineEarning(double totalSeconds) {
		if (totalSeconds > 10) {
			// calculate offline earning
			UpdateIncome();
			Debug.Log("f/s: " + this.FinalFoodPerSecond);
			double offlineEarning = this.FinalFoodPerSecond * totalSeconds;
			totalFood += offlineEarning;

			if (offlineEarning > 0) {
				// msg panel to state offline earning
				MessagePanelController offlineEarningPanel = uiController.NewMessagePanel ();
				offlineEarningPanel.SetTitle ("Offline Earning");
				offlineEarningPanel.SetBody ("While you were away, your workers earned <color=#ff0000ff>"
					+ FormatDouble (offlineEarning) + "</color> of food.\n\nLong Live the Chairman!");
				offlineEarningPanel.SetButtonText ("Long Live!");
				offlineEarningPanel.SetIcon ("Sprites/Upgrade Icons/wheat", Color.red);
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

	public double RedBookMultiplier {
		get {
			return (1 + (double)numRedBooks * redBookMultPerBook * perkRedBookMultMult);
		}
	}

	public double PerkClickProdMult {
		get {
			return this.perkClickProdMult;
		}
	}

	public double PerkAutoProdMult {
		get {
			return this.perkAutoProdMult;
		}
	}

	public double PerkClickAddPercentAuto {
		get {
			return this.perkClickAddPercentAuto;
		}
	}

	public double PerkRedBookGainMult {
		get {
			return this.perkRedBookGainMult;
		}
	}

	public double PerkRedBookMultMult {
		get {
			return this.perkRedBookMultMult;
		}
	}

	public double FinalFoodPerSecond {
		get {
			return (foodPerSecond * perkAutoProdMult) * this.RedBookMultiplier;
		}
	}

	public double FinalFoodPerClick {
		get {
			return (foodPerClick + foodPerSecond * perkAutoProdMult * perkClickAddPercentAuto) * perkClickProdMult * this.RedBookMultiplier;
		}
	}

	public double RedBookMultPerBook {
		get {
			return this.redBookMultPerBook;
		}
	}

	public bool TutorialOn {
		get {
			return this.tutorialOn;
		}
		set {
			tutorialOn = value;
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

	public bool tutorialOn;

	public DateTime timeStamp;

	public Dictionary<int, int> upgradeLevels = new Dictionary<int, int> ();
	public Dictionary<int, int> perkLevels = new Dictionary<int, int> ();
}
