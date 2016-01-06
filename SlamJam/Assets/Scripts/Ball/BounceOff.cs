using UnityEngine;
using System.Collections;

public class BounceOff : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name.StartsWith ("Wave area")) {
			col.gameObject.GetComponentInParent<InterpolatedTrack> ().Collide (gameObject);
		}
	}
}
