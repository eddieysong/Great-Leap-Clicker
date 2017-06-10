using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour {

	// UI elements
	private Button button;
	private Text buttonText;
	private Image image;
	private GameObject[] scrollableInterfaces;
	private GameObject[] tabs;
	private GameObject multiLevelButton;

	[SerializeField]
	private Sprite buttonImage;
	[SerializeField]
	private Sprite disabledButtonImage;

	// config variables
	[SerializeField]
	private string interfaceControlled;

	private GameController gameController;
	private TutorialController tutorialController;

	void Awake () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		tutorialController = GameObject.Find ("TutorialController").GetComponent<TutorialController> ();
		button = GetComponent<Button> ();
		image = GetComponent<Image> ();
		buttonText = transform.Find ("Text").GetComponent<Text> ();
		multiLevelButton = GameObject.Find ("Multiple Levels Button");
	}


	// Use this for initialization
	void Start () {
		button.onClick.AddListener (ButtonClick);
		scrollableInterfaces = GameObject.FindGameObjectsWithTag ("ScrollableInterface");
		tabs = GameObject.FindGameObjectsWithTag ("Tabs");
//		foreach (GameObject tab in tabs) {
//			Debug.Log (tab.name);
//		}

	}
	
	public void ButtonClick () {
		gameController.PlayClickSound2 ();

		foreach (GameObject sInterface in scrollableInterfaces) {
			if (sInterface.name != interfaceControlled) {
				sInterface.SetActive (false);
			} else {
				sInterface.SetActive (true);
			}
		}
		SwapImage (true);
		foreach (GameObject tab in tabs) {
			if (tab.GetComponent<TabController> ().InterfaceControlled != interfaceControlled) {
				tab.GetComponent<TabController> ().SwapImage (false);
			}
		}

		// enable mult-level button if buying upgrades
		if (interfaceControlled == "Main Upgrade Interface") {
			multiLevelButton.SetActive (true);
		} else {
			multiLevelButton.SetActive (false);
		}

		// tutorial panel pop-ups
		if (interfaceControlled == "Main Perk Interface") {
			tutorialController.SendMessage ("TutorialPerks", SendMessageOptions.DontRequireReceiver);
		} else if (interfaceControlled == "Main Boost Interface") {
			tutorialController.SendMessage ("TutorialBoosts", SendMessageOptions.DontRequireReceiver);
		} else if (interfaceControlled == "Main IAP Interface") {
			tutorialController.SendMessage ("TutorialDiamonds", SendMessageOptions.DontRequireReceiver);
		}
	}

	// sets the button image to "enabled" or "disabled" depending on the parameter
	public void SwapImage(bool active) {
		if (active) {
			image.sprite = buttonImage;
		} else {
			image.sprite = disabledButtonImage;
		}
	}

	// getters/setters
	public string InterfaceControlled {
		get {
			return this.interfaceControlled;
		}
	}
}
