using UnityEngine;
using System.Collections;

public class MaterialQueuePriority : MonoBehaviour {

	public int priority = 0;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer> ().material.renderQueue = priority;
	}
}
