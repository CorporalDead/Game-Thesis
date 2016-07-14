using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {
	public GameObject ForearmGib;
	public GameObject ShoulderGib;
	public GameObject ArmGib;

	public GameObject RightForearmGib;
	public GameObject RightShoulderGib;
	public GameObject RightArmGib;

	private GameObject LeftForearm;
	private GameObject LeftShoulder;
	private GameObject LeftArm;

	private GameObject RightForearm;
	private GameObject RightShoulder;
	private GameObject RightArm;

	private Dismemberment DismemberReference;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision col){
		//collision for enemies
		if(col.gameObject.tag=="Enemy"){
			Rigidbody[] rb=col.gameObject.GetComponentsInParent<Rigidbody>();
			foreach(Rigidbody JointRB in rb){
				if(JointRB.gameObject.transform.parent!=null)
					if(JointRB.GetComponent<Dismemberment>()!=null){
						DismemberReference=col.gameObject.GetComponentInParent<Dismemberment>();

						DismemberReference.setAlive(false);
						DismemberReference.setBodyHP(DismemberReference.getHP()-1);

				
				if(DismemberReference.getHP()==0){
					if(col.gameObject.name=="LeftForearm"){
						LeftForearm=Instantiate(ForearmGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
						DismemberReference.setLeftForearmActive(true);
					}
					if(col.gameObject.name=="LeftClavicle"){
						if(DismemberReference.getLeftForearmActive()==false)
							LeftArm=Instantiate(ArmGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
					}

					if(col.gameObject.name=="RightForearm"){
							RightForearm=Instantiate(ForearmGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;		
					}
					if(col.gameObject.name=="RightClavicle"){	
							RightArm=Instantiate(ArmGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
						}
					
					Destroy(col.gameObject);
				}
			}
		}
	}

		//Destroy(gameObject);
	}
}
