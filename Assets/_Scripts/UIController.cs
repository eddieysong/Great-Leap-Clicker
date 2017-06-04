using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	// upgrade panel prefab
	[SerializeField]
	private GameObject panelPrefab;

	// msg panel prefab
	[SerializeField]
	private GameObject msgPanel;
	private List<GameObject> msgPanelQueue;

	// handles to other controllers
	private GameController gameController;
	private UpgradeController[] upgradePanels;
	private PerkController[] perkPanels;

	// handles to UI elements displayed
	private Text totalDisplay;
	private Text perClickDisplay;
	private Text perSecDisplay;

	private Text redBookDisplay;
	private Text diamondDisplay;

	// config variables
	private double initialXPos = 500f;
	private double initialYPos = -100f;
	private double panelHeight = 200f;

	// Use this for initialization
	void Awake () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		upgradePanels = GameObject.Find ("Main Upgrade Interface/Viewport/Content").transform.GetComponentsInChildren<UpgradeController> ();
		perkPanels = GameObject.Find ("Main Perk Interface/Viewport/Content").transform.GetComponentsInChildren<PerkController> ();

		totalDisplay = GameObject.Find ("Total Display").GetComponent<Text> ();
		perClickDisplay = GameObject.Find ("PerClick Display").GetComponent<Text> ();
		perSecDisplay = GameObject.Find ("PerSec Display").GetComponent<Text> ();
		redBookDisplay = GameObject.Find ("Top Panel/Red Book Display").GetComponent<Text> ();
		diamondDisplay= GameObject.Find ("Top Panel/Diamond Display").GetComponent<Text> ();


	}
	
	// Update is called once per frame
	void Update () {
		totalDisplay.text = gameController.FormatDouble(gameController.TotalFood);
		perClickDisplay.text = gameController.FormatDouble(gameController.FoodPerClick * gameController.RedBookMultiplier);
		perSecDisplay.text = gameController.FormatDouble(gameController.FoodPerSecond * gameController.RedBookMultiplier);
		redBookDisplay.text = gameController.FormatLong(gameController.NumRedBooks);
		diamondDisplay.text = gameController.NumDiamonds.ToString();
	}

	public void UpdateAllPanels () {
		// updates all panels
		foreach (UpgradeController panel in upgradePanels) {
			panel.RefreshPanel ();
		}
		foreach (PerkController panel in perkPanels) {
			panel.RefreshPanel ();
		}
	}

	public MessagePanelController NewMessagePanel () {

		bool msgPanelExists = GameObject.FindGameObjectWithTag ("MsgPanel");

		GameObject newMsgPanel = Instantiate (msgPanel);
		newMsgPanel.transform.SetParent (GameObject.FindGameObjectWithTag ("UICanvas").transform);

		MessagePanelController msgPanelController = newMsgPanel.GetComponent<MessagePanelController> ();

		if (!msgPanelExists) {
			msgPanelController.PopUp ();
		}

		return msgPanelController;


	}

	public void MessagePanelDestroyed() {
		Debug.Log (" MessagePanelDestroyed called");
		GameObject msgPanel = GameObject.FindGameObjectWithTag ("MsgPanel");
			if (msgPanel) {
				msgPanel.SendMessage ("PopUp", SendMessageOptions.DontRequireReceiver);
			}
	}



	// getters/setters
	public GameObject MsgPanel {
		get {
			return this.msgPanel;
		}
	}
}
