using UnityEngine;
using System.Collections;

public class WeaponBobbing : MonoBehaviour {
	MovementScript playerMovement;

	float MouseX;
	float MouseY;

	Quaternion rotationSpeed;
	// Use this for initialization
	void Start () {
		playerMovement=GameObject.Find("Player").GetComponent<MovementScript>();
	}
	
	// Update is called once per frame
	void Update () {

			MouseX=Input.GetAxis("Mouse X");
			MouseY=Input.GetAxis("Mouse Y");

		if(!playerMovement.isSprinting)
			rotationSpeed=Quaternion.Euler(-MouseY,-MouseX+90,0);
		else
			if(playerMovement.isSprinting)
				rotationSpeed=Quaternion.Euler(-MouseY,-MouseX,0);

		transform.localRotation=Quaternion.Slerp(transform.localRotation,rotationSpeed,3*Time.deltaTime);

	}
}
