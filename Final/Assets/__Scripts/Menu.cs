using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
	public Canvas C;
	public Text gtMovs; // The GT_Level GUIText
	public Text gtTime; // The GT_Score GUIText


	public void ShowGT(){
		int sumT=0,sumM = 0;
		for (int i = 0; i < ApplicationModel.ScoreMovs.Length; i++)
		{
			sumT += ApplicationModel.ScoreTime[i];
			sumM += ApplicationModel.ScoreMovs[i];
		}
		gtMovs.text = "Movimientos totales: "+sumM;
		gtTime.text = "Tiempo utilizado: "+ sumT + " s";
	}

	void Update () {
		ShowGT();
	}
}
