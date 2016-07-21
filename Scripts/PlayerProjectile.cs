﻿using UnityEngine;
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

	private GameObject blood;

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
							blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							Destroy(blood,3);
							Destroy(col.gameObject);
					}
					if(col.gameObject.name=="LeftClavicle"){
						if(!DismemberReference.getLeftForearmActive())
							LeftArm=Instantiate(ArmGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							else
								LeftShoulder=Instantiate(ShoulderGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							Destroy(blood,3);
							Destroy(col.gameObject);
					}

					if(col.gameObject.name=="RightForearm"){
							RightForearm=Instantiate(ForearmGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							DismemberReference.setRightForearmActive(true);
							blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							Destroy(blood,3);
							Destroy(col.gameObject);
					}
					if(col.gameObject.name=="RightClavicle"){
						if(!DismemberReference.getRightForearmActive())
							RightArm=Instantiate(ArmGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							else
								RightShoulder=Instantiate(ShoulderGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							Destroy(blood,3);
							Destroy(col.gameObject);
					}

					if(col.gameObject.name=="Chest"){
						if(DismemberReference.getLeftForearmActive()&&DismemberReference.getRightForearmActive()){
							Chest=Instantiate(ChestGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
								blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
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
							Destroy(blood,3);
							Destroy(col.gameObject);
					}
					if(col.gameObject.name=="RightLeg"){
							RightUpperLeg=Instantiate(UpperLegGib,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							blood=Instantiate(BloodParticleReference,col.gameObject.transform.position,col.gameObject.transform.rotation) as GameObject;
							Destroy(blood,3);
							Destroy(col.gameObject);
					}
				}
			}
		}
			RaycastHit bloodRay;
			
			Debug.Log("I love pkmns"+gameObject.name);
			
			Debug.DrawRay(transform.position,-col.transform.forward,Color.cyan);
			
			if(Physics.Raycast(transform.position,-col.transform.forward,out bloodRay,3f)){

				if(bloodRay.collider.tag=="BloodSurface"){
					Instantiate(BloodSplatter,bloodRay.point,Quaternion.FromToRotation(Vector3.up, bloodRay.normal));
					Debug.Log(bloodRay.normal);
				}
			}	
			Destroy(gameObject);
		}
	}
}
