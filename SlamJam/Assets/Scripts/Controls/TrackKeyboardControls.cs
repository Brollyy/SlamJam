using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TrackKeyboardControls : MonoBehaviour {

	private float[] sliders;
	public KeyCode[] upCodes = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };
	public KeyCode[] downCodes = new KeyCode[] { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F };
	private GameObject tracklist;
	private const float rateOfChange = 0.05F;
	private AudioSource[] sources;
	private int n;

	// Use this for initialization
	void Start () {
		tracklist = GameObject.FindGameObjectWithTag("Tracklist");
		sliders = new float[4];
		sources = tracklist.GetComponents<AudioSource> ();
		n = Mathf.Min (4, sources.Length);
		for (int i = 0; i < n; ++i) {
			sliders [i] = sources[i].volume;
		}
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < n; ++i) {
			if (Input.GetKey (upCodes[i]) && sliders[i] < 1.0F) {
				sliders [i] += rateOfChange;
				sources[i].volume = sliders [i];
			}	
			if (Input.GetKey (downCodes[i]) && sliders[i] >= 0.0F) {
				sliders [i] -= rateOfChange;
				sources[i].volume = (sliders [i] >= 0.0F ? sliders[i] : 0.0F);
			}
		}
	}
}
