using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScore : MonoBehaviour { 

	public Text Table; // ScoreBoard, ojala hubiese sido mas bonito
	public float Ratio = 1.5f; // cuanto lo vamos a retocar el texto
	public bool _____________ = false;
	public float _oldWidth;
	public float _oldHeight;
	public float _oldRatio; // ayuda al debugueo
	void Start () {
		// guardar el tamaño de la pantalla (en principio no cambia, pero debugueando si)
		_oldWidth = Screen.width;
		_oldHeight = Screen.height;
		_oldRatio = Ratio;
		// calculamos el tamaño de la letra en funcion al espacio disponible
		Table.fontSize = Mathf.RoundToInt(Mathf.Min (Screen.width, Screen.height / 9.0f) / Ratio);
		string txt = "";
		// Construimos tabla de records a mano... muy feo
		txt = "Level\tMovs\t\tTime\n";
		for (int i = 0; i < 8; i++) {
			txt = txt+"-"+(i+1)+"-\t\t\t"+ApplicationModel.ScoreMovs[i]+"\t\t\t"+ApplicationModel.ScoreTime[i]+"s\n";
		}
		Table.text = txt;

	}

	void Update(){
		// por si redimensionas, sirve para debuguear
		if (_oldWidth != Screen.width || _oldHeight != Screen.height || _oldRatio != Ratio && Ratio != 0) {
			_oldWidth = Screen.width;
			_oldHeight = Screen.height;
			_oldRatio = Ratio;
			Table.fontSize = Mathf.RoundToInt (Mathf.Min (Screen.width, Screen.height / 9.0f) / Ratio);
		}
	}

}
