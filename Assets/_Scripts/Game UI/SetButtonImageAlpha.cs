using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetButtonImageAlpha : MonoBehaviour {

	Button btn;
	Image img;
	// Use this for initialization
	void Start () {
		btn = this.transform.parent.gameObject.GetComponent<Button>();
		img = GetComponent<Image> ();
	}
	
	void Update () {
		img.color = new Color (img.color.r, img.color.g, img.color.b, btn.IsInteractable() ? btn.colors.normalColor.a : btn.colors.disabledColor.a);
	}
}
