using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	public Transform followObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (followObject) {
			transform.position = new Vector3 (followObject.position.x, followObject.position.y, -10.0F);
		}
	}
}
