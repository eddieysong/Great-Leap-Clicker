using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostController : MonoBehaviour {

	// handles to other controllers
	private GameController gameController;
	private UIController uiController;
	private MessagePanelController msgPanelController;

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
	// id 0: time warp 1 hr
	// id 1: time warp 8 hrs
	// id 2: time warp 72 hrs
	// id 10: reset but keep levels
	// id 11: reset but keep levels and gain 2x red books
	// id 12: reset but keep levels and gain 8x red books
	private int id;
	[SerializeField]
	private string boostName;
	[SerializeField]
	private string description;
	[SerializeField]
	private string functionDescription;
	[SerializeField]
	private int cost = 25;
	[SerializeField]
	private int multiplier = 1;

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
		if (gameController.NumDiamonds >= cost) {
			button.interactable = true;
		} else {
			button.interactable = false;
		}

		if (msgPanelController != null) {
			
			// time warp selected
			if (id < 10) {
				msgPanelController.SetBody ("By spending <color=#ff0000ff>" + cost.ToString () + "</color> diamonds, you can leap forward <color=#ff0000ff>"
				+ multiplier.ToString () + "</color> hours in time and gain <color=#ff0000ff>"
				+ gameController.FormatDouble (gameController.FinalFoodPerSecond * 3600 * multiplier)
				+ "</color> of food immediately.");
			}

			// diamond reset selected
			else if (id < 20) {
				msgPanelController.SetBody ("By spending <color=#ff0000ff>" + cost.ToString() + "</color> diamonds, you can reset and gain <color=#ff0000ff>" 
					+ gameController.FormatLong(gameController.CalcRedBooksGained () * multiplier) 
					+ "</color> Red Books (Bonus: <color=#ff0000ff>" + ((multiplier - 1) * 100).ToString() + "%</color>) without losing your upgrade levels.\n\n " 
					+ "Each Red Book increase total production by 10% (plus bonuses), stacking additively.");
			}
		}
	}

	// recalculates and displays this panel's information
	public void RefreshPanel ()
	{
		gameController.UpdatePerkMultipliers ();

		SetTitle ("<color=#ff0000ff>" + boostName + "</color>");

		SetBody (functionDescription);

		SetButtonText (gameController.FormatLong (cost));

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
		msgPanel.SetTitle ("<color=#ff0000ff>" + boostName + "</color>");
		msgPanel.SetBody(description
			+ "\n\nLong Live the Chairman!");
		msgPanel.SetIcon (icon);
		msgPanel.SetButtonText ("Long Live!");

	}

	public void ButtonClick ()
	{
		if (gameController.NumDiamonds >= cost) {
			switch (id) {
			case 0:
			case 1:
			case 2:
				if (!GameObject.FindGameObjectWithTag ("MsgPanel")) {
					if (msgPanelController = uiController.NewMessagePanel ()) {
						msgPanelController.SetTitle ("<color=#ff0000ff>" + boostName + "</color>");
						msgPanelController.SetButtonText ("Buy Now!");
						msgPanelController.CallBackFunctionName = "LeapForward";
						msgPanelController.CallBackParameter = new int[] {cost, multiplier};
					}
				}
				// gameController.LeapForward (multiplier);
				break;
			case 10:
			case 11:
			case 12:
				if (!GameObject.FindGameObjectWithTag ("MsgPanel")) {
					if (msgPanelController = uiController.NewMessagePanel ()) {
						msgPanelController.SetTitle ("<color=#ff0000ff>" + boostName + "</color>");
						msgPanelController.SetButtonText ("Buy Now!");
						msgPanelController.CallBackFunctionName = "Revolution";
						msgPanelController.CallBackParameter = new int[] {cost, multiplier};
					}
				}
				// gameController.Revolution (multiplier);
				break;
			}
		}
	}

	// Setters and getters
	public int Id {
		get {
			return this.id;
		}
	}

	public string BoostName {
		get {
			return this.boostName;
		}
	}

	public string Description {
		get {
			return this.description;
		}
	}

	public int Cost {
		get {
			return this.cost;
		}
	}

}
