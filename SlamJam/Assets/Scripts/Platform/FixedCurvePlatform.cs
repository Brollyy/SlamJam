using UnityEngine;
using System.Collections;

public class FixedCurvePlatform : MonoBehaviour {

	public Vector3 positionChange = Vector3.zero;
	public Vector3 rotationChange = Vector3.zero;
	public Vector3 scaleChange = Vector3.zero;

	public float time = 1.0F;
	public float startingTime = 0.0F;
	private float actualTime;
	public AnimationCurve positionCurve = AnimationCurve.Linear(0.0F, 0.0F, 1.0F, 1.0F);
	public AnimationCurve rotationCurve = AnimationCurve.Linear(0.0F, 0.0F, 1.0F, 1.0F);
	public AnimationCurve scaleCurve = AnimationCurve.Linear(0.0F, 0.0F, 1.0F, 1.0F);

	public float boostForce = 100.0F;

	private Vector3[] start;
	private Vector3[] end;

	// Use this for initialization
	void Start () {

		start = new Vector3[3] {
			gameObject.transform.localPosition,
			gameObject.transform.localEulerAngles,
			gameObject.transform.localScale
		};
		end = new Vector3[3] {
			start[0] + positionChange,
			start[1] + rotationChange,
			start[2] + scaleChange
		};
		actualTime = startingTime;
	}

	// Update is called once per frame
	void FixedUpdate () {
		gameObject.transform.localPosition = Vector3.Lerp (start [0], end [0], positionCurve.Evaluate (actualTime / time));
		gameObject.transform.localEulerAngles = Vector3.Lerp (start [1], end [1], rotationCurve.Evaluate (actualTime / time));
		gameObject.transform.localScale = Vector3.Lerp (start [2], end [2], scaleCurve.Evaluate (actualTime / time));
		actualTime += Time.fixedDeltaTime;
		if (actualTime > time)
			actualTime -= time;
	}

	public void Collide(GameObject go) {
		Vector2 force = gameObject.transform.TransformDirection (scaleChange);
		force *= boostForce;
		if (transform.parent && transform.parent.gameObject.GetComponent<Platform> ())
			force *= transform.parent.gameObject.GetComponent<Platform> ().Magnitude ();
		go.GetComponent<Rigidbody2D> ().AddForce (force);
	}
}
