using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public enum StMov {Parado,Mov,Ajustar,Caer,Morir,Ganar};

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
	public StMov st = StMov.Parado;
	public bool _________________ = false;
	//----------------------------------------------------
	public bool _______Walk_____ = false;
	public string dir;
	public Vector3 pivotPoint = Vector3.zero;
	public Vector3 pivotAngle = Vector3.zero;
	public Vector3 reference = Vector3.zero;
	public float pivotAmount = 0.0f;
	public float pivotLimit = 90.0f;
	public Transform target;
	public bool ___________________ = false;
	public int movs = 0;
	public float time = 0;
	public float endTime = 0;
	public float delta = 100f;
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


	public bool isVertical(){
		/* Indica si el player esta apoyado en alguna de las bases
		 * Simplemente es mirar si Rotation.x es cero
		 */
		Utils.tr (transform.eulerAngles.z,"angle");
		return Mathf.Approximately(transform.eulerAngles.x, 0.0f) && ( Mathf.Approximately(transform.eulerAngles.z, 0.0f) || Mathf.Approximately(transform.eulerAngles.z,180.0f) );
	}

	public bool inBase(){
		/* Indica si el player esta apoyado en la base objetivo	
		 * Sera si Rotation.x y Rotation.z son cero
		 */
		return Mathf.Approximately(transform.eulerAngles.x, 0.0f) && Mathf.Approximately(transform.eulerAngles.z, 0.0f);
	}

	public void RoundRotation(){
		/* Esta funcion asegura que al mover el cubo siempre quede
		 * bien posicionadas sus componentes de rotacion.
		 * Redondeandolas a los "90" grados mas cercanos.
		 */
		Vector3 vec = transform.eulerAngles;
		vec.x = Mathf.Round(vec.x / 90) * 90;
		vec.y = Mathf.Round(vec.y / 90) * 90;
		vec.z = Mathf.Round(vec.z / 90) * 90;
		transform.eulerAngles = vec;
	}
	public void RoundPosition(){
		Vector3 vec = transform.position;
		vec.x = Mathf.Round (vec.x / 0.5f) * 0.5f;
		vec.y = Mathf.Round (vec.y / 0.5f) * 0.5f;
		vec.z = Mathf.Round (vec.z / 0.5f) * 0.5f;
		transform.position = vec;
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
		endTime = Time.time + ApplicationModel.MaxTime;
	}
	public string getDir(){
		if (Input.GetKeyDown ("up") || Input.GetKeyDown ("w")) {
			return "W";
		} else if (Input.GetKeyDown ("down") || Input.GetKeyDown ("s")) {
			return "S";
		} else if (Input.GetKeyDown ("left") || Input.GetKeyDown ("a")) {
			return "A";
		} else if (Input.GetKeyDown ("right") || Input.GetKeyDown ("d")) {
			return "D";
		}
		return "none";
	}
	public void updateText(){
		Game.S.movsText.text = "Movimientos: " + movs;
		Game.S.timeText.text = "Tiempo restante: " + time + " s";
	}
	public void checkTime(){
		time = endTime - Time.time;
		if (time < 0.0f) {
			time = 0.0f;
			st = StMov.Morir;
		}
	}
	public void Update () {
		checkTime ();
		updateText ();
		switch (st) {
			case StMov.Parado:
			if (Game.S.checkMapHole (x,y)) {
				st = StMov.Caer;
				break;
			} else if (Game.S.checkGoal(x,y) && inBase ()){
				st = StMov.Ganar;
				break;
			}	
			dir = getDir ();
			if(dir == "none") break;
			movs ++;
			float sentido = 1.0f;
			Vector3 altura = new Vector3(0.0f,0.5f,0.0f);
			Vector3 offset = Vector3.zero;
			reference = transform.position * 2;
			reference.x = reference.x %2;
			reference.y = reference.y %2;
			reference.z = reference.z %2;
			float tall = transform.localScale.y/2.0f;

			Utils.tr (reference,"eee",altura);
			switch(dir)
			{
			case "S":
				sentido = -1.0f;
				goto case "W";
			case "W":
				offset     = Vector3.forward * sentido;
				pivotAngle = Vector3.right   * sentido;
				if(isVertical()){ //la base es cuadrada de tamaño 1
					Utils.tr ("Vertical");
					altura = new Vector3(0.0f,tall,0.0f);
				}else{ // no estoy en la base... :(
					if(reference.z>0){ 
						offset *= transform.localScale.y;
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
				if(isVertical()){ //la base siempre es cuadrada de tamaño 1
					Utils.tr ("Vertical");
					altura = new Vector3(0.0f,tall,0.0f);
				}else{ // no estoy en la base... :(
					if(reference.x>0){ 
						offset *= transform.localScale.y;
					}
				}
				offset *= 0.5f;
				break;
			default:
				break;
			}

			int dif = Game.S.difHeight(dir);
			if(dif < 0){ // caida
				muerte = dif < -1;
				pivotLimit = 180.0f;
			}else if (dif == 0){ // mismo nivel
				pivotLimit = 90.0f;
			}else if (dif == 1){ //subida
				pivotLimit = 180.0f;
				altura = -altura;
			}else{
				break;
			}
			Utils.tr ("pos",transform.position);
			Utils.tr("alt",altura);
			Utils.tr ("off",offset);
			pivotPoint = transform.position-altura + offset; 
			Utils.tr ("pivote",pivotPoint);
			target.position = pivotPoint;
			st = StMov.Mov;
			break;
		case StMov.Mov:
			float amount = Time.deltaTime * speed * 20.0f;
			pivotAmount = pivotAmount + amount;
			amount = amount - (pivotAmount > pivotLimit ? pivotAmount - pivotLimit : 0.0f);
			transform.RotateAround (pivotPoint, pivotAngle, amount);
				
			if (pivotAmount >= pivotLimit) {
				RoundRotation();
				RoundPosition();
				st = muerte ? StMov.Mov:StMov.Parado;
				pivotAmount = 0;
			    actualizaPos (dir);
			}
			break;
		case StMov.Caer:
			//transform.GetComponent<Rigidbody>().useGravity = true;
			//transform.GetComponent<Rigidbody>().isKinematic = false;
			if (transform.position.y < bottomY)
				st = StMov.Morir;
			break;
		case StMov.Morir:
			morir ();
			break;
		case StMov.Ganar:
			ganar ();
			break;
		default:
			break;
		}
	}
	public void morir(){
		Destroy( this.gameObject );
		Game.S.playerDestroyed();
	}
	public void ganar(){
		if (ApplicationModel.ScoreMovs [ApplicationModel.ActualLevel] > movs || ApplicationModel.ScoreMovs [ApplicationModel.ActualLevel] < 1) {
			ApplicationModel.ScoreMovs [ApplicationModel.ActualLevel] = movs;
		}
		if (ApplicationModel.ScoreTime [ApplicationModel.ActualLevel] > ApplicationModel.MaxTime-Mathf.RoundToInt(time) || ApplicationModel.ScoreTime [ApplicationModel.ActualLevel] < 1)
			ApplicationModel.ScoreTime [ApplicationModel.ActualLevel] = ApplicationModel.MaxTime-Mathf.RoundToInt(time);
		if (ApplicationModel.MaxLevel == ApplicationModel.ActualLevel) {
			ApplicationModel.MaxLevel += 1;
		}
		Destroy( this.gameObject );
		Game.S.playerDestroyed();
	}
	public void actualizaPos(string dir){
		Utils.tr (reference);
		switch (dir) {
		case "D":
			x+=1+(int)reference.x;
			break;
		case "A":
			x-=1-(int)reference.x;
			break;
		case "S":
			y-=1-(int)reference.z;
			break;
		case "W":
			y+=1+(int)reference.z;
			break;
		default:
			break;
		}
	}

	public int[] getPos(){
		return new int[]{x,y};
	}
	
}