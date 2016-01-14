using UnityEngine;
using System.Collections;

public class HueChange : MonoBehaviour {
	public GameObject[] backgrounds;
	float nextFlash = 0.2F;
	public float flashRate = 0.5F;
	int i = 0;
	public float delay = 0.0F;
	public Color[] colors;
    public bool rave = true;
	
	// Use this for initialization
	void Start () {
		for (int b = 0; b < backgrounds.Length; b++) {
			backgrounds [b].GetComponentInChildren<SpriteRenderer> ().color = Color.LerpUnclamped (colors [i], Color.white, nextFlash / flashRate);
		}
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown ("1")) {
			rave = !rave;
		}

			if (Time.fixedTime < delay)
				return;

			if (nextFlash < flashRate) {
				nextFlash += Time.fixedDeltaTime;

			} else {
				nextFlash = 0;
				i++;
				if (i == colors.Length) {
					i = 0;
				}
				//i = j;
				//do {
				//	j = Random.Range (0, colors.Length - 1);
				//} while(j == i);
			}
		if (rave) {
			for (int b = 0; b < backgrounds.Length; b++) {	
				if (i < colors.Length - 1) {
					backgrounds [b].GetComponentInChildren<SpriteRenderer> ().color = Color.LerpUnclamped (colors [i], colors [i + 1], nextFlash / flashRate);

				} else {
					backgrounds [b].GetComponentInChildren<SpriteRenderer> ().color = Color.LerpUnclamped (colors [i], colors [0], nextFlash / flashRate);


				}
			}
		} 
		if(!rave){
			for (int b = 0; b < backgrounds.Length; b++) {
				backgrounds [b].GetComponentInChildren<SpriteRenderer> ().color = Color.white;
			}
		}
	}
}