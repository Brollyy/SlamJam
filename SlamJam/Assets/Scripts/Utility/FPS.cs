using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<TextMesh> ().text = ((int)(1.0F / Time.deltaTime)).ToString();
	}
}
