using UnityEngine;
using System.Collections;
using System.Collections;

public class MovementScript : MonoBehaviour {
	public GameObject ProjectileTest;
	public GameObject WeaponOnHand;
	
	private CharacterController playerController;
	private Transform playerTransform;
	private Camera playerCamera;
	private GameObject playerCamGO;
	private GameObject CarriedObject;
	private GameObject Projectile;
	private Vector3 Direction;
	
	private float playerSpeed;
	private float playerJumpSpeed;
	private float playerRotX;
	private float playerRotY;
	private float FieldOfViewValue;
	private float runTimer;
	private bool isJumping;
	private bool isWallRunning;
	private bool isWallRunningLeft;
	private bool isWallRunningRight;
	private bool isCarrying;
	private bool jumpedFromLeft;
	private bool jumpedFromRight;
	private bool pointA;
	private bool pointB;
	private bool Aiming;
	private int timer;
	// Use this for initialization
	void Start () {
		Cursor.visible=false;

		playerController=GetComponent<CharacterController>();
		playerCamera=gameObject.GetComponentInChildren<Camera>();
		playerCamGO=GameObject.Find("PlayerCamera");
		
		FieldOfViewValue=playerCamera.fieldOfView;
		Direction=Vector3.zero;
		playerTransform=transform;
		playerSpeed=3f;
		playerJumpSpeed=5f;
		runTimer=0.5f;
		timer=0;
	}
	
	// Update is called once per frame
	void Update () {	
		float ForwardBackward=Input.GetAxis("Vertical")*playerSpeed;
		float LeftRight=Input.GetAxis("Horizontal")*playerSpeed;
 

		playerRotX+=Input.GetAxis("Mouse Y")*1f;
		//correcting rotation so player will not bend over (and break their necks)
		if(playerRotX>85&&playerCamGO.transform.localEulerAngles.x>270)
			playerRotX-=Input.GetAxis("Mouse Y")*1f;
		else
			if(playerRotX<-85&&playerCamGO.transform.localEulerAngles.x>85)
				playerRotX-=Input.GetAxis("Mouse Y")*1f;
		
		playerRotY+=Input.GetAxis("Mouse X")*1f;
		
		if(playerController.isGrounded){
			//Character Sprint
			if(Input.GetButton("Sprint")&&ForwardBackward>0){
				ForwardBackward+=playerSpeed;
				
				if(playerCamera.fieldOfView<FieldOfViewValue+10f)
					playerCamera.fieldOfView+=0.5f;

			}else{
				if(playerCamera.fieldOfView>FieldOfViewValue)
					playerCamera.fieldOfView-=0.5f;
			}
			
			Direction.y=0;
			Direction=new Vector3(LeftRight,0,ForwardBackward);
			Direction=transform.TransformDirection(Direction);
			
			//Character Jump
			if(Input.GetButtonDown("Jump")&&(!isWallRunningLeft||!isWallRunningRight)){
				Direction.y+=5f;
				isJumping=true;
			}

		}else{
			if(!isWallRunning){
				Direction.y-=20f*Time.deltaTime;
				isJumping=false;
				runTimer=0.5f;

				if(playerCamGO.transform.localEulerAngles.z>=345f&&playerCamGO.transform.localEulerAngles.z!=0f)
					playerCamGO.transform.Rotate(0,0,1);
				//check if player was wallrunning on the right side then correct the rotation
				if(playerCamGO.transform.localEulerAngles.z<=345f&&playerCamGO.transform.localEulerAngles.z!=0f)
					playerCamGO.transform.Rotate(0,0,-1);
			}else{
				if(isWallRunningLeft){
					if(Input.GetButtonDown("Jump")){
						//Debug.Log("Jump On");
						jumpedFromLeft=true;
						
						Direction.y+=2f;
						
						Direction.x=0;
						Direction.z=0;
						Direction.x+=5f;
						Direction.z+=5f;
						Direction = transform.TransformDirection(Direction);
				}
					if((playerCamGO.transform.localEulerAngles.z>=0f&&playerCamGO.transform.localEulerAngles.z<=1f)||playerCamGO.transform.localEulerAngles.z>=350f)
						playerCamGO.transform.Rotate(0,0,-1);
					//decrement runtimer so player does not wall run forever
				runTimer-=Time.deltaTime;
			}else
			if(isWallRunningRight){
				if(Input.GetButtonDown("Jump")){
						//Debug.Log("Jump off");
					jumpedFromRight=true;
					
					Direction.y+=2f;
					
					Direction.x=0;
					Direction.z=0;
					Direction.x-=5f;
					Direction.z+=5f;
					Direction = transform.TransformDirection(Direction);
				}
					//tilt camera to the right
					if((playerCamGO.transform.localEulerAngles.z>=359f&&playerCamGO.transform.localEulerAngles.z<=359.9f)||playerCamGO.transform.localEulerAngles.z<=10f)
						playerCamGO.transform.Rotate(0,0,1);

				runTimer-=Time.deltaTime;
			}
		}
			//force player to stop wallrunning 
			if(runTimer<0){
				isJumping=false;
				runTimer=0.5f;
			}
		}

		playerController.Move(Direction*Time.deltaTime);
		
		playerTransform.localEulerAngles=new Vector3(playerTransform.localEulerAngles.x,playerRotY,playerTransform.localEulerAngles.z);
		playerCamGO.transform.localEulerAngles= new Vector3(-(playerRotX),playerCamGO.transform.localEulerAngles.y,playerCamGO.transform.localEulerAngles.z);

		RaycastHit leftRay;
		RaycastHit rightRay;
		
		Debug.DrawRay(playerCamGO.transform.position,-(playerCamGO.transform.right),Color.red);
		Debug.DrawRay(playerCamGO.transform.position,playerCamGO.transform.right,Color.green);
		
		//check left side if player is touching a wall
		if(Physics.Raycast(playerCamGO.transform.position,-(playerCamGO.transform.right),out leftRay,1)&&(isJumping&&Input.GetButton("Sprint"))){
			isWallRunningLeft=true;
			isWallRunning=true;
		}else{
			isWallRunningLeft=false;
			if(!isWallRunningRight)
			isWallRunning=false;
		}
		//check right side if player is touching a wall
		if(Physics.Raycast(playerCamGO.transform.position,playerCamGO.transform.right,out rightRay,1)&&(isJumping&&Input.GetButton("Sprint"))){
			isWallRunningRight=true;
			isWallRunning=true;
		}else{
			isWallRunningRight=false;
			if(!isWallRunningLeft)
			isWallRunning=false;
		}
		//carry items
		CarryMethod();
		//creating projectiles
		ProjectileMaker();
	}
	
