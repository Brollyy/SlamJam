using UnityEngine;
using System.Collections;

public class HueChange : MonoBehaviour {
	public GameObject[] backgrounds;
	float nextFlash = 0.2F;
	public float flashRate = 0.5F;
	int i = 0;

	public Color[] colors;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (nextFlash < flashRate) {
			nextFlash += Time.fixedDeltaTime;

		} 
		else {
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
		for (int b = 0; b < backgrounds.Length; b++) {	
			if (i < colors.Length-1) {
				backgrounds [b].GetComponentInChildren<SpriteRenderer>().color = Color.LerpUnclamped (colors[i], colors [i+1], nextFlash / flashRate);

			}else{
				backgrounds [b].GetComponentInChildren<SpriteRenderer>().color = Color.LerpUnclamped (colors[i], colors [0], nextFlash / flashRate);


			}
		}
	}
}