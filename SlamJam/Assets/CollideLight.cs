using UnityEngine;
using System.Collections;

public class CollideLight : MonoBehaviour {
	float nextFlash = 0.0F;
	float flashRate = 0.5F;
	private MeshRenderer[] platforms;
	public Color[] colors;
	Color[] baseColor;
	private bool flashing = false;

	// Use this for initialization
	void Start () {
		platforms = gameObject.GetComponentsInChildren<MeshRenderer> ();
		for (int i = 0; i < platforms.Length; i++) {
			baseColor[i] = platforms [i].material.color;
		}
	}

	public void OnTriggerStay2D(Collider2D other){
		if (other.transform.gameObject.name == "Player") {
			flashing = true;
		}
	}

	public void OnTriggerExit2D(Collider2D other){
		if (other.transform.gameObject.name == "Player") {
			flashing = false;
		}
	}

	void FixedUpdate(){
		if(flashing){
			if (nextFlash < flashRate) {
				nextFlash += Time.fixedDeltaTime;
			} else {
				nextFlash = 0.0F;
			//i = j;
			//do {
			//	j = Random.Range (0, colors.Length - 1);
			//} while(j == i);
			}
			for (int i = 0; i < platforms.Length; i++) {
				platforms [i].material.color = Color.LerpUnclamped (colors [0], colors [1], nextFlash / flashRate);
				platforms [i].material.color = Color.LerpUnclamped (colors [1], colors [0], nextFlash / flashRate);
			}
		}
		else {
			for (int i = 0; i < platforms.Length; i++) {
				platforms[i].material.color = baseColor[i];
			}
		}
	}
			
		
	// Update is called once per frame

}
