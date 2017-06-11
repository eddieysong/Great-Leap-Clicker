using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

	// handles to other controllers
	private GameController gameController;
	private UIController uiController;
	private Animator animator;

	// handles to UI elements displayed
	private Image icon;
	private Text title;
	private Button outside;


	// Use this for initialization
	void Awake ()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		uiController = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController>();
		animator = GetComponent<Animator> ();
		icon = transform.Find ("Icon").GetComponent<Image> ();
		title = transform.Find ("Title").GetComponent<Text> ();
		outside = transform.Find ("Outside").GetComponent<Button> ();
		outside.onClick.AddListener (OutsideClick);

	}

	public void ResetScrollPosition () {
		
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

	public void ButtonClick ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName("Visible")) {
			FadeOut ();
		}
	}

	public void OutsideClick ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName("Visible")) {
			FadeOut ();
		}
	}

	public void PopUp() {
		if (animator.GetCurrentAnimatorStateInfo (0).IsName("Hidden") && !animator.IsInTransition(0)) {
			animator.SetTrigger ("Show");
		}
	}

	public void FadeOut() {
		animator.SetTrigger ("Hide");
	}


	public void UnitConversions () {
		MessagePanelController msgPanelController;

		Debug.Log (GameObject.FindGameObjectWithTag ("MsgPanel"));
		if (!GameObject.FindGameObjectWithTag ("MsgPanel")) {
			if (msgPanelController = uiController.NewMessagePanel ()) {
				Debug.Log ("msgPanel instantiated");
				msgPanelController.SetTitle ("Unit Conversions");
				msgPanelController.SetBody ("1000 g = 1 kg (kilogram)\n" +
					"1000 kg = 1 t (ton)\n" +
					"1000 t = 1 kt (kilo-ton)\n" +
					"1000 kt = 1 Mt (Mega-ton)\n" +
					"1000 Mt = 1 Gt (Giga-ton)\n" +
					"1000 Gt = 1 Tt (Tera-ton)\n" +
					"1000 Tt = 1 Pt (Peta-ton)\n" +
					"1000 Pt = 1 Et (Exa-ton)\n" +
					"1000 Et = 1 Zt (Zetta-ton)\n" +
					"1000 Zt = 1 Yt (Yotta-ton)\n" +
					"1000 Yt = 1 KY (Kilo-Yotta-ton)\n" +
					"1000 KY = 1 MY (Mega-Yotta-ton)\n" +
					"1000 MY = 1 GY (Giga-Yotta-ton)\n" +
					"1000 GY = 1 TY (Tera-Yotta-ton)\n" +
					"1000 TY = 1 PY (Peta-Yotta-ton)\n" +
					"1000 PY = 1 EY (Exa-Yotta-ton)\n" +
					"1000 EY = 1 ZY (Zetta-Yotta-ton)\n" +
					"1000 ZY = 1 YY (Yotta-Yotta-ton)\n" +
					"> 1000 YY : Displayed in Scientific Notation");
				msgPanelController.SetIcon ("Sprites/UI/info", Color.yellow);
				msgPanelController.SetButtonText ("Reset Tutorial");
				msgPanelController.CallBackFunctionName = "ResetTutorial";
			}
		}
		FadeOut ();
	}

	public void ResetTutorial () {
		MessagePanelController msgPanelController;

		Debug.Log (GameObject.FindGameObjectWithTag ("MsgPanel"));
		if (!GameObject.FindGameObjectWithTag ("MsgPanel")) {
			if (msgPanelController = uiController.NewMessagePanel ()) {
				Debug.Log ("msgPanel instantiated");
				msgPanelController.SetTitle ("Reset Tutorial");
				msgPanelController.SetBody ("If you would like to reset the tutorial so tutorial panels would pop up again as you progress, click the button below.");
				msgPanelController.SetIcon ("Sprites/UI/info", Color.yellow);
				msgPanelController.SetButtonText ("Reset Tutorial");
				msgPanelController.CallBackFunctionName = "ResetTutorial";
			}
		}
		FadeOut ();
	}

	public void WipeData () {
		MessagePanelController msgPanelController;
		if (!GameObject.FindGameObjectWithTag ("MsgPanel")) {
			if (msgPanelController = uiController.NewMessagePanel ()) {
				msgPanelController.SetTitle ("<color=#ff0000ff>Wipe Data</color>");
				msgPanelController.SetBody ("<color=#ff0000ff>This is not a Reset!</color>\n\n" +
					"This option will completely erase all your progress, and you will have to start from the very beginning.");
				msgPanelController.SetIcon ("Sprites/UI/info", Color.red);
				msgPanelController.SetButtonText ("<color=#ff0000ff>Wipe Data!</color>");
				msgPanelController.CallBackFunctionName = "DeleteSaveFile";
			}
		}
		FadeOut ();
	}
}
