using UnityEngine;
using System.Collections;

public class USP45Script:Secondary{
	// Use this for initialization
	void Start(){

		InitialMagCap=12;

		MagazineCapacity=InitialMagCap;
		AmmoSum=120;
	}
	// Update is called once per frame
	void Update () {
		Shoot(3000,1,10,true); //Shoot(int MagazineCapacity, int ammocapacity,float force,float damage, float rateOfFire, bool isSemiAuto);
	//	WeaponReference.Reload(30,90); //Reload(int magazinecapacity, int ammocount);
		if(Input.GetKeyDown(KeyCode.R))
			Reload();

		Debug.Log("Magazine Capacity: "+MagazineCapacity+" Deducted"+DeductedFromSum+"Total Ammo:"+AmmoSum);
	}
}
