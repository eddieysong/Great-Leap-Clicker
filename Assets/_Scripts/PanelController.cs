using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
	// handles to other controllers
	private GameController gameController;
	private MultipleLevelsButtonScript MLBScript;

	// handles to UI elements displayed
	private Image icon;
	private Text title;
	private Text body;
	private Button button;
	private Text buttonText;

	// upgrade properties
	[SerializeField]
	// id = 0 means click upgrade, id >= 1 && id <= 12 means per second income upgrade
	private int id;
	[SerializeField]
	private int level;
	[SerializeField]
	private string upgradeName;
	[SerializeField]
	private string description;
	[SerializeField]
	private double baseCost = 5f;
	[SerializeField]
	private double increasePerLevel = 1f;

	// upgrade cost follows the formula: Y = baseCost * (1 + costPercentIncreasePerLevel) ^ currentLevel
	// this is A
	//	public double flatIncreasePerLevel = 1f;
	// this is B
	[SerializeField]
	private double costPercentIncreasePerLevel = 0.07f;
	// this is C
	//	public double expFactorPerLevel = 1.02f;

	private double currentCost;
	private double currentProduction;

	// Use this for initialization
	void Start ()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		MLBScript = GameObject.Find ("Multiple Levels Button").GetComponent<MultipleLevelsButtonScript> ();
		icon = transform.Find ("Icon").GetComponent<Image> ();
		title = transform.Find ("Title").GetComponent<Text> ();
		body = transform.Find ("Body").GetComponent<Text> ();
		button = transform.Find ("Button").GetComponent<Button> ();
		buttonText = button.transform.Find ("Text").GetComponent<Text> ();
		button.onClick.AddListener (ButtonClick);

		RefreshPanel ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gameController.TotalFood >= currentCost) {
			button.interactable = true;
		} else {
			button.interactable = false;
		}
	}

	// recalculates and displays this panel's information
	public void RefreshPanel ()
	{
		CalcCurrentCost ();
		SetTitle (upgradeName + " - Level " + level);
		SetBody (description + "\nProduction: "
			+ gameController.FormatDouble(increasePerLevel * gameController.TotalMultiplier * MLBScript.Multiplier)
			+ ((id == 0) ? "/Click" : "/Second"));
		SetButtonText ("BUY\n" + gameController.FormatDouble(currentCost));
	}

	public void SetImage ()
	{

	}

	public void SetTitle (string title)
	{
		this.title.text = title;
	}

	public void SetBody (string body)
	{
		this.body.text = body;
	}

	public void SetButtonText (string text)
	{
		this.buttonText.text = text;
	}

	public void ButtonClick ()
	{
		if (gameController.TotalFood >= currentCost) {
			gameController.SpendFood (currentCost);
			level += MLBScript.Multiplier;
			CalcCurrentProduction ();
			gameController.UpdateIncome ();
			RefreshPanel ();
		}
	}

	public void CalcCurrentCost ()
	{
//		if (id == 0) {
//			currentCost = gameController.FormatDouble (System.Math.Min (baseCost + level, 20) * System.Math.Pow (1 + costPercentIncreasePerLevel, level));
//		} else {

		currentCost = baseCost * System.Math.Pow (1 + costPercentIncreasePerLevel, level) * GeometricSum(MLBScript.Multiplier, 1.0 + costPercentIncreasePerLevel);
	}

	// every 25 levels increase production by 2x, every 100 levels = 10x, every 1000 levels = 100x
	public void CalcCurrentProduction() {
		currentProduction = increasePerLevel * level * System.Math.Pow (2, (level / 25)) * System.Math.Pow (10, (level / 100)) * System.Math.Pow (100, (level / 1000));
		Debug.Log (CurrentProduction);
	}

	// helper method, returns the sum of the first n terms of a geometric series with ratio r
	private double GeometricSum (int n, double r) {
		return (1.0 - System.Math.Pow(r, n)) / (1.0 - r);
	}

	// Setters and getters
	public int Id {
		get {
			return this.id;
		}
	}

	public int Level {
		get {
			return this.level;
		}
	}

	public string UpgradeName {
		get {
			return this.upgradeName;
		}
	}

	public string Description {
		get {
			return this.description;
		}
	}

	public double BaseCost {
		get {
			return this.baseCost;
		}
	}

	public double IncreasePerLevel {
		get {
			return this.increasePerLevel;
		}
	}

	public double CostPercentIncreasePerLevel {
		get {
			return this.costPercentIncreasePerLevel;
		}
	}

	public double CurrentCost {
		get {
			return this.currentCost;
		}
	}

	public double CurrentProduction {
		get {
			return this.currentProduction;
		}
	}
}
