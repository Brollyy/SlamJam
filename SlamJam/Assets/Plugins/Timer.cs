using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Timer : MonoBehaviour {

	public float animationTime = 1.5F;

	private float timer = 0.0F;
	private TimeSpan timeSpan;
	private bool active = false;

	// Use this for initialization
	void FixedUpdate () {
		if (active) {
			timer += Time.fixedDeltaTime;
			timeSpan = TimeSpan.FromSeconds (timer);
			gameObject.GetComponent<Text> ().text = string.Format ("{0:D2} : {1:D2} : {2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
		} 
	}

	public void Restart() {
		timer = 0.0F;
		timeSpan = TimeSpan.FromSeconds (timer);
		gameObject.GetComponent<Text> ().text = string.Format ("{0:D2} : {1:D2} : {2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
	}

	public TimeSpan GetTime() {
		return timeSpan;
	}

	public void StopTimer() {
		active = false;
	}

	public void StartTimer() {
		active = true;
	}

	public void ChangeTimer(float time) {
		timer += time;
		if (timer < 0.0F) timer = 0.0F;
		timeSpan = TimeSpan.FromSeconds (timer);
		gameObject.GetComponent<Text> ().text = string.Format ("{0:D2} : {1:D2} : {2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
	}
}
