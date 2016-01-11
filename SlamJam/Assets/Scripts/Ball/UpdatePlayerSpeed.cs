using UnityEngine;
using System.Collections;

public class UpdatePlayerSpeed : MonoBehaviour {

	private Animator anim;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		rb = transform.parent.gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetFloat ("Speed", rb.velocity.magnitude);
	}
}
