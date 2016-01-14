using UnityEngine;
using System.Collections;

public class Fan : MonoBehaviour {

	private ParticleSystem part;
	public float forceMultiplier = 1.0F;

	// Use this for initialization
	void Start () {
		part = GameObject.Find("Particles").GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		part.startSpeed = transform.lossyScale.y;
		if (Mathf.Approximately (transform.lossyScale.y, 0.0F)) {
			part.Stop ();
		} else if(!part.isPlaying) {
			part.Play ();
		}
	}

	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.name == "Player" && !col.gameObject.GetComponent<Rigidbody2D>().isKinematic) {
			float dist = transform.InverseTransformDirection (col.transform.position - transform.position).y;
			Vector3 force = transform.up.normalized;
			print (force);
			force *= (transform.lossyScale.y - dist) * forceMultiplier;
			print (force);
			col.gameObject.GetComponent<Rigidbody2D> ().AddForce (force);
		}
	}
}
