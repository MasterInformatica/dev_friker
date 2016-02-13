using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScore : MonoBehaviour {
	public Text Table;
	// Use this for initialization
	void Start () {
		string txt = "";
		txt = "Level\tMovs\tTime\n";
		for (int i = 0; i < 8; i++) {
			txt = txt+"-"+(i+1)+"-\t\t"+ApplicationModel.ScoreMovs[i]+"\t\t"+ApplicationModel.ScoreTime[i]+"s\n";
		}
		Table.text = txt;
	}

}
