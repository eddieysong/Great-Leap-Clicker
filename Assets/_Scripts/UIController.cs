using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	// upgrade panel prefab
	[SerializeField]
	private GameObject panelPrefab;

	// handles to other controllers
	private GameController gameController;

	// handles to UI elements displayed
	private Text totalDisplay;
	private Text perClickDisplay;
	private Text perSecDisplay;
	private GameObject viewportContent;

	private Text redBookDisplay;
	private Text diamondDisplay;

	// config variables
	private double initialXPos = 500f;
	private double initialYPos = -100f;
	private double panelHeight = 200f;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		totalDisplay = GameObject.Find ("Total Display").GetComponent<Text> ();
		perClickDisplay = GameObject.Find ("PerClick Display").GetComponent<Text> ();
		perSecDisplay = GameObject.Find ("PerSec Display").GetComponent<Text> ();
		viewportContent = GameObject.Find ("Viewport/Content");
		redBookDisplay = GameObject.Find ("Top Panel/Red Book Display").GetComponent<Text> ();
		diamondDisplay= GameObject.Find ("Top Panel/Diamond Display").GetComponent<Text> ();

	}
	
	// Update is called once per frame
	void Update () {
		totalDisplay.text = gameController.FormatDouble(gameController.TotalFood);
		perClickDisplay.text = gameController.FormatDouble(gameController.FoodPerClick * gameController.TotalMultiplier);
		perSecDisplay.text = gameController.FormatDouble(gameController.FoodPerSecond * gameController.TotalMultiplier);
		redBookDisplay.text = gameController.FormatLong(gameController.NumRedBooks);
		diamondDisplay.text = "0";
	}

	public void UpdateUpgradePanels () {
		// updates all panels
		foreach (PanelController panel in GameObject.Find("Main Upgrade Interface/Viewport/Content").transform.GetComponentsInChildren<PanelController>()) {
			panel.RefreshPanel ();
		}
	}
//
//	public void AddPanel (int id) {
//		GameObject newPanel = Instantiate (panelPrefab);
//		newPanel.transform.SetParent (viewportContent.transform);
//		newPanel.transform.localScale = new Vector3 (1f, 1f, 1f);
//		newPanel.transform.localPosition = new Vector2 (initialXPos, initialYPos - panelHeight * id);
//	}
}
