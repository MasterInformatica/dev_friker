using UnityEngine;
using System.Collections;

public class ButtonMenu : MonoBehaviour {
	public GameObject playerPrefab; // Prefab for player
	public int level = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Awake() {
		Transform launchPointTrans = transform.Find("LaunchPoint");
		/*launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive( false );
		launchPos = launchPointTrans.position;
*/	}
	void OnMouseDown() {
/*		// The player has pressed the mouse button while over Slingshot
		aimingMode = true;
		// Instantiate a Projectile
		projectile = Instantiate( prefabProjectile ) as GameObject;
		// Start it at the launchPoint
		projectile.transform.position = launchPos;
		// Set it to isKinematic for now
		projectile.GetComponent<Rigidbody>().isKinematic = true;
*/
		ApplicationModel.playerPrefab = playerPrefab;
		ApplicationModel.level = level;
		Application.LoadLevel ("blocker");
	}
}
