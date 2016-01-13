using UnityEngine;
using System.Collections;

public class Streak : MonoBehaviour {

	private int streak;
	public int streakMax;
	public float continueTime = 0.5F;
	private float time;

	// Use this for initialization
	void Start () {
		streak = 0;
		time = 0.0F;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (streak > 0) {
			time += Time.fixedDeltaTime;
			if (time >= continueTime) {
				streak = 0;
				time = 0.0F;
			}
		}
	}

	public int GetStreak() {
		return streak;
	}

	public void UpStreak() {
		if(streak < streakMax) streak++;
		time = 0.0F;
	}

	public void Restart() {
		streak = 0;
		time = 0.0F;
	}
}
