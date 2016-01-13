using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour {

	public KeyCode respawnButton = KeyCode.Space;
	public float respawnTime = 1.0F;
	public AnimationCurve curve = AnimationCurve.EaseInOut(0.0F, 0.0F, 1.0F, 1.0F);
	private GameObject timer;
	private Transform player;
	private Animator anim;
	private Vector3 pos = Vector3.zero;
	private Vector3 rot = Vector3.zero;
	private float time = 0.0F;
	private int state = 2;

	// Use this for initialization
	void Start () {
		timer = GameObject.FindGameObjectWithTag ("Timer");
		player = gameObject.GetComponentsInChildren<Transform> () [1];
		anim = player.GetChild (0).gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (state != 1 && Input.GetKeyDown (respawnButton)) {
			Respawn ();
		}
	}

	void FixedUpdate() {

		if (state == 1) {
			time += Time.fixedDeltaTime;
			player.localPosition = Vector3.Lerp(pos, Vector3.zero, curve.Evaluate(time/respawnTime));
			player.localEulerAngles = Vector3.Lerp (rot, Vector3.zero, curve.Evaluate (time / respawnTime));

			if (time >= respawnTime) {
				time = 0.0F;
				anim.SetBool ("Active", true);
				state = 2;
			}
		}
	}

	public void Respawn() {
		if (state == 0) {
			timer.GetComponent<Timer> ().Restart ();
			timer.GetComponent<Timer> ().StopTimer ();
			pos = player.localPosition;
			rot = player.localEulerAngles;
			player.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
			anim.SetBool ("Active", false);
			GameObject[] collectibles = GameObject.FindGameObjectsWithTag ("Collectible");
			foreach (GameObject go in collectibles) {
				if(go.GetComponent<Collectible>())
				go.GetComponent<Collectible> ().Remake ();
			}
			player.GetComponent<Streak> ().Restart ();
			state = 1;
		} 
		else if (state == 2) {
			player.gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;
			timer.GetComponent<Timer> ().StartTimer ();
			state = 0;
		}
	}
}
