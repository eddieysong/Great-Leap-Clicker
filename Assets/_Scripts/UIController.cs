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
	}
	
	// Update is called once per frame
	void Update () {
		totalDisplay.text = gameController.NumberFormat(gameController.totalFood);
		perClickDisplay.text = gameController.NumberFormat(gameController.foodPerClick * gameController.totalMultiplier);
		perSecDisplay.text = gameController.NumberFormat(gameController.foodPerSecond * gameController.totalMultiplier);
	}

//	public void UpdateNumbers () {
//		
//	}
//
//	public void AddPanel (int id) {
//		GameObject newPanel = Instantiate (panelPrefab);
//		newPanel.transform.SetParent (viewportContent.transform);
//		newPanel.transform.localScale = new Vector3 (1f, 1f, 1f);
//		newPanel.transform.localPosition = new Vector2 (initialXPos, initialYPos - panelHeight * id);
//	}
}
