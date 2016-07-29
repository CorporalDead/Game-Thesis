using UnityEngine;
using System.Collections;

public class USP45Script:Weapon {
	Weapon WeaponReference;
	// Use this for initialization
	void Start(){
		WeaponReference = new Weapon();

	}
	// Update is called once per frame
	void Update () {
		WeaponReference.Shoot(1,30,1,true); //Shoot(float damage, float rateOfFire, float recoilmodifier, bool isSemiAuto);
	}
}
