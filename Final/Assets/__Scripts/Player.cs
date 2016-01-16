using UnityEngine;
using System.Collections;
public enum StMov {Parado,Mov,Caer,Morir,Ganar};

public class Player : PT_MonoBehaviour {
	public static float bottomY = -20f;
	public float speed = 300.0f;
	public bool __________________ = false;
	private int _height = 0;
	private Vector3 _pos;
	public int x, y;
	// Estos dos bools definen el estado de la figura tumbada o de pie.
	//st tiene el state del movimiento.
	public bool _______State______ = false;
	public bool inBase = true;
	public bool change = false; //true -> AD; false -> WS
	public StMov st = StMov.Parado;
	public bool _________________ = false;
	//----------------------------------------------------
	public bool _______Walk_____ = false;
	public string dir;
	public Vector3 pivotPoint = Vector3.zero;
	public Vector3 pivotAngle = Vector3.zero;
	public float pivotAmount = 0.0f;
	public float pivotLimit = 90.0f;
	public Transform target;
	public bool ___________________ = false;
	public bool muerte = false;


	
	new public Vector3 pos {
		get { return( _pos ); }
		set {
			_pos = value;
			x = (int)_pos.x;
			y = (int)_pos.z;
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
		target.GetComponent<Renderer>().enabled = false;// comment out not to see the pivot point.
	}
	
	public void Update () {
		switch (st) {
			case StMov.Parado:
			if (Game.S.checkMapHole (x,y)) {
				st = StMov.Caer;
				break;
			} else if (Game.S.checkGoal(x,y)){
				st = StMov.Ganar;
				break;
			}	else if (Input.GetKeyDown ("up") || Input.GetKeyDown ("w")) {
				dir = "W";
			} else if (Input.GetKeyDown ("down") || Input.GetKeyDown ("s")) {
				dir = "S";
			} else if (Input.GetKeyDown ("left") || Input.GetKeyDown ("a")) {
				dir = "A";
			} else if (Input.GetKeyDown ("right") || Input.GetKeyDown ("d")) {
				dir = "D";
			} else {
				break;
			}
			ApplicationModel.totalMovs++;
			float sentido = 1.0f;
			Vector3 altura = new Vector3(0.0f,0.5f,0.0f);
			Vector3 offset = Vector3.zero;
			switch(dir)
			{
			case "S":
				sentido = -1.0f;
				goto case "W";
			case "W":
				offset = Vector3.forward*sentido;
				pivotAngle = Vector3.right*sentido;
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
				pivotAngle = Vector3.forward*(-1.0f*sentido);
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

			int dif = Game.S.difHeight(dir);
			if(dif < 0){ // caida
				pivotLimit = 180.0f;
			}else if (dif == 0){ // mismo nivel
				pivotLimit = 90.0f;
			}else if (dif == 1){ //subida
				pivotLimit = 180.0f;
				altura = -altura;
			}else{
				break;
			}
			if (dif < -1 ) 
				muerte = true;

			pivotPoint = transform.position-altura + offset; //*/
			target.position = pivotPoint;


			st = StMov.Mov;
			break;
		case StMov.Mov:
			float amount = Time.deltaTime * speed * 20.0f;
			pivotAmount = pivotAmount + amount;
			amount = amount - (pivotAmount > pivotLimit ? pivotAmount - pivotLimit : 0.0f);
			transform.RotateAround (pivotPoint, pivotAngle, amount);
				
			if (pivotAmount >= pivotLimit) {
				target.position = transform.position;//hides the target after it completes rotating.
				if(muerte)
					st = StMov.Morir;
				else
					st = StMov.Parado;
				pivotAmount = 0;
			    actualizaPos (dir);
			}
			break;
		case StMov.Caer:
			transform.GetComponent<Rigidbody>().useGravity = true;
			transform.GetComponent<Rigidbody>().isKinematic = false;
			if (transform.position.y < bottomY)
				st = StMov.Morir;
			break;
		case StMov.Morir:
			morir ();
			break;
		default:
			break;
		}
	}
	public void morir(){
		Destroy( this.gameObject );
		Game.S.playerDestroyed();
	}
	public void actualizaPos(string dir){
		switch (dir) {
		case "D":
			x++;
			break;
		case "A":
			x--;
			break;
		case "S":
			y--;
			break;
		case "W":
			y++;
			break;
		default:
			break;


		}
	}

	public int[] getPos(){
		return new int[]{x,y};
	}
	
}