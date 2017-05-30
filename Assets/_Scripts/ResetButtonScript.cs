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
		
	}

	void Update () {
		if (msgPanelController != null) {
			redBooksGained = gameController.CalcRedBooksGained ();
			msgPanelController.SetBody ("Resetting the game will cause all upgrade levels and skill levels to be reset.\n\n " +
				"In exchange, you gain " + redBooksGained.ToString() + " Red Books based on the total food you have produced.\n\n " +
				"Each Red Book increase total production by 10%, stacking additively.");
		}
	}

	public void ButtonClick () {

		if (msgPanelController = uiController.NewMessagePanel ()) {
			msgPanelController.SetTitle ("A New Start");
			msgPanelController.SetBody ("Resetting the game will cause all upgrade levels and skill levels to be reset.\n\n " +
				"In exchange, you gain " + redBooksGained.ToString() + " Red Books based on the total food you have produced.\n\n " +
			"Each Red Book increase total production by 10%, stacking additively.");
			msgPanelController.SetButtonText ("Reset Now!");
			msgPanelController.CallBackParameter = "ResetGame";
		}
	}
}
