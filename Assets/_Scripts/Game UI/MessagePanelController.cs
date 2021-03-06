﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanelController : MonoBehaviour {
	
	// handles to other controllers
	private GameController gameController;
	private UIController uiController;
	private Animator animator;

	// handles to UI elements displayed
	private Image icon;
	private Text title;
	private Text body;
	private Button button;
	private Text buttonText;
	private Button outside;

	private ScrollRect scrollRect;

	private string callBackFunctionName;
	private int [] callBackParameter;

	// Use this for initialization
	void Awake ()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
		animator = GetComponent<Animator> ();
		icon = transform.Find ("Icon").GetComponent<Image> ();
		title = transform.Find ("Title").GetComponent<Text> ();
		scrollRect = transform.Find ("Scroll View").GetComponent<ScrollRect> ();
		body = transform.Find ("Scroll View").GetComponentInChildren<Text> ();
		button = transform.Find ("Button").GetComponent<Button> ();
		outside = transform.Find ("Outside").GetComponent<Button> ();
		buttonText = button.transform.Find ("Text").GetComponent<Text> ();
		button.onClick.AddListener (ButtonClick);
		outside.onClick.AddListener (OutsideClick);

	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.T)) {
			
		}
	}

	// A hacky solution to the scroll view issue where it auto scrolls to bottom on PopUp()
	// This function is called by an event about 1/6 of the way into the animation, and it stops the scroll bar from moving down
	public void ResetScrollPosition () {
		scrollRect.verticalNormalizedPosition = 1;
	}

	public void SetIcon (Image image)
	{
		icon.sprite = image.sprite;
		icon.color = image.color;
	}

	public void SetIcon (string filePath, Color color)
	{
		icon.sprite = Resources.Load<Sprite>(filePath);
		icon.color = color;
	}

	public void SetTitle (string title)
	{
		this.title.text = title;
	}

	public void SetBody (string body)
	{
//		scrollRect.enabled = false;
		this.body.text = body;
//		scrollRect.enabled = true;
	}

	public void SetButtonText (string text)
	{
		this.buttonText.text = text;
	}

	public void ButtonClick ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName("Visible")) {
			gameController.PlayClickSound2 ();
			gameController.SendMessage (callBackFunctionName, callBackParameter, SendMessageOptions.DontRequireReceiver);
			StartCoroutine(FadeOut ());
		}
	}

	public void OutsideClick ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName("Visible")) {
			StartCoroutine(FadeOut ());
		}
	}

	public void PopUp() {
//		Debug.Log ("show panel");
		animator.SetTrigger ("Show");
	}

	public IEnumerator FadeOut() {
		animator.SetTrigger ("Hide");
		gameObject.tag = "Untagged";
		yield return new WaitForSeconds (0.4f);

		uiController.MessagePanelDestroyed ();
		Destroy (gameObject);
	}

	// getters and setters
	public string CallBackFunctionName {
		get {
			return this.callBackFunctionName;
		}
		set {
			callBackFunctionName = value;
		}
	}

	public int [] CallBackParameter {
		get {
			return this.callBackParameter;
		}
		set {
			callBackParameter = value;
		}
	}
}

