using UnityEngine;
using System.Collections;

public class ButtonAction : MonoBehaviour {
	// accion que realizara el boton al pulsarse
	public string action = "highscore";

	void OnMouseDown() {
		switch (action) {
		case "highscore": // cargar highscores
			Application.LoadLevel ("highscore");
			break;
		case "back": // cargar menu
			Application.LoadLevel ("menu");
			break;
		case "retry": // recargar nivel
			Application.LoadLevel ("blocker");
			break;
		}

	}
}
