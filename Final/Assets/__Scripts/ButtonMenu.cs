using UnityEngine;
using System.Collections;

public class ButtonMenu : MonoBehaviour {
	public GameObject playerPrefab; // Prefab for player
	public int level = 1;
	public int realLevel = 0;

	void Awake() {
		//BORRAR CUANDO ACABES
		if (realLevel == 0)
			Utils.tr ("level allow: ",ApplicationModel.ActualLevel,"total movs: ",ApplicationModel.totalMovs);
		if (ApplicationModel.ActualLevel < realLevel) {
			Destroy (this.gameObject);
		} else {
			Transform launchPointTrans = transform.Find ("LaunchPoint");
		}
	}
	void OnMouseDown() {
		ApplicationModel.playerPrefab = playerPrefab;
		ApplicationModel.level = level;
		Application.LoadLevel ("blocker");
	}
}
