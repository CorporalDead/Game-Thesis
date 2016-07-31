using UnityEngine;
using System.Collections;

public class KrissVectorScript : Primary {
	Weapon WeaponReference;
	// Use this for initialization
	void Start () {
		WeaponReference=new Primary();
	}
	
	// Update is called once per frame
	void Update () {
		WeaponReference.Shoot (5000,2,5,false);
	}
}
