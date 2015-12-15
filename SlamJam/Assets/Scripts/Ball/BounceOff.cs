using UnityEngine;
using System.Collections;

public class BounceOff : MonoBehaviour {

	public float bounce = 15;

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name.StartsWith ("Wave area")) {
			Vector2 speed = (col.gameObject.GetComponent<SmoothingSpeedStorage> ().smoothingSpeed1 + col.gameObject.GetComponent<SmoothingSpeedStorage> ().smoothingSpeed2)/2;
			//Vector2 speed = col.relativeVelocity + gameObject.GetComponent<Rigidbody2D> ().velocity;
			if (speed.y > 0) {
				gameObject.GetComponent<Rigidbody2D> ().AddForce (bounce*speed);
			}
		}
	}
}
