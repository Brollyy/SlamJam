using UnityEngine;
using System.Collections;

public class PassOnTriggerToParent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D other) {
		gameObject.transform.parent.GetComponent<CollideLight> ().OnTriggerStay2D (other);
	}

	void OnTriggerExit2D(Collider2D other) {
		gameObject.transform.parent.GetComponent<CollideLight> ().OnTriggerExit2D (other);
	}
}
