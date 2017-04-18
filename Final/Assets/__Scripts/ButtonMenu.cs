using UnityEngine;
using System.Collections;

public class ButtonMenu : MonoBehaviour {
	public GameObject playerPrefab; // Prefab de la figura que jugara el nivel
	public int level = 1;           // nivel que se solicitara al XML
	public int realLevel = 0;       // nivel dentro del juego
	public int timeLimit = 0;       // tiempo para superar el nivel

	void Awake() {
		if (ApplicationModel.MaxLevel < realLevel) { 
			// si aun no han desbloqueado el nivel lo elimino
			Destroy (this.gameObject);
		}
	}

	void OnMouseDown() {
		// cargar los valores del nivel en el "Model" del jugo
		ApplicationModel.playerPrefab = playerPrefab;
		ApplicationModel.XMLlevel = level;
		ApplicationModel.ActualLevel = realLevel;
		ApplicationModel.MaxTime = timeLimit;
		// cargar la escena de juego
		Application.LoadLevel ("blocker");
	}
}
