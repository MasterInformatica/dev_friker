using UnityEngine;
using System.Collections;

[System.Serializable]
public static class ApplicationModel
{
	static public int XMLlevel = 0;    // this is reachable from everywhere
	static public GameObject playerPrefab;
	static public int totalMovs = 0;
	static public int MaxLevel = 0;
	static public int ActualLevel = 0;
	static public int totalPoints = 0;
	
}
public class Game : MonoBehaviour {
	static public Game S;
	public bool __________________ = false;
	public Player pl;
	public int levelPoints = 0;
	public int[,] map = new int[100,100];
	public int x_g,y_g,x_p,y_p,z_p;
	// Use this for initialization
	void Awake () {
		S = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void playerDestroyed(){
		
		Application.LoadLevel( "menu" );
	}  

	public void drawPlayer(){
			GameObject go = Instantiate (ApplicationModel.playerPrefab) as GameObject;
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
	public bool checkMapHole(int x, int y){
		return map [x, y] == 0;
	}
	public bool checkGoal(int x, int y){
		if (x_g == x && y_g == y)
			return true;
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
			futura = map [x,y+1];
			break;
		case "S":
			futura = map [x,y-1];
			break;
		case "A":
			futura = map [x-1,y];
			break;
		case "D":
			futura = map [x+1,y];
			break;
		default:
			futura = actual;
			break;
		}
		if (futura == 0)
			return -1;
		else
			return futura - actual;

	}
}
