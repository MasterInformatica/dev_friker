using UnityEngine;
using System.Collections;

public class Player : PT_MonoBehaviour {
	public bool __________________ = false;
	private int _height = 0;
	private Vector3 _pos;


	// Estos dos bools definen el estado de la figura tumbada o de pie.
	public bool _______State______ = false;
	public bool inBase = true;
	public bool change = false; //true -> LR; false -> UD
	public bool _________________ = false;
	//----------------------------------------------------
	public bool isAnimating = false;
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
		if (!isAnimating) {

			Vector3 offset = Vector3.zero;
			int check = 1;
			string dir = "";
			if (Game.S.checkMapHole ()){	
				return;
			}else if(Input.GetKeyDown("up") || Input.GetKeyDown("w")){
				dir = "W";
				offset=Vector3.forward;
				//if(longways==1)offset=offset * 2;
				pivotAngle=Vector3.right;
			}else if(Input.GetKeyDown("down") || Input.GetKeyDown("s")){
				dir = "S";
				offset=-Vector3.forward;
				//if(longways==1)offset=offset * 2;
				pivotAngle=-Vector3.right;
			}else if(Input.GetKeyDown("left") || Input.GetKeyDown("a")){
				dir ="A";
				offset=-Vector3.right;
				//if(longways==2)offset=offset * 2;
				pivotAngle=Vector3.forward;
				check=2;
			}else if(Input.GetKeyDown("right") || Input.GetKeyDown("d")){
				dir = "D";
				offset=Vector3.right;
				//if(longways==2)offset=offset * 2;
				pivotAngle=-Vector3.forward;
				check=2;
			}else{
				return;
			}

			actualizaEstado(dir);
			/*int dif = Game.S.difHeight(dir);
			if(dif < 0){ // caida
				pivotLimit = 180.0f;
			}else if (dif == 0){ // mismo nivel
				pivotLimit = 90.0f;
			}else{ //subida
				pivotLimit = 180.0f;
			}*/
					

			if(offset!=Vector3.zero){
				Utils.tr (offset,transform.position);
				//Vector3 = transform.sca
				// TODO: llamar a getAltura y motificar el target position 

				pivotPoint=transform.position+offset/2;
				Utils.tr (pivotPoint);
				target.position=pivotPoint;
				isAnimating=true;
			}
		}else{
			float amount=Time.deltaTime * speed*20.0f;
			pivotAmount=pivotAmount + amount;
			//Utils.tr(pivotAmount,pivotPoint);
			amount=amount - (pivotAmount > 90.0f ? pivotAmount - 90.0f : 0.0f);
			transform.RotateAround(pivotPoint,pivotAngle, amount);
			
			if(pivotAmount >= 90f){
				target.position=transform.position;//hides the target after it completes rotating.
				isAnimating=false;
				pivotAmount=0;
			}
		}
	}

	public void actualizaEstado(string dir){
		//inBase = true si estoy en base
		//change
	}

	public int getAltura(){

		return 0;
	}




}