using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanelButtonsScript : MonoBehaviour
{
	
	// UI elements
	private Button redBookButton;
	private Button diamondButton;

	// handles to other controllers
	private UIController uiController;
	private GameController gameController;
	private MessagePanelController msgPanelController;


	void Awake ()
	{
		// store handles to other objects
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		redBookButton = transform.Find ("Red Book Button").GetComponent<Button> ();
		diamondButton = transform.Find ("Diamond Button").GetComponent<Button> ();
		redBookButton.onClick.AddListener (RedBookButtonClick);
		diamondButton.onClick.AddListener (DiamondButtonClick);
	}

	// Use this for initialization
	void Start ()
	{

	}


	void RedBookButtonClick ()
	{
		if (!GameObject.FindGameObjectWithTag ("MsgPanel")) {
			gameController.PlayClickSound2 ();
			if (msgPanelController = uiController.NewMessagePanel ()) {
				msgPanelController.SetTitle ("Red Books");
				msgPanelController.SetBody ("<color=#ff0000ff>The Red Book</color>, also known as <color=#ff0000ff>The Great Chairman's Quotations</color>, contains philosophy that explains the truth of the universe.\n\n" +
					"Having a Red Book will inspire our comrades to work with renewed vigor, providing a <color=#ff0000ff>" +
					(gameController.RedBookMultPerBook * gameController.PerkRedBookMultMult * 100).ToString ("F2") + "%</color> increase to overall production per book.\n\n" +
					"Red Books are obtained by <color=#ff0000ff>Resetting the Game</color>.\n\n" +
				"Current Multiplier: <color=#ff0000ff>" + gameController.FormatMultiplier (gameController.RedBookMultiplier) + "</color>");
				msgPanelController.SetButtonText ("Understood!");
			}
		}
	}

	void DiamondButtonClick ()
	{
		if (!GameObject.FindGameObjectWithTag ("MsgPanel")) {
			gameController.PlayClickSound2 ();
			if (msgPanelController = uiController.NewMessagePanel ()) {
				msgPanelController.SetTitle ("Diamonds");
				msgPanelController.SetBody ("<color=#ff0000ff>Diamonds</color> are the premium currency of this game, obtainable from In-App Purchases.\n\n" +
					"Using diamonds, we will gain access to supernatural powers that allows us to <color=#ff0000ff>produce massive amounts of food instantly</color> " +
					"or to <color=#ff0000ff>reset the game without losing upgrade levels, even gaining bonus Red Books in the process.</color>");
				msgPanelController.SetButtonText ("Buy Diamonds!");
				msgPanelController.CallBackFunctionName = "SwitchToDiamondPurchase";
			}
		}
	}
}
