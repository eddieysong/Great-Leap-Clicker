using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour {

	public void SwitchToMain () {
		SceneManager.LoadScene ("Main");
	}
}
