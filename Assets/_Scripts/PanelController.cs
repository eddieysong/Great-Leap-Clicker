using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
	// handles to other controllers
	private GameController gameController;

	// handles to UI elements displayed
	private Image icon;
	private Text title;
	private Text body;
	private Button button;
	private Text buttonText;

	// upgrade properties
	public int id;
	public int level;
	public string upgradeName;
	public string description;
	public double baseCost = 5f;
	public double increasePerLevel = 1f;

	// upgrade cost follows the formula: Y = baseCost * (1 + costPercentIncreasePerLevel) ^ currentLevel
	// this is A
	//	public double flatIncreasePerLevel = 1f;
	// this is B
	public double costPercentIncreasePerLevel = 0.05f;
	// this is C
	//	public double expFactorPerLevel = 1.02f;

	public double currentCost;

	// Use this for initialization
	void Start ()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		icon = transform.Find ("Icon").GetComponent<Image> ();
		title = transform.Find ("Title").GetComponent<Text> ();
		body = transform.Find ("Body").GetComponent<Text> ();
		button = transform.Find ("Button").GetComponent<Button> ();
		buttonText = button.transform.Find ("Text").GetComponent<Text> ();
		button.onClick.AddListener (ButtonClick);

		RefreshPanelText ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gameController.totalFood >= currentCost) {
			button.interactable = true;
		} else {
			button.interactable = false;
		}
	}

	void RefreshPanelText ()
	{
		CalcCurrentCost ();
		SetTitle (upgradeName + " - Level " + level);
		SetBody (description + "\nProduction: " + gameController.NumberFormat(increasePerLevel) + ((id == 0) ? "/Click" : "/Second"));
		SetButtonText ("BUY\n" + gameController.NumberFormat(currentCost));
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
		if (gameController.totalFood >= currentCost) {
			gameController.totalFood -= currentCost;
			level++;
			if (id == 0) {
				gameController.foodPerClick += increasePerLevel;
			} else {
				gameController.foodPerSecond += increasePerLevel;
			}
			RefreshPanelText ();
		}
	}

	public void CalcCurrentCost ()
	{
		if (id == 0) {
			currentCost = System.Math.Floor (System.Math.Min (baseCost + level, 20) * System.Math.Pow (1 + costPercentIncreasePerLevel, level));
		} else {
			currentCost = System.Math.Floor (baseCost * System.Math.Pow (1 + costPercentIncreasePerLevel, level));
		}
	}
}
