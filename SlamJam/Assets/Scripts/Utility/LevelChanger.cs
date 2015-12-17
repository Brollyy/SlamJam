using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelChanger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			if(SceneManager.GetActiveScene().name != "level1") SceneManager.LoadScene ("level1");
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			if(SceneManager.GetActiveScene().name != "modeTest") SceneManager.LoadScene ("modeTest");
		}
	}
}
