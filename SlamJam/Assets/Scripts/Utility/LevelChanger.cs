using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelChanger : MonoBehaviour {

	public KeyCode[] keys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };
	private Scene[] maps;
	private int n;

	// Use this for initialization
	void Start () {
		maps = SceneManager.GetAllScenes ();
		n = Mathf.Min (keys.Length, maps.Length);
	}
	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < n; ++i) {
			if (Input.GetKeyDown (keys[i])) {
				if(SceneManager.GetActiveScene().name != maps[i].name) SceneManager.LoadScene (maps[i].name);
			}
		}

	}
}
