using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour,IFireArms {
	internal AudioSource weaponFireClip;
	internal float damage;
	internal float rateOfFire;
	internal float delayFireTime;
	internal float reloadTime;
	internal float recoilModifier;

	void Update(){

	}
	public void Shoot(float damage,float rateOfFire, float recoilModifier, bool isSemiAuto){
		if(isSemiAuto){
			if(delayFireTime>rateOfFire){
				if(Input.GetKeyDown(KeyCode.Mouse0)){
					Debug.Log("bang");

					delayFireTime=0;
				}
			}
		}
		delayFireTime++;
	}
	public void Reload(){
		
	}
	public void RecoilPattern(){
		
	}
	public void Aim(){
		
	}
}
