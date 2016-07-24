using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JanitorScript : MonoBehaviour {

	public int BloodSpriteCTR;

	private PlayerProjectile projectileReference;

	private List<GameObject> bloodSprites;
	// Use this for initialization
	void Start () {
		bloodSprites=new List<GameObject>();	
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
