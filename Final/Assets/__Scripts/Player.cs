using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public enum StMov {Parado,Mov,Ajustar,Caer,Morir,Ganar};

public class Player : PT_MonoBehaviour {
	public float limitAmount = 100f;
	public static float bottomY = -20f; // limite de caida para eliminar el prefab
	public float speed = 300.0f; // velocidad de movimiento
	public bool __________________ = false;
	private int _height = 0; // heredado de pintar el mapa por tiles
	private Vector3 _pos; // heredado
	public int x, y; // posicion actual del jugador en el mapa
	public bool _______State______ = false;
	public StMov st = StMov.Parado; // estado en que se encuentra el jugador
	public bool muerte = false; // indica si depues de moverse debe morir
	public bool _________________ = false;
	//----------------------------------------------------
	public bool _______Walk_____ = false;
	public string dir; // direccion seleccionada
	// variables de giro
	public Vector3 pivotPoint = Vector3.zero; // punto de giro
	public Vector3 pivotAngle = Vector3.zero; // angulo de giro
	public Vector3 reference = Vector3.zero; // vector unitario con la direccion de la figura
	public float pivotAmount = 0.0f; // cuanto llevo girado
	public float pivotLimit = 90.0f; // cuantos grados debo girar
	public Transform pivote; // Elemento de giro, para visionar los calculos
	public float acamount = 0.0f;
	public bool ___________________ = false;
	// estas variables tendrian mas sentido en la clase Game
	// estan aqui para hacer mas sencillo su acceso y dado que solo
	// existe un player por game, no hay problema
	public int movs = 0; 
	public float time = 0;
	public float endTime = 0;

		
	new public Vector3 pos { // heredada...
		get { return( _pos ); }
		set {
			_pos = value;
			x = (int)_pos.x;
			y = (int)_pos.z;
			AdjustHeight();
		}
	}


	public bool isVertical(){
		/* 
		 * Indica si el player esta apoyado en alguna de las bases
		 * Simplemente es mirar si Rotation.x es cero y Rotation.z es cero o 180
		 */
		Utils.tr (transform.eulerAngles.z,"angle");
		return Mathf.Approximately(transform.eulerAngles.x, 0.0f) && ( Mathf.Approximately(transform.eulerAngles.z, 0.0f) || Mathf.Approximately(transform.eulerAngles.z,180.0f) );
	}

	public bool inBase(){
		/* 
		 * Indica si el player esta apoyado en la base objetivo	
		 * Sera si Rotation.x y Rotation.z son cero
		 */
		return Mathf.Approximately(transform.eulerAngles.x, 0.0f) && Mathf.Approximately(transform.eulerAngles.z, 0.0f);
	}

	public void RoundRotation(){
		/* 
		 * Esta funcion asegura que al mover el cubo siempre quede
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
		/*
		 * Esta funcion asegura que al mover el cubo siempre quede
		 * bien posicionadas sus componentes de posicion, evita
		 * errores de division, redondea a los ".5" o ".0" mas cercano
		 */
		Vector3 vec = transform.position;
		vec.x = Mathf.Round (vec.x / 0.5f) * 0.5f;
		vec.y = Mathf.Round (vec.y / 0.5f) * 0.5f;
		vec.z = Mathf.Round (vec.z / 0.5f) * 0.5f;
		transform.position = vec;
	}

	// Methods
	public void AdjustHeight() { // heredada ....
		// Moves the block up or down based on _height
		Vector3 vertOffset = Vector3.back*(_height);
		// The -0.5f shifts the Tile down 0.5 units so that it's top surface is
		// at z=0 when pos.z=0 and height=0
		transform.position = _pos+vertOffset;
	}

