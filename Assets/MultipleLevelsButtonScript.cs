using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleLevelsButtonScript : MonoBehaviour {

	// level multiplier when buying, player can buy 1, 10, 25 or 100 levels at once.
	private int multiplier = 1;

	// UI elements
	private Button button;
	private Text buttonText;

	// handles to other controllers
	private UIController uiController;

	// Use this for initialization
	void Awake () {
		
		// store handles to other objects
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
		button = GetComponent<Button> ();
		buttonText = transform.Find ("Text").GetComponent<Text> ();
		buttonText.text = "x1";
		button.onClick.AddListener (ButtonClick);
	}

	public void ButtonClick () {
		
		// updates multiplier button
		if (multiplier == 1) {
			multiplier = 10;
		} else if (multiplier == 10) {
			multiplier = 25;
		} else if (multiplier == 25) {
			multiplier = 100;
		} else if (multiplier == 100) {
			multiplier = 1;
		}
		buttonText.text = "x" + multiplier.ToString();

		uiController.UpdateUpgradePanels ();
	}

	// Getters and setters
	public int Multiplier {
		get {
			return this.multiplier;
		}
	}
}
