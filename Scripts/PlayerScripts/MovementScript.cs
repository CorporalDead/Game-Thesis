using UnityEngine;
using System.Collections;
using System.Collections;

public class MovementScript : MonoBehaviour {
	public bool isSprinting;

	private PauseMenu PauseMenuReference;

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
	private float Momentum;
	private bool isJumping;
	private bool isWallRunning;
	private bool isWallRunningLeft;
	private bool isWallRunningRight;
	private bool isCarrying;
	private bool jumpedFromLeft;
	private bool jumpedFromRight;
	private bool pointA;
	private bool pointB;
	// Use this for initialization
	void Start () {
		Cursor.visible=false;
		Cursor.lockState=CursorLockMode.Confined;

		playerController=GetComponent<CharacterController>();
		playerCamera=gameObject.GetComponentInChildren<Camera>();
		playerCamGO=GameObject.Find("PlayerCamera");

		PauseMenuReference=GetComponent<PauseMenu>();

		FieldOfViewValue=playerCamera.fieldOfView;
		Direction=Vector3.zero;
		playerTransform=transform;
		playerSpeed=3f;
		playerJumpSpeed=5f;
		runTimer=0.5f;
	}
	
	// Update is called once per frame
	void Update () {	

		if(!PauseMenuReference.getPaused()){
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
					if(playerSpeed<=6)
						playerSpeed+=0.1f;
					
					if(playerCamera.fieldOfView<FieldOfViewValue+10f)
						playerCamera.fieldOfView+=0.5f;

					isSprinting=true;
				}else{
					if(playerCamera.fieldOfView>FieldOfViewValue)
						playerCamera.fieldOfView-=0.5f;
					playerSpeed=3;
					isSprinting=false;
				}
				
				Direction.y=0;
				Direction=new Vector3(LeftRight,0,ForwardBackward);
				Direction=transform.TransformDirection(Direction);
				
				//Character Jump
				if(Input.GetKey(KeyCode.Space)&&(!isWallRunningLeft||!isWallRunningRight)){
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
					if(ForwardBackward>6){
					if(isWallRunningLeft){
						if(Input.GetKeyDown(KeyCode.Space)){
							//Debug.Log("Jump On");
							jumpedFromLeft=true;
							
							//Direction.y+=2f;
							
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
					if(Input.GetKeyDown(KeyCode.Space)){
							//Debug.Log("Jump off");
						jumpedFromRight=true;
						
						//Direction.y+=2f;
						
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
					Direction.y-=4f*Time.deltaTime;
			}
		}
				//force player to stop wallrunning 
				if(runTimer<0){
					isJumping=false;
					runTimer=0.5f;
				}
			}
			//Turn the character to where the player is looking at after each frame.
			playerController.Move(Direction*Time.deltaTime);

			playerTransform.localEulerAngles=new Vector3(playerTransform.localEulerAngles.x,playerRotY,playerTransform.localEulerAngles.z);
			playerCamGO.transform.localEulerAngles= new Vector3(-(playerRotX),playerCamGO.transform.localEulerAngles.y,playerCamGO.transform.localEulerAngles.z);

			RaycastHit leftRay;
			RaycastHit rightRay;
			
			Debug.DrawRay(playerCamGO.transform.position,-(playerCamGO.transform.right),Color.red);
			Debug.DrawRay(playerCamGO.transform.position,playerCamGO.transform.right,Color.green);
			
			//check left side if player is touching a wall
			if(Physics.Raycast(playerCamGO.transform.position,-(playerCamGO.transform.right),out leftRay,1.4f)&&(isJumping&&Input.GetButton("Sprint")&&ForwardBackward>6)){
				isWallRunningLeft=true;
				isWallRunning=true;
			}else{
				isWallRunningLeft=false;
				if(!isWallRunningRight)
				isWallRunning=false;
			}
			//check right side if player is touching a wall
			if(Physics.Raycast(playerCamGO.transform.position,playerCamGO.transform.right,out rightRay,1.4f)&&(isJumping&&Input.GetButton("Sprint")&&ForwardBackward>6)){
				isWallRunningRight=true;
				isWallRunning=true;
			}else{
				isWallRunningRight=false;
				if(!isWallRunningLeft)
				isWallRunning=false;
			}
			//carry items
			CarryMethod();
			Debug.Log(ForwardBackward);
		}
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
}