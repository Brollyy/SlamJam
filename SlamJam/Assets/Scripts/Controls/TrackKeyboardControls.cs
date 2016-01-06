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
	private List<GameObject> UIsliders;

	// Use this for initialization
	void Start () {
		UIsliders = new List<GameObject>();
		tracklist = GameObject.FindGameObjectWithTag("Tracklist");
		sliders = new float[4];
		for (int i = 0; i < 4; ++i) {
			sliders [i] = tracklist.GetComponents<AudioSource>()[i].volume;
			UIsliders.Add(GameObject.FindGameObjectWithTag ("Slider"+i)) ;
			UIsliders [i].GetComponent<Slider> ().normalizedValue = sliders [i];
		}
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 4; ++i) {
			if (Input.GetKey (upCodes[i]) && sliders[i] < 1.0F) {
				sliders [i] += rateOfChange;
				tracklist.GetComponents<AudioSource>()[i].volume = sliders [i];
				UIsliders [i].GetComponent<Slider>().normalizedValue = sliders[i];
			}	
			if (Input.GetKey (downCodes[i]) && sliders[i] >= 0.0F) {
				sliders [i] -= rateOfChange;
				tracklist.GetComponents<AudioSource>()[i].volume = (sliders [i] >= 0.0F ? sliders[i] : 0.0F);
				UIsliders [i].GetComponent<Slider>().normalizedValue = sliders[i];
			}
		}
	}
}
