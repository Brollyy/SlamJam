using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerBonus : MonoBehaviour {

	public Transform prefabText;
	private GameObject canvas;
	private Transform text;
	private float bonus;
	private bool active;
	private float time;
	public float animationTime = 0.1F;
	private Vector2 start, end;
	private Vector2 startScale = new Vector2(1.0F, 1.0F);

	// Use this for initialization
	void Start () {
		canvas = GameObject.FindGameObjectWithTag ("Canvas");
		active = false;
		time = 0.0F;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (active) {
			time += Time.fixedDeltaTime;
			(text as RectTransform).position = Vector2.Lerp(start, end, time/animationTime);
			(text as RectTransform).localScale = Vector2.Lerp (startScale, Vector2.zero, time / animationTime);
			if (time >= animationTime) {
				active = false;
				if (text) Destroy (text.gameObject);
				time = 0.0F;
			}
		}
	}

	public void Activate(float bonus) {
		if (!active) {
			active = true;
			if (prefabText) {
				text = Instantiate (prefabText);
				text.SetParent(canvas.transform, false);
			}
			text.GetComponent<Text> ().text = string.Format("{0:F}", bonus);
			start = (text as RectTransform).position;
			RectTransform rect = (text as RectTransform);
			Vector2 pivot = rect.pivot;
			rect.pivot = new Vector2 (1.0F, 1.0F);
			Vector2 anchorMax = rect.anchorMax;
			rect.anchorMax = new Vector2 (1.0F, 1.0F);
			Vector2 anchorMin = rect.anchorMin;
			rect.anchorMin = new Vector2 (1.0F, 1.0F);
			end = rect.position;
			rect.pivot = pivot;
			rect.anchorMax = anchorMax;
			rect.anchorMin = anchorMin;

			int streak = GameObject.FindGameObjectWithTag ("Player").GetComponent<Streak> ().GetStreak ();
			int streakMax = GameObject.FindGameObjectWithTag ("Player").GetComponent<Streak> ().streakMax;
			startScale.Set (1 + 3*((streak - 1.0F) / streakMax), 1 + 3*((streak - 1.0F) / streakMax));
		}
	}
}
