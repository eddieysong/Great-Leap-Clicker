using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
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
	//	[SerializeField]
	private int level;
	[SerializeField]
	private string upgradeName;
	[SerializeField]
	private string description;
	//	[SerializeField]
	private double baseCost = 5f;
	//	[SerializeField]
	private double increasePerLevel = 1f;

	// upgrade cost follows the formula: Y = baseCost * (1 + costPercentIncreasePerLevel) ^ currentLevel
	// this is A
	//	public double flatIncreasePerLevel = 1f;
	// this is B
	//	[SerializeField]
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

	void Start ()
	{
		// setting upgrade values dynamically through a formula
		if (id != 0) {
			baseCost = 50 * Factorial (id) * System.Math.Pow (2, id - 1);
			increasePerLevel = (0.1 * System.Math.Pow (0.5, id)) * baseCost;
		}
		RefreshPanel ();
	}

	// helper function
	long Factorial (long n)
	{
		if (n >= 2)
			return n * Factorial (n - 1);
		return 1;
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
//		Debug.Log ("currentProduction" + currentProduction);
		gameController.UpdateIncome ();
		SetTitle (upgradeName + " - Level <color=#ff0000ff>" + level + "</color>");

		if (id == 0) {
			SetBody ("Current Production: <color=#ff0000ff>"
			+ gameController.FormatDouble (currentProduction * gameController.RedBookMultiplier * gameController.PerkClickProdMult) + "</color>/Click"
			+ "\nIncrease after Buy: <color=#ff0000ff>"
			+ gameController.FormatDouble ((CalcCurrentProduction (level + MLBScript.Multiplier) - currentProduction) * gameController.RedBookMultiplier * gameController.PerkClickProdMult) + "</color>/Click");
		} else {
			SetBody ("Current Production: <color=#ff0000ff>"
			+ gameController.FormatDouble (currentProduction * gameController.RedBookMultiplier * gameController.PerkAutoProdMult) + "</color>/Second"
			+ "\nIncrease after Buy: <color=#ff0000ff>"
			+ gameController.FormatDouble ((CalcCurrentProduction (level + MLBScript.Multiplier) - currentProduction) * gameController.RedBookMultiplier * gameController.PerkAutoProdMult) + "</color>/Second"
			+ "\n<color=#ff0000ff>" + ((gameController.FoodPerSecond == 0) ? 0 : (currentProduction / gameController.FoodPerSecond * 100)).ToString ("F2")
			+ "%</color> of Total Income/Second");
		}


		SetButtonText ("BUY\n" + gameController.FormatDouble (currentCost));

	}

	public void SetImage (string filePath)
	{
		icon.sprite = Resources.Load<Sprite> (filePath);
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

	public void PanelButtonClick ()
	{
		if (!GameObject.FindGameObjectWithTag ("MsgPanel")) {
			MessagePanelController msgPanel = uiController.NewMessagePanel ();
			msgPanel.SetTitle (upgradeName);
			msgPanel.SetBody (description
			+ "\n\nCurrent Level: <color=#ff0000ff>" + level.ToString () + "</color>"
			+ "\nBase Increase/Level: <color=#ff0000ff>"
			+ gameController.FormatDouble (increasePerLevel) + "</color>"
			+ ((id == 0) ? "/Click" : "/Second")
			+ "\nGlobal Multiplier: <color=#ff0000ff>"
			+ gameController.FormatMultiplier (gameController.RedBookMultiplier) + "</color>"
			+ "\nUpgrade Level Multiplier: <color=#ff0000ff>"
			+ gameController.FormatMultiplier (CalcLevelMultiplier (level)) + "</color>"
			+ "\nCurrent Increase/Level: <color=#ff0000ff>"
			+ gameController.FormatDouble (increasePerLevel * gameController.RedBookMultiplier * CalcLevelMultiplier (level)) + "</color>"
			+ ((id == 0) ? "/Click" : "/Second")
			+ "\n\nLong Live the Chairman!");
			msgPanel.SetIcon (icon);
			msgPanel.SetButtonText ("Long Live!");
		}
	}

	public void ButtonClick ()
	{
		if (gameController.TotalFood >= currentCost) {
			gameController.SpendFood (currentCost);
			this.Level += MLBScript.Multiplier;

			// refresh all panels
			uiController.UpdateAllPanels ();
		}
	}

	public double CalcCurrentCost ()
	{
		//		Debug.Log (id.ToString() + "panel" + MLBScript.Multiplier.ToString() + "mlb called");
		return baseCost * System.Math.Pow (1 + costPercentIncreasePerLevel, level) * GeometricSum (MLBScript.Multiplier, 1.0 + costPercentIncreasePerLevel);
	}

	// every 25 levels increase production by a factor of 2x, every 100 levels = 4x, every 250 levels = 10x, every 1000 levels = 100x
	public double CalcCurrentProduction (int level)
	{
		return increasePerLevel * level * CalcLevelMultiplier (level);
	}

	// every 25 levels increase production by a factor of 2x, every 100 levels = 4x, every 250 levels = 10x, every 1000 levels = 100x
	public double CalcLevelMultiplier (int level)
	{
		return System.Math.Pow (2, (level / 25)) * System.Math.Pow (4, (level / 100)) * System.Math.Pow (10, (level / 250)) * System.Math.Pow (100, (level / 1000));
	}

	// helper method, returns the sum of the first n terms of a geometric series with ratio r
	private double GeometricSum (int n, double r)
	{
		return (1.0 - System.Math.Pow (r, n)) / (1.0 - r);
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
