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
			(text as RectTransform).anchoredPosition = Vector2.Lerp(start, end, time/animationTime);
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
			start = (text as RectTransform).anchoredPosition;
			end = new Vector3 (start.x, start.y - 10);
		}
	}
}
