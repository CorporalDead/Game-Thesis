using UnityEngine;
using System.Collections;

public class USP45Script:Secondary{
	Weapon WeaponReference;
	// Use this for initialization
	void Start(){
		WeaponReference = new Secondary();

	}
	// Update is called once per frame
	void Update () {
		WeaponReference.Shoot(3000,1,10,true); //Shoot(float force,float damage, float rateOfFire, bool isSemiAuto);
	
	}
}
