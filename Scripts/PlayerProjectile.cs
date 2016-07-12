using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {
	Dismemberment DismemberReference;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag=="Enemy"){
			Rigidbody[] rb=col.gameObject.GetComponentsInParent<Rigidbody>();
			foreach(Rigidbody JointRB in rb){
				if(JointRB.gameObject.transform.parent!=null)
					if(JointRB.GetComponent<Dismemberment>()!=null){
						DismemberReference=col.gameObject.GetComponentInParent<Dismemberment>();

						DismemberReference.setAlive(false);
						DismemberReference.setBodyHP(DismemberReference.getHP()-1);
					}
				}
			if(DismemberReference.getHP()==0)
			Destroy(col.gameObject);
		}

		Destroy(gameObject);
	}
}
