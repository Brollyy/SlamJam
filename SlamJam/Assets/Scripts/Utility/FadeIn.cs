using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour {

	private SpriteRenderer spriteRend;
	private Color spriteStart, spriteEnd;
	private Text guiRend;
	public float time = 1.0F;
	private float t = 0.0F;
	public AnimationCurve curve = AnimationCurve.EaseInOut (0.0F, 0.0F, 1.0F, 1.0F);

	// Use this for initialization
	void Start () {
		spriteRend = gameObject.GetComponent<SpriteRenderer> ();
		spriteStart = Color.black;
		if (spriteRend) spriteEnd = spriteRend.color;
		guiRend = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (t < time) {
			t += Time.fixedDeltaTime;
			if (spriteRend) {
				spriteRend.color = Color.Lerp(spriteStart, spriteEnd, curve.Evaluate (t / time));
			}

			if (guiRend) { 
				Color c = guiRend.color;
				c.a = Mathf.Lerp(0, 1, curve.Evaluate (t / time));
				guiRend.color = c;
			}
		}
	}
}
