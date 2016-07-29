using UnityEngine;
using System.Collections;

public class WeaponPickUp : MonoBehaviour {

	private GameObject Primary;
	private GameObject Secondary;
	private GameObject Melee;
	private GameObject Equipment;

	private Transform CurrentWeapon;

	private string WeaponName;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E)){
			RaycastHit hit;

			if(Physics.Raycast(transform.position,transform.forward,out hit,2f)){
				Transform[] ParentTrans=hit.collider.GetComponentsInParent<Transform>();
					foreach(Transform childTrans in ParentTrans){
						if(childTrans.gameObject.transform.parent==null){
							if(childTrans.tag=="PickUpWep"){
								Debug.Log(childTrans.name);
			
							CurrentWeapon=transform.FindChild(childTrans.name);
							CurrentWeapon.gameObject.SetActive(true);
						}
					}
				}
			}
		}
	}
}
