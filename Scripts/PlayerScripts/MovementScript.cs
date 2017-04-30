using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MovementScript : MonoBehaviour {
	public static MovementScript _MovementScript;
	public Animator playerAnim;

	public bool isSprinting;

	public GameObject[] mainWep;

	private GameObject hpText;
	public float stamina;
	public float staminaTimer;
    public float playerHP;
    public float HPTimer;

	private CharacterController playerController;
	private Transform playerCamGO;
    private Transform WeaponCamGO;
	private Transform playerCamGOTPS;
	private GameObject CarriedObject;
	private GameObject persistentHolder;

	public Vector3 Direction;
	private Vector3 playerCamGOEuler;
	private Vector3 playerCamGOTPSEuler;

    RaycastHit downRay;

    public float playerSpeed;
	public float playerBaseSpeed;
	public float playerJumpSpeed;

	public float playerRotX;
	public float playerRotY;

	private float FieldOfViewValue;
    private float weaponFieldOfViewValue;
	public float distanceToGround;

	internal bool isJumping;
	internal bool isWallRunning;
	internal bool isWallRunningLeft;
	internal bool isWallRunningRight;
	internal bool isCarrying;
	internal bool jumpedFromLeft;
	internal bool jumpedFromRight;

	public bool explodeJump;
    public bool isHit;

    private GameObject dmgIndicator;


    private float dmgTimer;
    // Use this for initialization
    void Start () {
        GameObject.Find("LoadScreen").SetActive(false);

       

        dmgIndicator = GameObject.Find("dmg");
        dmgIndicator.SetActive(false);

        hpText =GameObject.Find("HPText");

		Cursor.visible=false;
		Cursor.lockState=CursorLockMode.Confined;	

		playerCamGO = transform.FindChild("PlayerCameraFPS");
        WeaponCamGO = playerCamGO.transform.FindChild("WeaponCamera");
		playerCamGOTPS = transform.FindChild ("PlayerCameraTPS");

		playerController = transform.GetComponent<CharacterController>();

		playerCamGOEuler = playerCamGO.eulerAngles;
		playerCamGOTPSEuler = playerCamGOTPS.eulerAngles;

        distanceToGround = playerController.bounds.extents.y;

		FieldOfViewValue = playerCamGO.GetComponent<Camera>().fieldOfView;
        weaponFieldOfViewValue = WeaponCamGO.GetComponent<Camera>().fieldOfView;
		Direction = Vector3.zero;

		playerSpeed=10f;
		playerBaseSpeed=playerSpeed;
		playerJumpSpeed=2f;

		_MovementScript=this;

		stamina=100f;
        HPTimer = 0;
		staminaTimer=0;

		mainWep = new GameObject[2];

		persistentHolder = GameObject.Find("PersistentValues");

		playerCamGO.gameObject.SetActive(true);
		playerCamGOTPS.gameObject.SetActive (false);

        if (persistentHolder.GetComponent<PersistentValues>().getBM())
        {
            playerHP = 200;
        }
        if (persistentHolder.GetComponent<PersistentValues>().getMedic())
        {
            playerHP = 120;
        }
        if (persistentHolder.GetComponent<PersistentValues>().getCQBClass())
        {
            playerHP = 100;
        } 
        if(persistentHolder.GetComponent<PersistentValues>().getSharp())
            playerHP = 100;

       // hpText.GetComponent<Text>().text = playerHP.ToString();

    }
	// Update is called once per frame
	void Update () {

        Movement();

        if (isHit)
        {
            dmgIndicator.SetActive(true);
        }

        if (dmgIndicator.activeInHierarchy)
        {
            dmgTimer += Time.deltaTime;

            if(dmgTimer >= 0.5f)
            {
                dmgIndicator.SetActive(false);
                dmgTimer = 0;
            }

            if(playerHP <= 0)
            {
                dmgIndicator.SetActive(false);
            }
        }

     
    }

	void CarryMethod(){
		//carry
		RaycastHit hit;
		Debug.DrawRay(playerCamGO.position,playerCamGO.forward,Color.cyan);
		
		if(Physics.Raycast(playerCamGO.position,playerCamGO.forward,out hit,5f)){
			
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
	void Movement(){

		float ForwardBackward=Input.GetAxis("Vertical")*playerSpeed;
		float LeftRight=Input.GetAxis("Horizontal")*playerSpeed;

		playerRotX+=Input.GetAxis("Mouse Y")*1f;
		//correcting rotation so player will not bend over (and break their necks)
		if(playerCamGO.gameObject.activeInHierarchy)
			playerRotX=Mathf.Clamp(playerRotX,-80,80);
		
		
		playerRotY+=Input.GetAxis("Mouse X")*1f;

		RegenStamina();
        RegenHealth();

      
		if(Physics.Raycast(transform.position, Vector3.down, out downRay, distanceToGround) && downRay.transform.tag!="Enemy" && downRay.transform.tag!="Player" && downRay.transform.tag!="Survivor" && downRay.transform.tag!="PowerUp")
        {
           
            //    Debug.Log(downRay.collider.bounds.size);
            //     GameObject.Find("INFO").GetComponent<Text>().text = "Distance from the bottom: " + Mathf.Abs(transform.position.y - downRay.collider.gameObject.transform.position.y);
            // Debug.Log("Distance from the bottom: "+Mathf.Abs(transform.position.y - downRay.collider.gameObject.transform.position.y)+" Current Level:"+ Application.loadedLevel);
            //if (Application.loadedLevel == 2) //test Map
            //{
            //    if (Mathf.Abs(transform.position.y - downRay.collider.gameObject.transform.position.y) < 2.2f)
            //    {
            //        if (Mathf.Abs(transform.position.y - downRay.collider.gameObject.transform.position.y) > 0.6f)
            //        {
            //            transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
            //        }
            //    }
            //}

            //if(Application.loadedLevel == 1)
            //{
            //    if (Mathf.Abs(transform.position.y - downRay.collider.gameObject.transform.position.y) < 1.6f)
            //    {
            //        if (Mathf.Abs(transform.position.y - downRay.collider.gameObject.transform.position.y) > 0.6f && Mathf.Abs(transform.position.y - downRay.collider.gameObject.transform.position.y) < 0.8f)
            //        {
            //            transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
            //        }
            //        if(Mathf.Abs(transform.position.y - downRay.collider.gameObject.transform.position.y) < 0.2f)
            //        {
            //            transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
            //        }
            //    }
            //}
			//Character Sprint
			if(Input.GetKey(KeyCode.LeftShift)&&ForwardBackward>0&&Mathf.Round(stamina)>0){
					if(playerSpeed<=14)
						playerSpeed+=0.1f;

					if(playerCamGO.GetComponent<Camera>().fieldOfView<FieldOfViewValue+10f)
					playerCamGO.GetComponent<Camera>().fieldOfView+=40*Time.deltaTime;

					isSprinting=true;

					stamina-=15*Time.deltaTime;
					staminaTimer=0;

            }
            else{
                if (playerCamGO.GetComponent<Camera>().fieldOfView > FieldOfViewValue)
                {
                    playerCamGO.GetComponent<Camera>().fieldOfView -= 40 * Time.deltaTime;
                }

                if (!persistentHolder.GetComponent<PersistentValues>().getBM())
                {
                    if (WeaponBobbing.wepBobRef.isAiming)
                    {
                        if (!WeaponBobbing.wepBobRef.isSniper)
                        {
                            if (playerCamGO.GetComponent<Camera>().fieldOfView > FieldOfViewValue - 10f)
                            {
                                playerCamGO.GetComponent<Camera>().fieldOfView -= 40 * Time.deltaTime;
                            }
                            if (WeaponCamGO.GetComponent<Camera>().fieldOfView > FieldOfViewValue - 20f)
                            {
                                WeaponCamGO.GetComponent<Camera>().fieldOfView -= 80 * Time.deltaTime;
                            }
                        }
                        else
                        {
                            if (playerCamGO.GetComponent<Camera>().fieldOfView > FieldOfViewValue - 40f)
                            {
                                playerCamGO.GetComponent<Camera>().fieldOfView -= 220 * Time.deltaTime;
                            }
                            if (WeaponCamGO.GetComponent<Camera>().fieldOfView > FieldOfViewValue - 40f)
                            {
                                WeaponCamGO.GetComponent<Camera>().fieldOfView -= 0 * Time.deltaTime;
                            }
                        }
                    } else
                        if (!WeaponBobbing.wepBobRef.isAiming)
                    {
                        if (!WeaponBobbing.wepBobRef.isSniper)
                        {
                            if (playerCamGO.GetComponent<Camera>().fieldOfView < FieldOfViewValue)
                            {
                                playerCamGO.GetComponent<Camera>().fieldOfView += 40 * Time.deltaTime;
                            }
                            if (WeaponCamGO.GetComponent<Camera>().fieldOfView < FieldOfViewValue)
                            {
                                WeaponCamGO.GetComponent<Camera>().fieldOfView += 80 * Time.deltaTime;
                            }
                        }
                        else
                        {
                            if (playerCamGO.GetComponent<Camera>().fieldOfView < FieldOfViewValue)
                            {
                                playerCamGO.GetComponent<Camera>().fieldOfView = 60;
                            }
                            if (WeaponCamGO.GetComponent<Camera>().fieldOfView < FieldOfViewValue)
                            {
                                WeaponCamGO.GetComponent<Camera>().fieldOfView = 60;    
                            }
                        }
                    }          
                }

				if(playerSpeed>playerBaseSpeed)
					playerSpeed-=0.1f;
				isSprinting=false;

			}

			Direction.y=0;
			Direction=new Vector3(LeftRight,0,ForwardBackward);
			Direction=transform.TransformDirection(Direction);

			//Character Jump
			if(Input.GetKeyDown(KeyCode.Space)&&(!isWallRunningLeft||!isWallRunningRight))
				Direction.y+=15f;

            if (Mathf.Abs(downRay.point.y - playerController.bounds.center.y) < 2.2f && !isWallRunningRight && !isWallRunningLeft)
            {
                Direction.y += 3.5f;
            }

            isJumping =false;	
		}else{

			string yDirection = Direction.y.ToString();

			//if((!yDirection.Contains("E")||Direction.y!=0)||GetComponent<GrapplingHook>().attached){
				if(!isWallRunning){

					//if(GetComponent<GrapplingHook>().enabled){
					//	if(GetComponent<GrapplingHook>().attached){
					//	//	Direction.y+=90f*Time.deltaTime;
					//		isJumping=false;
     //               }else
     //                   {
                            Direction.y -= 50f * Time.deltaTime;
       //                 }
                    //}

                    //if (!persistentHolder.GetComponent<PersistentValues>().getCQBClass() && !persistentHolder.GetComponent<PersistentValues>().getBM())
                    //{
                    //    Direction.y -= 50f * Time.deltaTime;
                    //}

					if(playerCamGO.gameObject.activeInHierarchy){
						if(playerCamGO.localEulerAngles.z>=345f&&playerCamGO.localEulerAngles.z!=0f)
							playerCamGO.Rotate(0,0,50*Time.deltaTime);
						
					//check if player was wallrunning on the right side then correct the rotation
						if(playerCamGO.localEulerAngles.z<=345f&&playerCamGO.localEulerAngles.z!=0f)
							playerCamGO.Rotate(0,0,-50*Time.deltaTime);
					}
						
				}else{

					if(ForwardBackward>6&&Mathf.Round(stamina)>=0f){
						if(isWallRunningLeft){
							if(Input.GetKeyDown(KeyCode.Space)){
								jumpedFromLeft=true;

								Direction.x=0;
								Direction.z=0;
								Direction.x+=7.5f;
								Direction.y+=10f;
								Direction.z+=15f;
								Direction = transform.TransformDirection(Direction);
							}
							if(playerCamGO.gameObject.activeInHierarchy){
								if((playerCamGO.localEulerAngles.z>=0f&&playerCamGO.localEulerAngles.z<=1f)||playerCamGO.localEulerAngles.z>=350f)
									playerCamGO.Rotate(0,0,-50*Time.deltaTime);
							}

						}else
							if(isWallRunningRight){
								if(Input.GetKeyDown(KeyCode.Space)){
									jumpedFromRight=true;

									Direction.x=0;
									Direction.z=0;
									Direction.x-=7.5f;
									Direction.y+=10f;
									Direction.z+=15f;
									Direction = transform.TransformDirection(Direction);
								}	
								//tilt camera to the right
								if(playerCamGO.gameObject.activeInHierarchy){
									if((playerCamGO.localEulerAngles.z>=357f&&playerCamGO.localEulerAngles.z<=360f)||playerCamGO.localEulerAngles.z<=10f)
										playerCamGO.Rotate(0,0,50*Time.deltaTime);
								}
									
							}
						Direction.y-=30f*Time.deltaTime;

						if(Direction.y<0.00001f) //climbs up a certain distance before 'sticking' to the wall
							Direction.y-=Direction.y;
						
						stamina-=20*Time.deltaTime;
					}
				}
            //force player to stop wallrunning 
            if (Physics.Raycast(transform.position, Vector3.down, distanceToGround) || Mathf.Round(stamina) <= 0)
            {
                isJumping = false;
            }
			//	}
		//	}
		}
		if(!Physics.Raycast(transform.position, Vector3.down,distanceToGround))
			isJumping=true;

        // if (Physics.Raycast(transform.position, Vector3.down, distanceToGround))
        //  {
        //    Direction.y += 5f * Time.deltaTime;
        // }
      //  Debug.Log(Direction.y);
        //Turn the character to where the player is looking at after each frame.
        playerController.Move(Direction*Time.deltaTime);
        
		transform.localEulerAngles=new Vector3(transform.localEulerAngles.x,playerRotY,transform.localEulerAngles.z);

		if(playerCamGO.gameObject.activeInHierarchy)
			playerCamGO.localEulerAngles= new Vector3(-(playerRotX),playerCamGO.localEulerAngles.y,playerCamGO.localEulerAngles.z);
		
		RaycastHit leftRay;
		RaycastHit rightRay;

		//check left side if player is touching a wall
		Debug.DrawRay(playerController.transform.position,-(playerController.transform.right),Color.red);
		Debug.DrawRay(playerController.transform.position,(playerController.transform.right),Color.cyan);

		if(Physics.Raycast(playerController.transform.position,-(playerController.transform.right),out leftRay,2f)&&(isJumping&&Input.GetKey(KeyCode.LeftShift)&&ForwardBackward>6)){
			if(leftRay.collider.tag!="Player" && leftRay.collider.tag!="Enemy" && leftRay.collider.tag!="Blockade"){
				isWallRunningLeft=true;
				isWallRunning=true;
			}
		}else{
			isWallRunningLeft=false;
			if(!isWallRunningRight)
				isWallRunning=false;
		}
		//check right side if player is touching a wall
		if(Physics.Raycast(playerController.transform.position,playerController.transform.right,out rightRay,2f)&&(isJumping&&Input.GetKey(KeyCode.LeftShift)&&ForwardBackward>6)){
			if(rightRay.collider.tag!="Player" && rightRay.collider.tag!="Enemy" && rightRay.collider.tag != "Blockade")
            {
				isWallRunningRight=true;
				isWallRunning=true;
			}
		}else{
			isWallRunningRight=false;
			if(!isWallRunningLeft)
				isWallRunning=false;
		}
		//carry items
		CarryMethod();
       
        //Debug.Log(playerController.bounds.);
    }
		
	private void RegenStamina(){

		if(staminaTimer>2f&&stamina<=100){
			stamina+=40*Time.deltaTime;
		}else
			StaminaCoolDown();
	}
	private void StaminaCoolDown(){
		staminaTimer+=1*Time.deltaTime;
	}
    private void RegenHealth()
    {
        if (HPTimer > 2f)
        {

            if (persistentHolder.GetComponent<PersistentValues>().getBM())
            {
                if (playerHP <= 200)
                {
                    playerHP += 5 * Time.deltaTime;
                }
            }
            else
                if (persistentHolder.GetComponent<PersistentValues>().getCQBClass())
            {
                if (playerHP <= 100)
                {
                    playerHP += 5 * Time.deltaTime;
                }
            }
            else
                if (persistentHolder.GetComponent<PersistentValues>().getSharp())
            {
                if (playerHP <= 100)
                {
                    playerHP += 5 * Time.deltaTime;
                }
            }
            else
                if (persistentHolder.GetComponent<PersistentValues>().getMedic())
            {
                if (playerHP <= 120)
                {
                    playerHP += 5 * Time.deltaTime;
                }
            }
            isHit = false;
        }

        if (!isHit)
        {
            HPTimer = 0;
        } else
        {
            HPTimer += 1 * Time.deltaTime;
        }

        List<GameObject> medicPods = new List<GameObject>();
        List<GameObject> bomberSmokes = new List<GameObject>();

        bomberSmokes.AddRange(GameObject.FindGameObjectsWithTag("Farts"));
        medicPods.AddRange(GameObject.FindGameObjectsWithTag("PowerUp"));

        if (PhotonNetwork.isMasterClient)
        {
            foreach (GameObject mP in medicPods)
            {
                if (Vector3.Distance(transform.position, mP.transform.position) < 10f && mP.GetComponent<Rigidbody>().velocity.magnitude == 0 && mP.GetComponent<MedicPodBehavior>().lifeTime > 0)
                {
                    if (persistentHolder.GetComponent<PersistentValues>().getBM())
                    {
                        if (playerHP <= 200)
                        {
                            playerHP += 15 * Time.deltaTime;
                        }
                    }
                    else
                   if (persistentHolder.GetComponent<PersistentValues>().getCQBClass())
                    {
                        if (playerHP <= 100)
                        {
                            playerHP += 15 * Time.deltaTime;
                        }
                    }
                    else
                   if (persistentHolder.GetComponent<PersistentValues>().getSharp())
                    {
                        if (playerHP <= 100)
                        {
                            playerHP += 15 * Time.deltaTime;
                        }
                    }
                    else
                   if (persistentHolder.GetComponent<PersistentValues>().getMedic())
                    {
                        if (playerHP <= 120)
                        {
                            playerHP += 15 * Time.deltaTime;
                        }
                    }
                }
            }

            foreach (GameObject f in bomberSmokes)
            {
                if (Vector3.Distance(transform.position,f.transform.position) < 20f)
                {
                    playerHP -= 5 * Time.deltaTime;
                }
            }
        }else
        {
            foreach (GameObject f in bomberSmokes)
            {
                if (Vector3.Distance(transform.position, f.transform.position) < 15f)
                {
                    playerHP -= 5 * Time.deltaTime;
                }
            }

            foreach (GameObject mP in medicPods)
                {
                    if (Vector3.Distance(transform.position, mP.transform.position) < 10f)
                    {
                        if (persistentHolder.GetComponent<PersistentValues>().getBM())
                        {
                            if (playerHP <= 200)
                            {
                                playerHP += 15 * Time.deltaTime;
                            }
                        }
                        else
                       if (persistentHolder.GetComponent<PersistentValues>().getCQBClass())
                        {
                            if (playerHP <= 100)
                            {
                                playerHP += 15 * Time.deltaTime;
                            }
                        }
                        else
                       if (persistentHolder.GetComponent<PersistentValues>().getSharp())
                        {
                            if (playerHP <= 100)
                            {
                                playerHP += 15 * Time.deltaTime;
                            }
                        }
                        else
                       if (persistentHolder.GetComponent<PersistentValues>().getMedic())
                        {
                            if (playerHP <= 120)
                            {
                                playerHP += 15 * Time.deltaTime;
                            }
                        }
                    }
            }
        }
    }
}