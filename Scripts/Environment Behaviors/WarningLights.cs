using UnityEngine;
using System.Collections;

public class WarningLights : MonoBehaviour {

	Light CurrentLightSource;
	// Use this for initialization
	void Start () {
		CurrentLightSource=GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {

		//script taken from Unity Docs
		float phi = Time.time / 1 * 2 * Mathf.PI;
		float amplitude = Mathf.Cos(phi) * 1 + 1;
		CurrentLightSource.intensity=amplitude;
	}
}
