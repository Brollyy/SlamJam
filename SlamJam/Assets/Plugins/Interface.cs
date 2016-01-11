using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void ShowEndInterface() {
		transform.FindChild ("ContinueText").gameObject.SetActive (true);
	}
}
