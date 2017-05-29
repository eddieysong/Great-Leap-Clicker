using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanelController : MonoBehaviour {
	
	// handles to other controllers
	private GameController gameController;
	private Animator animator;

	// handles to UI elements displayed
	private Image icon;
	private Text title;
	private Text body;
	private Button button;
	private Text buttonText;
	private Button outside;

	// upgrade properties
	public int id;
	public int level;
	public string upgradeName;
	public string description;
	public double baseCost = 5f;
	public double increasePerLevel = 1f;

	// upgrade cost follows the formula: Y = baseCost * (1 + costPercentIncreasePerLevel) ^ currentLevel
	// this is A
	//	public double flatIncreasePerLevel = 1f;
	// this is B
	public double costPercentIncreasePerLevel = 0.05f;
	// this is C
	//	public double expFactorPerLevel = 1.02f;

	public double currentCost;

	// Use this for initialization
	void Awake ()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		animator = GetComponent<Animator> ();
		icon = transform.Find ("Icon").GetComponent<Image> ();
		title = transform.Find ("Title").GetComponent<Text> ();
		body = transform.Find ("Body").GetComponent<Text> ();
		button = transform.Find ("Button").GetComponent<Button> ();
		outside = transform.Find ("Outside").GetComponent<Button> ();
		buttonText = button.transform.Find ("Text").GetComponent<Text> ();
		button.onClick.AddListener (ButtonClick);
		outside.onClick.AddListener (OutsideClick);


		RefreshPanelText ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.T)) {
			PopUp ();
		}
	}

	public void RefreshPanelText ()
	{

	}

	public void SetImage ()
	{

	}

	public void SetTitle (string title)
	{
		this.title.text = title;
	}

	public void SetBody (string body)
	{
		this.body.text = body;
	}

	public void SetButtonText (string text)
	{
		this.buttonText.text = text;
	}

	public void ButtonClick ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName("Visible")) {
			Debug.Log ("ok");
			FadeOut ();
		}
	}

	public void OutsideClick ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName("Visible")) {
			Debug.Log ("outside");
			FadeOut ();
		}
	}

	private void PopUp() {
		animator.SetTrigger ("Show");
	}

	private void FadeOut() {
		animator.SetTrigger ("Hide");
	}

}