	public void Start(){
		// creamos el pivote que sirve para mover el jugador
		pivote=GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
		pivote.localScale= new Vector3(0.1f,0.1f,0.1f);
		pivote.name="Pivote";
		DestroyImmediate(pivote.GetComponent<Collider>()); // no queremos collider
		pivote.GetComponent<Renderer>().enabled = false; // no queremos que se vea
		endTime = Time.time + ApplicationModel.MaxTime; // preparamos el contador regresivo
	}
	public string getDir(){
		/*
		 * Esta funcion encapsula la pregunta de la tecla pulsada devolviendo un
		 * unico caracter por cada direccion. Hace posible abstraer las teclas 
		 * utilizadas.
		 */
		if (Input.GetKeyDown ("up") || Input.GetKeyDown ("w")) {
			return "W";
		} else if (Input.GetKeyDown ("down") || Input.GetKeyDown ("s")) {
			return "S";
		} else if (Input.GetKeyDown ("left") || Input.GetKeyDown ("a")) {
			return "A";
		} else if (Input.GetKeyDown ("right") || Input.GetKeyDown ("d")) {
			return "D";
		}
		return "none"; // no se ha pulsado ninguna
	}
	public void updateText(){
		/*
		 * Mantengamos informado al jugador de su tiempo y movimientos
		 */
		Game.S.movsText.text = "Movimientos: " + movs;
		Game.S.timeText.text = "Tiempo: " + Mathf.RoundToInt(time) + " s";
	}
	public void checkTime(){
		/*
		 * Mirar si queda tiempo para jugar
		 */
		time = endTime - Time.time;
		if (time < 0.0f) {
			time = 0.0f;
			st = StMov.Morir;
		}
	}
	public void Update () {
		checkTime (); // lo primero es mirar si queda tiempo para jugar
		updateText (); // lo segundo actualizar lo que ve el jugador
		switch (st) { // "maquina de estados"
			case StMov.Parado: // esperando una tecla
			if (Game.S.checkMapHole (x,y)) { // si hay un agujero donde estoy
				st = StMov.Caer;
				break;
			} else if (Game.S.checkGoal(x,y) && inBase ()){ // estoy en el objetivo y bien puesto
				st = StMov.Ganar;
				break;
			}	
			dir = getDir (); // obtener direccion seleccionada
			if(dir == "none") break; // ninguna direccion
			movs ++; // un movimiento mas en este nivel

			// variables para colocar el pivote
			float sentido = 1.0f; // 1 o -1 para generalizar cuentas
			Vector3 altura = new Vector3(0.0f,0.5f,0.0f); // mitad de altura de la figura 
			Vector3 offset = Vector3.zero; // hacia que lado (izq o der) debo mover el pivote
			reference = transform.position * 2; // si mi figura es de dos pisos, hacia donde estan esos dos pisos
			reference.x = reference.x %2;
			reference.y = reference.y %2;
			reference.z = reference.z %2;
			// reference deja un 1.0 en el sentido que esten los dos pisos.
			float tall = transform.localScale.y/2.0f; // mitad de altura (0.5 o 1)

			switch(dir)
			{
			case "S": // como W pero en sentido contrario
				sentido = -1.0f;
				goto case "W";
			case "W":
				offset     = Vector3.forward * sentido; // hacia adelante
				pivotAngle = Vector3.right   * sentido; // angulo a la derecha
				if(isVertical()){ //la base es cuadrada de tamaño 1
					altura = new Vector3(0.0f,tall,0.0f); // la correccion de la altura es con tall
				}else{ // no estoy en la base... :(
					if(reference.z>0){ // me muevo a W o S y los dos pisos estan en mi direccion
						offset *= transform.localScale.y;
					}
				}
				offset *= 0.5f;
				break;
			case "A": // como D pero en sentido contrario
				sentido = -1.0f;
				goto case "D";
			case "D":
				offset     = Vector3.right   * sentido; // hacia la derecha
				pivotAngle = Vector3.forward * ( -1.0f * sentido ); // hacia delante
				if(isVertical()){ //la base siempre es cuadrada de tamaño 1
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

			int dif = Game.S.difHeight(dir); // calcular la diferencia de altura

			if(dif < 0){ // caida
				muerte = dif < -1; // si hay mas de un piso moriremos
				pivotLimit = 180.0f;
			}else if (dif == 0){ // mismo nivel
				pivotLimit = 90.0f;
			}else if (dif == 1){ //subida
				pivotLimit = 180.0f;
				altura = -altura; // el pivote en lugar de estar en la base inferior estara en la superior
			}else{
				break; // algo fue mal
			}
			pivotPoint = transform.position - altura + offset;  // posicion del pivote
			pivote.position = pivotPoint; // pintado del pivote
			st = StMov.Mov; // siguiente paso moverse
			break;
		case StMov.Mov: // el jugador se esta moviendo
			float amount = Time.deltaTime * speed * 20.0f;
			pivotAmount = pivotAmount + amount;
			amount = amount - (pivotAmount > pivotLimit ? pivotAmount - pivotLimit : 0.0f);
			transform.RotateAround (pivotPoint, pivotAngle, amount); // girar lo que corresponde
				
			if (pivotAmount >= pivotLimit) { // termine de girar
				RoundRotation(); // redondear mi rotacion
				RoundPosition(); // redondear mi posicion
				acamount= 0.0f;
				st = muerte ? StMov.Caer:StMov.Parado; // si me movia hacia la muerte muere
				pivotAmount = 0;
			    actualizaPos (dir); // actualizar posicion logica
			}
			break;
		case StMov.Caer: // tienes que caer.. cae!
			transform.GetComponent<Rigidbody>().useGravity = true;
			transform.GetComponent<Rigidbody>().isKinematic = false;
			acamount += Time.deltaTime * speed * 20.0f; // si caes por desnivel
			if (muerte && acamount > limitAmount) // te dejamos caer un poco
				st = StMov.Morir;
			if (transform.position.y < bottomY) // si caes por agujero.. un poco diferente
				st = StMov.Morir;
			break;
		case StMov.Morir: // tienes que morir.. MUERE!
			morir ();
			break;
		case StMov.Ganar: // no se como pero has ganado ¬¬
			ganar ();
			break;
		default:
			break;
		}
	}
	public void morir(){
		Destroy( this.gameObject ); // morir es morir
		Game.S.playerDestroyed(); // avisa al juego de la muerte
	}
	public void ganar(){ 
		/*
		 * Ganar implica actualizar records (si mejorar mejorarlo)
		 * y luego morir :D
		 */
		if (ApplicationModel.ScoreMovs [ApplicationModel.ActualLevel] > movs || ApplicationModel.ScoreMovs [ApplicationModel.ActualLevel] < 1) {
			ApplicationModel.ScoreMovs [ApplicationModel.ActualLevel] = movs;
		}
		if (ApplicationModel.ScoreTime [ApplicationModel.ActualLevel] > ApplicationModel.MaxTime-Mathf.RoundToInt(time) || ApplicationModel.ScoreTime [ApplicationModel.ActualLevel] < 1)
			ApplicationModel.ScoreTime [ApplicationModel.ActualLevel] = ApplicationModel.MaxTime-Mathf.RoundToInt(time);
		if (ApplicationModel.MaxLevel == ApplicationModel.ActualLevel) {
			ApplicationModel.MaxLevel += 1;
		}
		morir (); // hay que volver al menu :P
	}
	public void actualizaPos(string dir){
		switch (dir) { // muevo x e y en funcion de la tecla pulsada y de si en esa direccion esta el lado largo
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

	public int[] getPos(){ // heredado...
		return new int[]{x,y};
	}
	
}