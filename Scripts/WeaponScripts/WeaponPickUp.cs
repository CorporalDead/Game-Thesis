using UnityEngine;
using System.Collections;

public class WeaponPickUp : MonoBehaviour {
	DroppedWeaponScript	 DroppedWeaponScriptReference;

	private Transform Primary;
	private Transform Secondary;
	private Transform Melee;
	private Transform Equipment;

	private Transform CurrentWeapon;
	private Transform PreviousWewapon;

	private MonoBehaviour PickedUpWeaponInheritance;

	private string WeaponName;
	private bool isPickUp;

	// Update is called once per frame
	void Update () {

		WeaponSwapping();

		//Debug.Log(isPickUp);
	}


	//Pick Up by Raycast
//	void PickUp(){
//		RaycastHit hit;
//
//			if(Physics.Raycast(transform.position,transform.forward,out hit,2f)){
//				Transform[] ParentTrans=hit.collider.GetComponentsInParent<Transform>();
//					foreach(Transform childTrans in ParentTrans){
//						if(childTrans.gameObject.transform.parent==null){
//							if(childTrans.tag=="PickUpWep"){
//						
//								isPickUp=true;
//							
//								if(Input.GetKeyDown(KeyCode.E)){	
//								Transform PickedUpWeapon=transform.FindChild(childTrans.name);
//								PickedUpWeaponInheritance=PickedUpWeapon.GetComponent<MonoBehaviour>();
//								
//								if(PickedUpWeaponInheritance is Primary)
//									Primary=PickedUpWeapon;
//								if(PickedUpWeaponInheritance is Secondary)
//									Secondary=PickedUpWeapon;
//						}
//					}
//				}
//			}
//		}
//	}


	void WeaponSwapping(){
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			if(Primary!=null){
				Primary.gameObject.SetActive(true);

				if(Secondary!=null)
					Secondary.gameObject.SetActive(false);
//				Melee.gameObject.SetActive(false);
//				Equipment.gameObject.SetActive(false);
			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha2)){
			if(Secondary!=null){
				Secondary.gameObject.SetActive(true);

				if(Primary!=null)
					Primary.gameObject.SetActive(false);
//				Melee.gameObject.SetActive(false);
//				Equipment.gameObject.SetActive(false);
			}
		}
	}

	void OnCollisionEnter(Collision col){

		if(col.gameObject.tag=="PickUpWep"){
		Transform[] ParentTrans=col.gameObject.GetComponentsInParent<Transform>();
		foreach(Transform childTrans in ParentTrans){
			if(childTrans.gameObject.transform.parent==null){

					Debug.Log("Collided Weapon:"+childTrans.name);
					isPickUp=true;
					
					if(Input.GetKeyDown(KeyCode.E)){	
						Transform PickedUpWeapon=GameObject.Find("PlayerCamera").transform.FindChild(childTrans.name);
						PickedUpWeaponInheritance=PickedUpWeapon.GetComponent<MonoBehaviour>();

						if(PickedUpWeaponInheritance is Primary)
							Primary=PickedUpWeapon;
						if(PickedUpWeaponInheritance is Secondary)
							Secondary=PickedUpWeapon;

						Destroy(childTrans.gameObject);
				}
			}
		}
	}else
			isPickUp=false;
}
	void OnGUI(){
		if(isPickUp)
		GUI.Label(new Rect(Screen.width/2,Screen.height/2,200,30),"Pick Up?");
	}
}
