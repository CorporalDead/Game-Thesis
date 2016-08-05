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

	internal int InitialMagCap;

	internal int DeductedFromSum;
	void Start(){
		InitialMagCap=MagazineCapacity;
	}

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
		Debug.Log("Initial"+InitialMagCap);
		delayFireTime++;
	}
	public void Reload(){
		if(AmmoSum>0){
			if(MagazineCapacity<InitialMagCap){
				DeductedFromSum=InitialMagCap-MagazineCapacity;
				AmmoSum-=DeductedFromSum;

				MagazineCapacity=InitialMagCap;
		}
	}
}
	public void RecoilPattern(){
		
	}
	public void Aim(){
		
	}
}
