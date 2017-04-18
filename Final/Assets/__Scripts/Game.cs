using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class ApplicationModel{
	// clase que contiene las variables del juego

	// variables relativas al nivel a iniciar
	static public int XMLlevel = 1; 
	static public GameObject playerPrefab;
	static public int MaxTime = 30;

	// variables para gestionar niveles visibles
	static public int MaxLevel = 0;
	static public int ActualLevel = 0;

	// variables de Score
	static public int[] ScoreMovs = {0,0,0,0,0,0,0,0};
	static public int[] ScoreTime = {0,0,0,0,0,0,0,0};	
}

public class Game : MonoBehaviour {
	static public Game S; //singleton
	public Text timeText;
	public Text movsText;
	public bool __________________ = false;
	public Player pl;
	public int levelPoints = 0;
	public int[,] map = new int[100,100];
	public int x_g,y_g,x_p,y_p,z_p;
	// Use this for initialization
	void Awake () {
		S = this;
	}

	public void playerDestroyed(){ 
		// me han dicho que se ha destruido el jugador
		// simplemente carguemos el menu
		Application.LoadLevel("menu");
	}  

	public void drawPlayer(){ 
		// pinto el jugador, tal y como me digan que debo hacerlo
		GameObject go = Instantiate (ApplicationModel.playerPrefab) as GameObject;
		pl = go.GetComponent<Player> ();
		float z = go.transform.localScale.y/2;
		// Set the position of the tile
		pl.pos = new Vector3 (x_p,z_p-0.5f+z,y_p);			
	}

	public void setMap(int[,] m,int x_goal,int y_goal, int x_start, int y_start, int z_start){
		/*
		 * Funcion para guardarse el mapa y las cositas objetivo
		 */
		x_g = x_goal;
		y_g = y_goal;
		x_p = x_start;
		y_p = y_start;
		z_p = z_start;
		map = m;

		drawPlayer ();
	}
	public bool checkMapHole(int x, int y){
		// si hay un 0 es un agujero
		return map [x, y] == 0;
	}
	public bool checkGoal(int x, int y){
		// es la casilla objetivo?
		return (x_g == x && y_g == y);
	}

	public int difHeight(string dir){
		// calcula la diferencia de altura
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
		if (futura == 0) // si es un agujero me da igual la diferencia simplemente caera
			return -1;
		else
			return futura - actual;
	}
}
