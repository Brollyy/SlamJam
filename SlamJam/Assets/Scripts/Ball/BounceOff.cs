using UnityEngine;
using System.Collections;

public class BounceOff : MonoBehaviour {

	private bool hit = false;

	void Update() {
		hit = false;
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (!hit && col.gameObject.name.StartsWith ("Wave area")) {
			col.gameObject.GetComponentInParent<InterpolatedTrack> ().Collide (gameObject);
			hit = true;
		}
	}
}
