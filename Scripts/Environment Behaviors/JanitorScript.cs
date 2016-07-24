using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JanitorScript : MonoBehaviour {

	public int GibletsCTR;
	public int BloodSpriteCTR;

	private PlayerProjectile projectileReference;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(BloodSpriteCTR>32){
			Destroy(GameObject.FindGameObjectWithTag("BloodSplatter"));
			BloodSpriteCTR--;
		}
		Debug.Log("# of Splats: "+BloodSpriteCTR);
	}
}
