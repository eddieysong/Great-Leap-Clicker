using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkController : MonoBehaviour {

	// handles to other controllers
	private GameController gameController;
	private UIController uiController;

	// handles to UI elements displayed
	private Button panelButton;
	private Image icon;
	private Text title;
	private Text body;
	private Button button;
	private Text buttonText;

	// upgrade properties
	[SerializeField]
	// perk multipliers
	// id 0: perkClickProdMult = 0;
	// id 1: perkAutoProdMult = 0;
	// id 2: perkClickAddPercentAuto = 0;
	// id 3: perkRedBookGainMult = 0;
	// id 4: perkRedBookMultMult = 0;
	private int id;
	//	[SerializeField]
	private int level;
	[SerializeField]
	private string perkName;
	[SerializeField]
	private string description;
	[SerializeField]
	private string functionDescription;
		[SerializeField]
	private double baseCost = 25f;
		[SerializeField]
	private double increasePerLevel = 1f;

	// upgrade cost follows the formula: Y = Min(currentLevel, baseCost) * (1 + costPercentIncreasePerLevel) ^ currentLevel
	private double costPercentIncreasePerLevel = 0.15f;

	// Use this for initialization
	void Awake ()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
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
		RefreshPanel ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (gameController.NumRedBooks >= this.CurrentCost) {
			button.interactable = true;
		} else {
			button.interactable = false;
		}
	}

	// recalculates and displays this panel's information
	public void RefreshPanel ()
	{
		gameController.UpdatePerkMultipliers ();

		SetTitle (perkName + " - Level <color=#ff0000ff>" + level + "</color>");

		SetBody (functionDescription
			+ "\nCurrent Increase: <color=#ff0000ff>"
			+ ((id == 2) ? (this.CurrentValue * 100).ToString("F1") : (this.CurrentValue * 100).ToString("F0"))
			+ "%</color>");

		SetButtonText (gameController.FormatLong (this.CurrentCost));

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
		MessagePanelController msgPanel = uiController.NewMessagePanel ();
		msgPanel.SetTitle (perkName);
		msgPanel.SetBody(description
			+ "\n\nCurrent Level: <color=#ff0000ff>" + level.ToString() + "</color>"
			+ "\nIncrease/Level: <color=#ff0000ff>"
			+ ((id == 2) ? (this.increasePerLevel * 100).ToString("F1") : (this.increasePerLevel * 100).ToString("F0")) + "%</color>"
			+ "\nTotal Increase: <color=#ff0000ff>"
			+ ((id == 2) ? (this.CurrentValue * 100).ToString("F1") : (this.CurrentValue * 100).ToString("F0")) + "%</color>"
			+ "\n\nLong Live the Chairman!");
		msgPanel.SetIcon (icon);
		msgPanel.SetButtonText ("Long Live!");
	}

	public void ButtonClick ()
	{
		if (gameController.NumRedBooks >= this.CurrentCost) {
			gameController.IncreaseRedBooks (-this.CurrentCost);
			this.Level++;
			gameController.UpdatePerkMultipliers();
			// refresh all panels
			uiController.UpdateAllPanels ();
		}
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
//				RBScript.Activate ();
			}
		}
	}

	public string UpgradeName {
		get {
			return this.perkName;
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

	// upgrade cost follows the formula: Y = Min(currentLevel, baseCost) * (1 + costPercentIncreasePerLevel) ^ currentLevel
	public long CurrentCost {
		get {
			return System.Convert.ToInt64(System.Math.Min(level + 1, baseCost) * System.Math.Pow (1 + costPercentIncreasePerLevel, level));
		}
	}

	public double CurrentValue {
		get {
			return increasePerLevel * level;
		}
	}
}
