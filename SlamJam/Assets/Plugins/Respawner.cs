using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour {

	public KeyCode respawnButton = KeyCode.Space;
	private GameObject timer;

	// Use this for initialization
	void Start () {
		timer = GameObject.FindGameObjectWithTag ("Timer");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (respawnButton)) {
			Respawn ();
		}
	}

	public void Respawn() {
		timer.GetComponent<Timer> ().Restart ();
		Transform child = gameObject.GetComponentsInChildren<Transform>()[1];
		child.localPosition = Vector3.zero;
		child.localEulerAngles = Vector3.zero;
		child.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		child.gameObject.GetComponent<Rigidbody2D> ().angularVelocity = 0.0F;
	}
}
