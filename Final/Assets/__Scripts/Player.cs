using UnityEngine;
using System.Collections;
public enum StMov {Parado,Subiendo,Bajando,Mov};

public class Player : PT_MonoBehaviour {

	public bool __________________ = false;

	private int _height = 0;
	private Vector3 _pos;


	// Estos dos bools definen el estado de la figura tumbada o de pie.
	//st tiene el state del movimiento.
	public bool _______State______ = false;
	public bool inBase = true;
	public bool change = false; //true -> AD; false -> WS
	public StMov st = StMov.Parado;
	public bool _________________ = false;
	//----------------------------------------------------
	public Vector3 pivotPoint = Vector3.zero;
	public Vector3 pivotAngle = Vector3.zero;
	public float pivotAmount = 0.0f;
	public float pivotLimit = 90.0f;
	public int longways=0;
	
	public float speed = 300.0f;
	private Transform target;
	
	new public Vector3 pos {
		get { return( _pos ); }
		set {
			_pos = value;
			AdjustHeight();
		}
	}

	// Methods
	public void AdjustHeight() {
		// Moves the block up or down based on _height
		Vector3 vertOffset = Vector3.back*(_height);
		// The -0.5f shifts the Tile down 0.5 units so that it's top surface is
		// at z=0 when pos.z=0 and height=0
		transform.position = _pos+vertOffset;
	}

	public void Start(){
		//create a dummy to show my target placment. Helped figure out where everything was going wrong
		target=GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
		target.localScale= new Vector3(0.1f,0.1f,0.1f);
		target.gameObject.layer=2;
		target.name="Target";
		DestroyImmediate(target.GetComponent<Collider>());
		//target.GetComponent<Renderer>().enabled = false;// comment out not to see the pivot point.
	}
	
	public void Update () {
		switch (st) 
		{
			case StMov.Parado:
				Vector3 offset = Vector3.zero;
				int check = 1;
				string dir = "";
				if (Game.S.checkMapHole ()) {	
					break;
				} else if (Input.GetKeyDown ("up") || Input.GetKeyDown ("w")) {
					dir = "W";
					//offset = Vector3.forward;
					pivotAngle = Vector3.right;
				} else if (Input.GetKeyDown ("down") || Input.GetKeyDown ("s")) {
					dir = "S";
					//offset = -Vector3.forward;
					pivotAngle = -Vector3.right;
				} else if (Input.GetKeyDown ("left") || Input.GetKeyDown ("a")) {
					dir = "A";
					//offset = -Vector3.right;
					pivotAngle = Vector3.forward;
					//check = 2;
				} else if (Input.GetKeyDown ("right") || Input.GetKeyDown ("d")) {
					dir = "D";
					//offset = Vector3.right;
					pivotAngle = -Vector3.forward;
					//check = 2;
				} else {
					break;
				}
			float sentido = 1.0f;
			Vector3 altura = new Vector3(0.0f,0.5f,0.0f);
			switch(dir)
			{
			case "S":
				sentido = -1.0f;
				goto case "W";
			case "W":
				offset = Vector3.forward*sentido;
				if(inBase){ //la base siempre es cuadrada de tamaño 1
					inBase = false;
					change = false;
					altura = new Vector3(0.0f,transform.localScale.y/2,0.0f);
				}else{ // no estoy en la base... :(
					if(!change){ 
						offset *= transform.localScale.y;
						inBase = true;
					}
				}
				offset *= 0.5f;
				break;
			case "A":
				sentido = -1.0f;
				goto case "D";
			case "D":
				offset = Vector3.right*sentido;
				if(inBase){ //la base siempre es cuadrada de tamaño 1
					inBase = false;
					change = true;
					altura = new Vector3(0.0f,transform.localScale.y/2,0.0f);
				}else{ // no estoy en la base... :(
					if(change){ 
						offset *= transform.localScale.y;
						inBase = true;
					}
				}
				offset *= 0.5f;
				break;
			default:
				break;
			}

				/*int dif = Game.S.difHeight(dir);
					if(dif < 0){ // caida
						pivotLimit = 180.0f;
					}else if (dif == 0){ // mismo nivel
						pivotLimit = 90.0f;
					}else{ //subida
						pivotLimit = 180.0f;
					}*/
				Utils.tr ("offset and pos",offset, transform.position,altura);
/*			Vector3 mitad = getMitad(dir);

			Utils.tr (mitad);
			pivotPoint = transform.position-mitad + offset / 2;/*/
			pivotPoint = transform.position-altura + offset; //*/
			Utils.tr ("pivot",pivotPoint);
			target.position = pivotPoint;

//			actualizaEstado (dir);
			st = StMov.Mov;
				break;
		case StMov.Mov:

				float amount = Time.deltaTime * speed * 20.0f;
				pivotAmount = pivotAmount + amount;
				//Utils.tr(pivotAmount,pivotPoint);
				amount = amount - (pivotAmount > 90.0f ? pivotAmount - 90.0f : 0.0f);
				transform.RotateAround (pivotPoint, pivotAngle, amount);
				
				if (pivotAmount >= 90f) {
					target.position = transform.position;//hides the target after it completes rotating.
					st = StMov.Parado;
					pivotAmount = 0;
				}
				break;
			default:
				break;
		}
	}

	public void actualizaEstado(string dir){
		if (inBase) {
			inBase = false;
			switch(dir){
			case "W":
			case "S":
				change = false;
				break;
			case "A":
			case "D":
				change = true;
				break;
			}
		} else {
			switch(dir){
			case "W":
			case "S":
				if(!change)
					inBase = true;
				break;
			case "A":
			case "D":
				if(change)
					inBase = true;
				break;
			}
		}
		//inBase = true si estoy en base
		//change
		// st llamando a getaltura
	}

	public Vector3 getMitad(string dir){
		Utils.tr ("LS", transform.localScale);
		Vector3 m = Vector3.zero;
		if (inBase) {
			m = new Vector3(0.0f,transform.localScale.y/2.0f,0.0f);
		} else {
			switch(dir){
			case "W":
			case "S":
				m = new Vector3(0.0f,transform.localScale.x,0.0f);
				break;
			case "A":
			case "D":
				m = new Vector3(0.0f,0.0f,0.0f);
				break;
			}
		}
		return m;
	}




}