using UnityEngine;
using System.Collections;

public class Dismemberment : MonoBehaviour {
	private bool isAlive;
	private int bodyHP;
	// Use this for initialization
	void Start () {
		isAlive=true;
		bodyHP=2;
	}
	
	// Update is called once per frame
	void Update () {
		Rigidbody[] rb=gameObject.GetComponentsInChildren<Rigidbody>();
			
			foreach(Rigidbody JointRB in rb){
				if(JointRB.gameObject.transform.parent!=null&&isAlive||getAlive()){
						JointRB.isKinematic=true;
			}else
				if(!isAlive||getAlive()){
					JointRB.isKinematic=false;
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha1))
			isAlive=false;
		if(Input.GetKeyDown(KeyCode.Alpha2))
			isAlive=true;

		Debug.Log("Value from model: isAlive "+isAlive+" getAlive"+getAlive());
	}
	public void setAlive(bool isAlive){
		this.isAlive=isAlive;
	}
	public void setBodyHP(int bodyHP){
		this.bodyHP=bodyHP;

		if(this.bodyHP<0)
			this.bodyHP=2;
	}
	public bool getAlive(){
		return isAlive;
	}
	public int getHP(){
		return bodyHP;
	}
}
