using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour,IFireArms {
	public GameObject Projectiles;

	private GameObject PlayerProjectile;

	internal AudioSource weaponFireClip;
	internal bool isEmpty;
	internal float damage;
	internal float rateOfFire;
	internal float delayFireTime;
	internal float reloadTime;
	internal float recoilModifier;
	internal int MagazineCapacity;
	internal int AmmoSum;

	public void Shoot(float force,float damage,float rateOfFire, bool isSemiAuto){
		if(isSemiAuto&&MagazineCapacity!=0){
			if(delayFireTime>rateOfFire){
				if(Input.GetKeyDown(KeyCode.Mouse0)){
					PlayerProjectile=Instantiate(Resources.Load("Projectile"),GameObject.Find("Barrel").transform.position,Quaternion.identity) as GameObject;
					PlayerProjectile.GetComponent<Rigidbody>().AddForce(GameObject.Find("Barrel").transform.forward*force);

					delayFireTime=0;
					MagazineCapacity--;
				}
			}
		}else
			if(!isSemiAuto&&MagazineCapacity!=0){
				if(delayFireTime>rateOfFire){
					if(Input.GetKey(KeyCode.Mouse0)){
						PlayerProjectile=Instantiate(Resources.Load("Projectile"),GameObject.Find("Barrel").transform.position,Quaternion.identity) as GameObject;
						PlayerProjectile.GetComponent<Rigidbody>().AddForce(GameObject.Find("Barrel").transform.forward*force);
					
						delayFireTime=0;
						MagazineCapacity--;
				}
			}
		}
		delayFireTime++;
	}
	public void Reload(){
		if(MagazineCapacity==0)
			isEmpty=true;
	}
	public void RecoilPattern(){
		
	}
	public void Aim(){
		
	}
}
