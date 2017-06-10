using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButtonScript : MonoBehaviour {

	// UI elements
	private Button button;
	private Text buttonText;

	// handles to other controllers
	private UIController uiController;
	private GameController gameController;
	private MessagePanelController msgPanelController;

	private long redBooksGained = 0;

	// Use this for initialization
	void Awake () {

		// store handles to other objects
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		button = GetComponent<Button> ();
		buttonText = transform.Find ("Text").GetComponent<Text> ();
		button.onClick.AddListener (ButtonClick);

	}

	void Start () {
		gameObject.SetActive (false);
	}

	public void Activate () {
		gameObject.SetActive (true);
	}

	void Update () {
		if (msgPanelController != null) {
			redBooksGained = gameController.CalcRedBooksGained ();
			msgPanelController.SetBody ("Resetting the game will cause all upgrade levels to be reset back to 0.\n\n " +
				"In exchange, you gain <color=#ff0000ff>" + gameController.FormatLong(redBooksGained) + 
				"</color> Red Books (based on the total food you have produced, plus 1 book every 250 total upgrade levels).\n\n " +
				"Each Red Book increase total production by 10% (plus bonuses), stacking additively.");
		}
	}

	public void ButtonClick () {

		if (!GameObject.FindGameObjectWithTag ("MsgPanel")) {
			gameController.PlayClickSound2 ();
			if (msgPanelController = uiController.NewMessagePanel ()) {
				msgPanelController.SetTitle ("A New Start");
//			msgPanelController.SetBody ("Resetting the game will cause all upgrade levels and skill levels to be reset.\n\n " +
//				"In exchange, you gain Red Books based on the total food you have produced, plus 1 book every 500 total upgrade levels.\n\n " +
//			"Each Red Book increase total production by 10%, stacking additively.");
				msgPanelController.SetButtonText ("Reset Now!");
				msgPanelController.CallBackFunctionName = "ResetGame";
			}
		}
	}
}
