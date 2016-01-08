using UnityEngine;
using System.Collections;

public class HueChange : MonoBehaviour {
	public GameObject background;
	float nextFlash = 0.0F;
	public float flashRate = 0.5F;
	int i = 0, j = 0;

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
			i = j;
			do {
				j = Random.Range (0, colors.Length - 1);
			} while(j == i);
		}
			
		background.GetComponent<SpriteRenderer> ().color = Color.LerpUnclamped (colors [i], colors [j], nextFlash/flashRate);

	}
}