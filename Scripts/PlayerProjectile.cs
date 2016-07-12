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
		//	DismemberReference=col.gameObject.GetComponentInParent<Dismemberment>();
			foreach(Rigidbody JointRB in rb){
				if(JointRB.gameObject.transform.parent!=null)
					Debug.Log (JointRB.name);
				}
			Debug.Log(col.gameObject.name);
		//	Debug.Log("setAlive Value"+DismemberReference.getAlive());
		}

		Destroy(gameObject);
	}
}
