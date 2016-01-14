using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public GameObject playerPrefab; // Prefab for player

	public bool __________________ = false;
	Player pl;
	public int[,] map;
	static public Game S;
	int x_g,y_g,x_p,y_p,z_p;
	// Use this for initialization
	void Awake () {
		S = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void drawPlayer(){
			GameObject go = Instantiate (playerPrefab) as GameObject;
			pl = go.GetComponent<Player> ();
			float z = go.transform.localScale.y/2;// * 0.5750011f;
			// Set the position of the tile
			pl.pos = new Vector3 (x_p,z_p-0.5f+z,y_p);			
	}

	public void setMap(int[,] m,int x_goal,int y_goal, int x_start, int y_start, int z_start){
		x_g = x_goal;
		y_g = y_goal;
		x_p = x_start;
		y_p = y_start;
		z_p = z_start;
		map = m;

		drawPlayer ();
	}
	public bool checkMapHole(){
		
		return false;
	}

	public int difHeight(string dir){
		Utils.tr (x_p, y_p, dir);
		int actual = map [x_p,y_p];
		int futura = actual;
		switch (dir) {
		case "W":
			futura = map [x_p+1,y_p];
			break;
		case "S":
			futura = map [x_p-1,y_p];
			break;
		case "A":
			futura = map [x_p,y_p-1];
			break;
		case "D":
			futura = map [x_p,y_p+1];
			break;
		default:
			futura = actual;
			break;
		}
		return futura - actual;

	}
}
