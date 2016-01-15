using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public GameObject playerPrefab; // Prefab for player

	public bool __________________ = false;
	Player pl;
	public int[,] map;
	static public Game S;
	public int x_g,y_g,x_p,y_p,z_p;
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
		int x, y;
		int[] pos = pl.getPos ();
		x = pos[0];
		y = pos[1];
		Utils.tr (x, y, dir);
		int actual = map [x,y];
		int futura = actual;
		switch (dir) {
		case "W":
			futura = map [x+1,y];
			break;
		case "S":
			futura = map [x-1,y];
			break;
		case "A":
			futura = map [x,y-1];
			break;
		case "D":
			futura = map [x,y+1];
			break;
		default:
			futura = actual;
			break;
		}
		Utils.tr (futura - actual);
		return futura - actual;

	}
}
