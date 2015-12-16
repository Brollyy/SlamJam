using UnityEngine;
using System.Collections;

public class KeyboardControls : MonoBehaviour {

	public float[] sliders;
	private KeyCode[] upCodes;
	private KeyCode[] downCodes;
	private Wave wave;
	private const float rateOfChange = 0.05F;

	// Use this for initialization
	void Start () {
		upCodes = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };
		downCodes = new KeyCode[] { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F };
		wave = gameObject.GetComponent<Wave> ();
		sliders = new float[4];
		for (int i = 0; i < 4; ++i) {
			sliders [i] = 0.0F;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 4; ++i) {
			if (Input.GetKey (upCodes[i]) && sliders[i] < 1.0F) {
				sliders [i] += rateOfChange;
				wave.setVolume (i, sliders [i]);
			}	
			if (Input.GetKey (downCodes[i]) && sliders[i] > 0.0F) {
				sliders [i] -= rateOfChange;
				wave.setVolume (i, sliders [i]);
			}
		}
	}
}
