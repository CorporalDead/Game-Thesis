using UnityEngine;
using System.Collections;

public class KrissVectorScript : Primary {

	// Use this for initialization
	void Start () {
		MagazineCapacity=30;
		AmmoSum=300;
	}
	
	// Update is called once per frame
	void Update () {
		Shoot (5000,2,5,false);
	

		Debug.Log(""+MagazineCapacity);
	}
}
