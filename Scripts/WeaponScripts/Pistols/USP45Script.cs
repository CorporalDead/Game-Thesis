using UnityEngine;
using System.Collections;

public class USP45Script:Weapon,IFireArms {
	Weapon WeaponReference;
	IFireArms test;
	// Use this for initialization
	void Start(){
		WeaponReference = new Weapon();
		test = new USP45Script();
	}
	// Update is called once per frame
	void Update () {
		Debug.Log("Child:"+WeaponReference.damage+"Daddy:"+base.damage);
		Shoot();
	}
	public void Shoot(){
		WeaponReference.damage=5f;	
	}
	public void Reload(){
		Debug.Log("FUCK MEEEE; )))");
	}
	public void RecoilPattern(){
	
	}
}
