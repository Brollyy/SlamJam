using UnityEngine;
using System.Collections;

public class BounceOff : MonoBehaviour {

	public float bounce = 15;

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name.StartsWith ("Wave area")) {
			Vector2 speed = (col.gameObject.GetComponent<SmoothingSpeedStorage> ().smoothingSpeed1 + col.gameObject.GetComponent<SmoothingSpeedStorage> ().smoothingSpeed2)/2;
			if (speed.y > 0) {
				Vector2 leftEdge = col.gameObject.GetComponent<EdgeCollider2D> ().points [2];
				Vector2 rightEdge = col.gameObject.GetComponent<EdgeCollider2D> ().points [3];
				Vector2 slope = rightEdge - leftEdge;
				slope.Set (-slope.y, slope.x);
				slope.Normalize ();
				speed.Set (slope.x * speed.y, slope.y * speed.y);
				gameObject.GetComponent<Rigidbody2D> ().AddForce (bounce * speed);
			}
		}
	}
}
