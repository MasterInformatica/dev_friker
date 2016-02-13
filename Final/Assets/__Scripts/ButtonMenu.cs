using UnityEngine;
using System.Collections;

public class ButtonMenu : MonoBehaviour {
	public GameObject playerPrefab; // Prefab for player
	public int level = 1;
	public int realLevel = 0;
	public int timeLimit = 0;

	void Awake() {
		//BORRAR CUANDO ACABES
		if (realLevel == 0)
			Utils.tr ("level allow: ",ApplicationModel.MaxLevel);
		/*if (ApplicationModel.MaxLevel < realLevel) {
			Destroy (this.gameObject);
		} else {*/
			Transform launchPointTrans = transform.Find ("LaunchPoint");
		//}
	}
	void OnMouseDown() {
		ApplicationModel.playerPrefab = playerPrefab;
		ApplicationModel.XMLlevel = level;
		ApplicationModel.ActualLevel = realLevel;
		ApplicationModel.MaxTime = timeLimit;
		Application.LoadLevel ("blocker");
	}
}
