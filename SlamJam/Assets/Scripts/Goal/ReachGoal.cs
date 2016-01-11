using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReachGoal : MonoBehaviour {

	public string levelToLoad = "mainMenu";
	public KeyCode proceedButton = KeyCode.Space;
	private bool levelDone = false;

	public AudioClip finishSound;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		if (finishSound) {
			source = gameObject.AddComponent<AudioSource> ();
			source.clip = finishSound;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (levelDone) {
			if (Input.GetKeyDown (proceedButton)) {
				SceneManager.LoadScene (levelToLoad);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
			levelDone = true;
			GameObject.FindGameObjectWithTag ("Timer").GetComponent<Timer> ().Stop();
			GameObject.FindGameObjectWithTag ("Canvas").GetComponent<Interface> ().ShowEndInterface ();
			for (int i = 0; i < transform.childCount; ++i) {
				Transform tf = transform.GetChild (i);
				if (tf.name.StartsWith ("Particle")) {
					tf.gameObject.SetActive (true);
				}
			}
			if (source) {
				source.Play ();
			}
		}
	}
}
