using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {
	public GameObject ForearmGib;
	public GameObject ShoulderGib;
	public GameObject ArmGib;
	public GameObject ChestGib;
	public GameObject LowerLegGib;
	public GameObject UpperLegGib;

	public GameObject BloodSplatter;
	public GameObject BloodParticleReference;
	public GameObject HeadShotReference;

	private GameObject blood;
	private GameObject HeadshotSplatter;
	private GameObject bloodSplatter;

	private GameObject LeftForearm;
	private GameObject LeftShoulder;
	private GameObject LeftArm;
	private GameObject LeftUpperLeg;
	private GameObject LeftLowerLeg;


	private GameObject RightForearm;
	private GameObject RightShoulder;
	private GameObject RightArm;
	private GameObject RightUpperLeg;
	private GameObject RightLowerLeg;

	private GameObject Chest;

	private bool isHit;
	private float BloodSplatterScale;

	private Dismemberment DismemberReference;
	private JanitorScript CleanUpReference;
	// Use this for initialization
	void Start () {
		CleanUpReference=GameObject.Find("ObjectCleaner").GetComponent<JanitorScript>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision col){
		//collision for enemies
		if(col.gameObject.tag=="Enemy"){
			//variable that checks for rigidbodies for the collided objects
			//we check the parents instead of the child in order to get a script so we can reference and interact with it.
			Rigidbody[] rb=col.gameObject.GetComponentsInParent<Rigidbody>();
			foreach(Rigidbody JointRB in rb){
				if(JointRB.gameObject.transform.parent!=null){
					if(JointRB.GetComponent<Dismemberment>()!=null){
						DismemberReference=col.gameObject.GetComponentInParent<Dismemberment>();

						//disable kinematics once the character dies
						DismemberReference.setAlive(false);
						//only dismember when the limbs have been shot multiple times
						DismemberReference.setBodyHP(DismemberReference.getHP()-1);
					
				
				if(DismemberReference.getHP()==0){
					if(col.gameObject.name=="LeftForearm"){
							
							LeftForearm=Instantiate(ForearmGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							DismemberReference.setLeftForearmActive(true);
							blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;

							Destroy(LeftForearm,5);
							Destroy(blood,3);
							Destroy(col.gameObject);
					}
					if(col.gameObject.name=="LeftClavicle"){
						if(!DismemberReference.getLeftForearmActive())
							LeftArm=Instantiate(ArmGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							else
								LeftShoulder=Instantiate(ShoulderGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;

							Destroy(LeftArm,5);
							Destroy(blood,3);
							Destroy(col.gameObject);
					}

					if(col.gameObject.name=="RightForearm"){
							RightForearm=Instantiate(ForearmGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							DismemberReference.setRightForearmActive(true);

							blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;

							Destroy(RightForearm,5);
							Destroy(blood,3);
							Destroy(col.gameObject);
					}
					if(col.gameObject.name=="RightClavicle"){
							//remove the entire arm when rightforearm is still intact
						if(!DismemberReference.getRightForearmActive())
							RightArm=Instantiate(ArmGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							else //remove clavicle/shoulder if the rightforearm is not intact
								RightShoulder=Instantiate(ShoulderGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;

							blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;

							Destroy(RightArm,5);
							Destroy(blood,3);
							Destroy(col.gameObject);
					}

					if(col.gameObject.name=="Chest"){
						if(DismemberReference.getLeftForearmActive()&&DismemberReference.getRightForearmActive()){
							Chest=Instantiate(ChestGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
								blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;

								Destroy(Chest,5);
								Destroy(blood,3);
								Destroy(col.gameObject);
						}
					}
					if(col.gameObject.name=="Head"){
						//add blood splatter for headshots
							
							Destroy(col.gameObject);
					}

					if(col.gameObject.name=="LeftLeg"){
							LeftUpperLeg=Instantiate(UpperLegGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;

							Destroy(LeftUpperLeg,5);
							Destroy(blood,3);
							Destroy(col.gameObject);
					}
					if(col.gameObject.name=="RightLeg"){
							RightUpperLeg=Instantiate(UpperLegGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;

							Destroy(RightUpperLeg,5);
							Destroy(blood,3);
							Destroy(col.gameObject);
					}
				}
			}
		}
	}
			//raycasting for blood sprites
			RaycastHit bloodRay;
			//cast a ray on the backside of an object
			if(Physics.Raycast(transform.position,-col.transform.forward,out bloodRay,2f)){

				if(bloodRay.collider.tag=="BloodSurface"){
					BloodSplatterScale=Random.Range(0.05f,0.2f);
					bloodSplatter=Instantiate(BloodSplatter,bloodRay.point,Quaternion.FromToRotation(Vector3.up, bloodRay.normal)) as GameObject;
					//change scale of instantiated bloodsplatters in order to add more variety
					bloodSplatter.transform.localScale=new Vector3(BloodSplatterScale,BloodSplatterScale,BloodSplatterScale);
					//increment janitor script in order to delete the blood sprites once it reaches a certain number (varies depending on player settings)
					CleanUpReference.BloodSpriteCTR++;
				}
			}
		}
		Destroy(gameObject);
	}
}