	void OnCollisionStay(Collision col){
		if(col.gameObject.tag=="RunnableSurface")
			Debug.Log("WallRunSurface");
	}
	
	void CarryMethod(){
		//carry
		RaycastHit hit;
		Debug.DrawRay(playerCamGO.transform.position,playerCamGO.transform.forward,Color.cyan);
		
		if(Physics.Raycast(playerCamGO.transform.position,playerCamGO.transform.forward,out hit,3)){
			
			if(Input.GetKeyDown(KeyCode.E)){
				if(isCarrying)
					isCarrying=false;
				else
					if(!isCarrying)
						isCarrying=true;
			}
			
			if(hit.collider.gameObject.tag=="Carry"&&isCarrying){
				CarriedObject = hit.collider.gameObject;
				
				CarriedObject.GetComponent<Rigidbody>().useGravity=false;
				
				CarriedObject.transform.position=GameObject.Find("CarryPoint").transform.position;
				
			}
			else{
				if(CarriedObject!=null){
					if(CarriedObject.GetComponent<Rigidbody>().useGravity==false)
						CarriedObject.GetComponent<Rigidbody>().useGravity=true;
				}
			}
		}
	}
	void ProjectileMaker(){
		if(timer>=5){
			if(Input.GetButton("Fire1")){
				Projectile=Instantiate(ProjectileTest,GameObject.Find("Barrel").transform.position,Quaternion.identity) as GameObject;
				Projectile.GetComponent<Rigidbody>().AddForce(GameObject.Find("Barrel").transform.forward*5000);
			
				timer=0;
				Destroy(Projectile,3);
			}
	}

			if(Input.GetButtonDown("Fire2")){
				if(!Aiming){
					Aiming=true;
					
				if(playerCamera.fieldOfView>FieldOfViewValue-20f)
					playerCamera.fieldOfView-=20f;
			}
				else
					if(Aiming){
						Aiming=false;

					if(playerCamera.fieldOfView<FieldOfViewValue)
						playerCamera.fieldOfView+=20f;
			}

		if(Aiming)
			GameObject.Find("KrissVector").transform.position = GameObject.Find("AimPoint").transform.position;
		else
			if(!Aiming)
				GameObject.Find("KrissVector").transform.position = GameObject.Find("HipFire").transform.position;
		}
		timer++;
}
	void grapplingHook(){
		if(Input.GetButtonDown("Fire2")){
			Debug.Log ("1");
		}
	}
}