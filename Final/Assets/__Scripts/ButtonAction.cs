using UnityEngine;
using System.Collections;

public class ButtonAction : MonoBehaviour {
	public string action = "highscore";

	void OnMouseDown() {
		switch (action) {
		case "highscore":
			Application.LoadLevel ("highscore");
			break;
		case "back":
			Application.LoadLevel ("menu");
			break;
		case "retry":
			Application.LoadLevel ("blocker");
			break;
		}

	}
}
