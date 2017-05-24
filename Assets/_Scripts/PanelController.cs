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
	public float baseCost = 5f;
	public float increasePerLevel = 1f;

	// upgrade cost follows the formula: Y = baseCost * (1 + costPercentIncreasePerLevel) ^ currentLevel
	// this is A
	//	public float flatIncreasePerLevel = 1f;
	// this is B
	public float costPercentIncreasePerLevel = 0.05f;
	// this is C
	//	public float expFactorPerLevel = 1.02f;

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
		
	}

	void RefreshPanelText ()
	{
		SetTitle (upgradeName + " - Level " + level);
		SetBody (description + "\nIncreases production by " + increasePerLevel.ToString ("F2") + ((id == 0) ? "/Click" : "/Second"));
		SetButtonText ("BUY\n" + GetCurrentCost ().ToString ("F0"));
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
		if (gameController.totalFood >= GetCurrentCost ()) {
			gameController.totalFood -= GetCurrentCost ();
			level++;
			if (id == 0) {
				gameController.foodPerClick += increasePerLevel;
			} else {
				gameController.foodPerSecond += increasePerLevel;
			}
			RefreshPanelText ();
		}
	}

	public float GetCurrentCost ()
	{
		if (id == 0) {
			return Mathf.Floor (Mathf.Min (baseCost + level, 20) * Mathf.Pow (1 + costPercentIncreasePerLevel, level));
		} else {
			return Mathf.Floor (baseCost * Mathf.Pow (1 + costPercentIncreasePerLevel, level));
		}
	}
}
