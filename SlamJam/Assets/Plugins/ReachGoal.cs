﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ReachGoal : MonoBehaviour {

	public string levelToLoad = "mainMenu";
	public KeyCode proceedButton = KeyCode.Space;
	public bool levelDone = false;

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
				Complete ();
			}
		}
	}

	public bool Complete() {
		if (levelDone) {
			SceneManager.LoadScene (levelToLoad);
		}
		return levelDone;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
			levelDone = true;
			col.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
			GameObject.FindGameObjectWithTag ("Timer").GetComponent<Timer> ().StopTimer();
			GameObject.FindGameObjectWithTag ("Canvas").GetComponent<Interface> ().ShowEndInterface ();
			for (int i = 0; i < transform.childCount; ++i) {
				Transform tf = transform.GetChild (i);
				if (tf.name.StartsWith ("Particle")) {
					tf.gameObject.SetActive (true);
				}
			}
			if (source) {
                source.volume = 0.5F;
				source.Play ();
			}
		}
	}
}
