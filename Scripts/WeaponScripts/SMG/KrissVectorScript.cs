using UnityEngine;
using System.Collections;

public class KrissVectorScript : Primary {

	// Use this for initialization
	void Start () {
		InitialMagCap=30;

		MagazineCapacity=InitialMagCap;
		AmmoSum=300;
	}
	
	// Update is called once per frame
	void Update () {
		Shoot (5500,2,2,false);

		if(Input.GetKeyDown(KeyCode.R))
			Reload();

		Debug.Log("Magazine Capacity: "+MagazineCapacity+" Total Ammo:"+AmmoSum);
	}
}
