using System.Collections;
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

	private string callBackParameter;

	// Use this for initialization
	void Awake ()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
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
			gameController.SendMessage (callBackParameter, SendMessageOptions.DontRequireReceiver);
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
		Debug.Log ("show panel");
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
	public string CallBackParameter {
		get {
			return this.callBackParameter;
		}
		set {
			callBackParameter = value;
		}
	}
}

