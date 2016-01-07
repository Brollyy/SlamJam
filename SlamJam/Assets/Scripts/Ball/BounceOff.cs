using UnityEngine;
using System.Collections;

public class BounceOff : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name.StartsWith ("Booster collider")) {
			col.gameObject.GetComponentInParent<FixedCurvePlatform> ().Collide (gameObject);
		}
	}
}
