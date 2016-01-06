using UnityEngine;
using System.Collections;

public class BallControls : MonoBehaviour {

	public KeyCode[] controls = { KeyCode.LeftArrow, KeyCode.RightArrow };
	public float acceleration = 1.0F;
	public float maxSpeed = 5.0F;
	private Rigidbody2D rb;
	private Vector2[] forces;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
		forces = new Vector2[] { new Vector2(-rb.mass * acceleration, 0.0F), new Vector2(rb.mass * acceleration, 0.0F) };
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		for (int i = 0; i < 2; ++i) {
			if (Input.GetKey (controls [i]) 
				&& rb.velocity.magnitude < maxSpeed) {

				rb.AddForce (forces [i]);
			} 
		}
	}
}
