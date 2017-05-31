using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
	// handles to other controllers
	private GameController gameController;
	private UIController uiController;
	private MultipleLevelsButtonScript MLBScript;
	private ResetButtonScript RBScript;

	// handles to UI elements displayed
	private Button panelButton;
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
	void Awake ()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
		MLBScript = GameObject.Find ("Multiple Levels Button").GetComponent<MultipleLevelsButtonScript> ();
		RBScript = GameObject.Find ("Reset Button").GetComponent<ResetButtonScript> ();
//		Debug.Log (id.ToString () + "panel" + MLBScript.Multiplier.ToString () + "mlb loaded");
		panelButton = transform.Find ("Panel Button").GetComponent<Button> ();
		icon = transform.Find ("Icon").GetComponent<Image> ();
		title = transform.Find ("Title").GetComponent<Text> ();
		body = transform.Find ("Body").GetComponent<Text> ();
		button = transform.Find ("Button").GetComponent<Button> ();
		buttonText = button.transform.Find ("Text").GetComponent<Text> ();
		panelButton.onClick.AddListener (PanelButtonClick);
		button.onClick.AddListener (ButtonClick);

	}

	void Start()
	{
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
		currentCost = CalcCurrentCost ();
		currentProduction = CalcCurrentProduction (level);
		gameController.UpdateIncome ();
		SetTitle (upgradeName + " - Level <color=#ff0000ff>" + level + "</color>");
//		SetBody (description + "\nProduction: "
//			+ gameController.FormatDouble(increasePerLevel * gameController.TotalMultiplier * MLBScript.Multiplier)
//			+ ((id == 0) ? "/Click" : "/Second"));
		SetBody ("Current Production: <color=#ff0000ff>"
			+ gameController.FormatDouble(currentProduction * gameController.TotalMultiplier) + "</color>"
			+ ((id == 0) ? "/Click" : "/Second")
			+ "\nIncrease after Buy: <color=#ff0000ff>" 
			+ gameController.FormatDouble((CalcCurrentProduction(level + MLBScript.Multiplier) - currentProduction) * gameController.TotalMultiplier) + "</color>"
			+ ((id == 0) ? "/Click" : "/Second")
			+ ((id == 0) ? "" : "\n<color=#ff0000ff>" + (currentProduction / gameController.FoodPerSecond * 100).ToString("F2") + "%</color> of Total Income/Second"));
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

	public void PanelButtonClick() {
		Debug.Log (id.ToString() + "panel button clicked");
	}

	public void ButtonClick ()
	{
		if (gameController.TotalFood >= currentCost) {
			gameController.SpendFood (currentCost);
			this.Level += MLBScript.Multiplier;

			// refresh all panels
			uiController.UpdateUpgradePanels ();
		}
	}

	public double CalcCurrentCost ()
	{
//		Debug.Log (id.ToString() + "panel" + MLBScript.Multiplier.ToString() + "mlb called");
		return baseCost * System.Math.Pow (1 + costPercentIncreasePerLevel, level) * GeometricSum(MLBScript.Multiplier, 1.0 + costPercentIncreasePerLevel);
	}

	// every 25 levels increase production by 2x, every 100 levels = 10x, every 1000 levels = 100x
	public double CalcCurrentProduction(int level) {
		return increasePerLevel * level * System.Math.Pow (2, (level / 25)) * System.Math.Pow (10, (level / 100)) * System.Math.Pow (100, (level / 1000));
//		Debug.Log (CurrentProduction);
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
		set {
			level = value;
			RefreshPanel ();

			// activates reset button as soon as honey farm is purchased
			if (id == 12 && level > 0) {
				RBScript.Activate ();
			}
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
