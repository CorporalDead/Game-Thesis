using UnityEngine;
using System.Collections;

public class Dismemberment : MonoBehaviour {

	private bool isAlive;
	
	private bool leftForearmActive;
	private bool rightForearmActive;

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
				if(JointRB.gameObject.transform.parent!=null&&isAlive){
						JointRB.isKinematic=true;
			}else
				if(!isAlive){
					JointRB.isKinematic=false;
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha1))
			isAlive=false;
		if(Input.GetKeyDown(KeyCode.Alpha2))
			isAlive=true;

		if(leftForearmActive&&rightForearmActive)
			Debug.Log(":)");

		Debug.Log("LeftFore: "+leftForearmActive+"     RightFore: "+rightForearmActive);
	}
	public void setLeftForearmActive(bool leftForearmActive){
		this.leftForearmActive=leftForearmActive;
	}

	public void setRightForearmActive(bool rightForearmActive){
		this.rightForearmActive=rightForearmActive;
	}

	public void setAlive(bool isAlive){
		this.isAlive=isAlive;
	}
	public void setBodyHP(int bodyHP){
		this.bodyHP=bodyHP;

		if(this.bodyHP<0)
			this.bodyHP=2;
	}
	
	public bool getLeftForearmActive(){
		return leftForearmActive;
	}

	public bool getRightForearmActive(){
		return rightForearmActive;
	}

	public bool getAlive(){
		return isAlive;
	}
	public int getHP(){
		return bodyHP;
	}
}
