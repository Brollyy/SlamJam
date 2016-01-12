using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour {

	public KeyCode respawnButton = KeyCode.Space;
	public float respawnTime = 1.0F;
	private GameObject timer;
	private Transform player;
	private Animator anim;
	private Vector3 pos = Vector3.zero;
	private Vector3 smoothPos = Vector3.zero;
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
			pos = Vector3.SmoothDamp (pos, Vector3.zero, ref smoothPos, respawnTime);
			player.localPosition = pos;

			if (pos.magnitude < 0.01F) {
				player.localEulerAngles = Vector3.zero;
				anim.SetBool ("Active", true);
				state = 2;
			}
		}
	}

	public void Respawn() {
		if (state == 0) {
			timer.GetComponent<Timer> ().Restart ();
			pos = player.localPosition;
			player.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
			anim.SetBool ("Active", false);
			state = 1;
		} 
		else if (state == 2) {
			player.gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;
			state = 0;
		}
	}
}
