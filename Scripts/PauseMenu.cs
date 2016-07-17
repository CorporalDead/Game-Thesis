using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	bool paused;

	// Use this for initialization
	void Start ()
	{
		paused = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (Time.timeScale >= 1) {
				paused = true;
				Time.timeScale = 0;
			} else {
				paused = false;
				Time.timeScale = 1;
			}
		}
	}

	void FixedUpdate ()
	{
		Debug.Log ("Played");
	}

	void OnGUI ()
	{
		if (paused) {
			GUI.Box (new Rect (Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400), "Pause");

			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2 - 75, 150, 50), "Resume")) {
				paused = false;
				Time.timeScale = 1;
			} else if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2, 150, 50), "Exit")) {
				Application.Quit ();
			}
		}
	}
}
