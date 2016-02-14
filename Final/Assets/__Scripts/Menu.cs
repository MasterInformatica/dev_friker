using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
	public Canvas C;
	public Text gtMovs; // The GT_Level GUIText
	public Text gtTime; // The GT_Score GUIText


	public void Start(){
		/*
		 * Calcular el tiempo total y movimientos usados
		 * Mostrar record general al usuario
		 */
		int sumT=0,sumM = 0;
		for (int i = 0; i < ApplicationModel.ScoreMovs.Length; i++)
		{
			sumT += ApplicationModel.ScoreTime[i];
			sumM += ApplicationModel.ScoreMovs[i];
		}
		gtMovs.text = "Total Mov: "+sumM;
		gtTime.text = "Tiempo: "+ sumT + " s";
	}

}
