using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {
	
	// handles to other controllers
	private UIController uiController;
	private GameController gameController;

	// flags to control tutorial pop-ups, false if not viewed, true if already viewed
	private bool tBegin, tGameObjective, tFirstClick, tClickUpgrade, tClickUpgradeDone, tAutoUpgrade, tAutoUpgradeDone, tLevelMult, tPerks, tBoosts, tReset, tDiamonds;

	void Awake() {
		// store handles to other objects
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}

	void Start() {
		this.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if (gameController.TutorialOn) {
			if (!tBegin) {
				TutorialBegin ();
			}
			if (!tGameObjective) {
				TutorialGameObjective ();
			}
			if (!tFirstClick) {
				TutorialFirstClick ();
			}
			if (!tClickUpgrade && gameController.TotalFood >= 5) {
				TutorialClickUpgrade ();
			}
			if (!tAutoUpgrade && gameController.TotalFood >= 50) {
				TutorialAutoUpgrade ();
			}
		}
	}

	void TutorialBegin () {
		MessagePanelController msgPanel = uiController.NewMessagePanel ();
		msgPanel.SetTitle ("Welcome");
		msgPanel.SetBody ("Comrade, welcome to the Great Leap Clicker!\n\n" +
			"Through out the game, panels like this one will pop up and guide you through the great Socialist path we are taking.\n\n" +
			"If there's too much information on the panel, you can swipe up anywhere in the content to scroll up and down!\n\n" +
			"Give it a try!\n\n\n\n\n\n\n\n" +
			"There you go!\n\nClick the button or anywhere outside the panel to close this panel.");
		msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
		msgPanel.SetButtonText("Understood!");
		tBegin = true;
	}

	void TutorialGameObjective () {
		MessagePanelController msgPanel = uiController.NewMessagePanel ();
		msgPanel.SetTitle ("Our Mission");
		msgPanel.SetBody ("Comrade, our country is in need!\n\n" +
			"The Chairman has set glorious goals for us! We are going to develop our country so quickly that we'll surpass the U.S. and the U.K in 10 years!\n\n" +
			"In order to do that, we need to produce as much food as possible!\n\n");
		msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
		msgPanel.SetButtonText("Understood!");
		tGameObjective = true;
	}

	void TutorialFirstClick () {
		MessagePanelController msgPanel = uiController.NewMessagePanel ();
		msgPanel.SetTitle ("First Steps");
		msgPanel.SetBody ("Click anywhere in the top half of the screen to produce food!\n\n" +
			"Go on, give it a few tries!");
		msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
		msgPanel.SetButtonText("Understood!");
		tFirstClick = true;
	}

	void TutorialClickUpgrade () {
		MessagePanelController msgPanel = uiController.NewMessagePanel ();
		msgPanel.SetTitle ("Upgrade!");
		msgPanel.SetBody ("Comrade, it looks like you have produced enough food for an upgrade!\n\n" +
			"In the Upgrades tab, you can find the <color=#ff0000ff>Harvest Click</color> upgrade. Click on <color=#ff0000ff>BUY</color> to buy the upgrade!");
		msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
		msgPanel.SetButtonText("Understood!");
		tClickUpgrade = true;
	}

	void TutorialClickUpgradeDone () {
		if (!tClickUpgradeDone && gameController.TutorialOn) {
			MessagePanelController msgPanel = uiController.NewMessagePanel ();
			msgPanel.SetTitle ("Upgrade Purchased");
			msgPanel.SetBody ("Comrade, you have bought your first upgrade! Upgrading <color=#ff0000ff>Harvest Click</color> enables you to produce more food with each Click!\n\n" +
			"Hint: You can click on each upgrade to view its details!");
			msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
			msgPanel.SetButtonText ("Understood!");
			tClickUpgradeDone = true;
		}
	}

	void TutorialAutoUpgrade () {
		MessagePanelController msgPanel = uiController.NewMessagePanel ();
		msgPanel.SetTitle ("Auto-Production");
		msgPanel.SetBody ("Comrade, you have enough food to purchase an auto-production upgrade!\n\n" +
			"In the Upgrades tab, you can find the <color=#ff0000ff>Wheat Field</color> upgrade. Click on <color=#ff0000ff>BUY</color> to buy the upgrade!");
		msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
		msgPanel.SetButtonText("Understood!");
		tAutoUpgrade = true;
	}

	void TutorialAutoUpgradeDone () {
		if (!tAutoUpgradeDone && gameController.TutorialOn) {
			MessagePanelController msgPanel = uiController.NewMessagePanel ();
			msgPanel.SetTitle ("Auto-Production");
			msgPanel.SetBody ("Comrade, you have bought <color=#ff0000ff>Wheat Field</color>, an auto-production upgrade, and now we can produce food automatically!\n\n" +
				"Auto-production will stay in effect even when you are offline!\n\nHint: You can click on each upgrade to view its details!");
			msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
			msgPanel.SetButtonText ("Understood!");
			tAutoUpgradeDone = true;
		}
	}

	void TutorialLevelMult () {
		if (!tLevelMult && gameController.TutorialOn) {
			MessagePanelController msgPanel = uiController.NewMessagePanel ();
			msgPanel.SetTitle ("Level Multiplier");
			msgPanel.SetBody ("Comrade, one of your upgrades has gained a level multiplier!\n\n" +
			"Upgrades will gain level multipliers every time you reach a milestone in upgrade level.\n\n" +
			"Every 25 levels increases the upgrade's production by 50%, every 100 levels doubles production, every 250 levels quadruples production, and every 1000 levels increases production ten-fold!");
			msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
			msgPanel.SetButtonText ("Understood!");
			tLevelMult = true;
		}
	}

	void TutorialPerks () {
		if (!tPerks && gameController.TutorialOn) {
			MessagePanelController msgPanel = uiController.NewMessagePanel ();
			msgPanel.SetTitle ("Perks");
			msgPanel.SetBody ("Comrade, it looks like you have found the Perks tab!\n\n" +
			"Perks can only be purchased with Red Books, but they provide special bonuses!\n\n" +
			"For example, you can purchase perk levels to increase the number of Red Books gained through reset!");
			msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
			msgPanel.SetButtonText ("Understood!");
			tPerks = true;
		}
	}

	void TutorialBoosts () {
		if (!tBoosts && gameController.TutorialOn) {
			MessagePanelController msgPanel = uiController.NewMessagePanel ();
			msgPanel.SetTitle ("Boosts");
			msgPanel.SetBody ("Comrade, it looks like you have found the Boosts tab!\n\n" +
			"Boosts can only be purchased with Diamonds, the game's premium currency.\n\n" +
			"Boosts provide powerful utility, such as instantly gaining massive amounts of food, or resetting the game while keeping your upgrade levels!");
			msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
			msgPanel.SetButtonText ("Understood!");
			tBoosts = true;
		}
	}

	void TutorialReset () {
		if (!tReset && gameController.TutorialOn) {
			MessagePanelController msgPanel = uiController.NewMessagePanel ();
			msgPanel.SetTitle ("Reset");
			msgPanel.SetBody ("Comrade, it looks like your have bought the Honey Farm!\n\n" +
			"Buying the Honey Farm signifies the end of the Great Leap... But fear not!\n\n" +
			"We can reset the game to gain Red Books, which would provide powerful increases in production, and allow us to get even further during our next run!\n\n" +
			"Resetting the game means you lose all upgrade levels, but you'll progress much faster next time, and get further down the road of Socialism!");
			msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
			msgPanel.SetButtonText ("Understood!");
			tReset = true;
		}
	}

	void TutorialDiamonds () {
		if (!tDiamonds && gameController.TutorialOn) {
			MessagePanelController msgPanel = uiController.NewMessagePanel ();
			msgPanel.SetTitle ("Purchasing Diamonds");
			msgPanel.SetBody ("Comrade, it looks like you have discovered how to purchase Diamonds!\n\n" +
			"Diamonds can only be purchased with real money.\n\n" +
			"Diamonds can be used to purchase powerful boosts, such as instantly gaining massive amounts of food, or resetting the game while keeping your upgrade levels!");
			msgPanel.SetIcon ("Sprites/UI/info", Color.yellow);
			msgPanel.SetButtonText ("Understood!");
			tDiamonds = true;
		}
	}
		
}
