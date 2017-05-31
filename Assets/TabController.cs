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

	[SerializeField]
	private Sprite buttonImage;
	[SerializeField]
	private Sprite disabledButtonImage;

	// config variables
	[SerializeField]
	private string interfaceControlled;


	void Awake () {
		button = GetComponent<Button> ();
		image = GetComponent<Image> ();
		buttonText = transform.Find ("Text").GetComponent<Text> ();
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
