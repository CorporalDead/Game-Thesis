using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	internal AudioSource weaponFireClip;
	internal float damage;
	internal float rateOfFire;
	internal float recoilModifier;

	void Update(){
		Debug.Log("Parent: "+damage);
	}
	internal string Message(){
		return	"Parent: "+damage; 
	}
}
