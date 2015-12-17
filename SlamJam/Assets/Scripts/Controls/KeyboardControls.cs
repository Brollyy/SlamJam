using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KeyboardControls : MonoBehaviour {

	public float[] sliders;
	private KeyCode[] upCodes;
	private KeyCode[] downCodes;
	private Wave wave;
	private const float rateOfChange = 0.05F;
	private List<GameObject> UIsliders;

	// Use this for initialization
	void Start () {
		UIsliders = new List<GameObject>();
		upCodes = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };
		downCodes = new KeyCode[] { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F };
		wave = gameObject.GetComponent<Wave> ();
		sliders = new float[4];
		for (int i = 0; i < 4; ++i) {
			sliders [i] = 0.0F;
			UIsliders.Add(GameObject.FindGameObjectWithTag ("Slider"+i)) ;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 4; ++i) {
			if (Input.GetKey (upCodes[i]) && sliders[i] < 1.0F) {
				sliders [i] += rateOfChange;
				wave.setVolume (i, sliders [i]);
				UIsliders [i].GetComponent<Slider>().normalizedValue = UIsliders[i].GetComponent<Slider>().normalizedValue+rateOfChange;
			}	
			if (Input.GetKey (downCodes[i]) && sliders[i] >= 0.0F) {
				sliders [i] -= rateOfChange;
				wave.setVolume (i, (sliders [i] >= 0.0F ? sliders[i] : 0.0F));
				UIsliders [i].GetComponent<Slider>().normalizedValue = UIsliders[i].GetComponent<Slider>().normalizedValue-rateOfChange;

			}
		}
	}
}
