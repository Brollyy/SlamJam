using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootBalls : MonoBehaviour {
	
	public Vector2 initialForce = new Vector2 (10.0F, 0.0F); 
	public Material materialOfBall;
	public Mesh meshOfBall;
	public PhysicsMaterial2D pMaterialOfBall;
	public float ballBounce = 15.0F;
	public float ballLifeTime = 5.0F;
	private List<GameObject> balls;
	private List<float> durations;

	// Use this for initialization
	void Start () {
		balls = new List<GameObject> ();
		durations = new List<float> ();
	}
	
	// Update is called once per frame
	void Update () {
		List<int> toDelete = new List<int> ();
		for (int i = 0; i < balls.Count; ++i) {
			if (Time.time - durations [i] > ballLifeTime) {
				toDelete.Add (i);
			}
		}
		for (int i = toDelete.Count - 1; i >= 0; --i) {
			GameObject.Destroy (balls [toDelete [i]]);
			balls.RemoveAt (toDelete [i]);
			durations.RemoveAt (toDelete [i]);
		}

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.Return)) {
			GameObject newBall = new GameObject("Ball" + balls.Count);
			newBall.transform.parent = gameObject.transform;
			newBall.transform.localPosition = Vector3.zero;
			newBall.transform.localScale = new Vector3 (1.0F, 1.0F, 1.0F);
			newBall.AddComponent<MeshRenderer> ().material = materialOfBall;
			newBall.AddComponent<MeshFilter> ().mesh = meshOfBall;
			newBall.AddComponent<CircleCollider2D> ().sharedMaterial = pMaterialOfBall;
			newBall.AddComponent<Rigidbody2D> ().AddForce (initialForce);
			newBall.AddComponent<BounceOff> ().bounce = ballBounce;
			balls.Add (newBall);
			durations.Add (Time.time);
		}
	}
}
