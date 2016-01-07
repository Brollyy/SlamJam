using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Timer : MonoBehaviour {

	private float timer = 0.0F;
	private TimeSpan timeSpan;

	// Use this for initialization
	void FixedUpdate () {
		timer += Time.fixedDeltaTime;
		timeSpan = TimeSpan.FromSeconds (timer);
		gameObject.GetComponent<Text> ().text = string.Format ("{0:D2} : {1:D2} : {2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds/10);
	}

	public void Restart() {
		timer = 0.0F;
	}

	public TimeSpan GetTime() {
		return timeSpan;
	}
}
