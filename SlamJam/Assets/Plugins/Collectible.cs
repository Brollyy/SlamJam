using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

	private int state = 0;
	private GameObject player;
	private ParticleSystem part;
	private AudioSource source;
	public AudioClip[] soundEffects;
	public AnimationCurve respawnCurve = AnimationCurve.EaseInOut(0.0F, 0.0F, 1.0F, 1.0F);
	public AnimationCurve despawnCurve = AnimationCurve.EaseInOut(0.0F, 0.0F, 1.0F, 1.0F);
	public float despawnTime = 0.2F;
	public float respawnTime = 0.5F;
	private float time = 0.0F;
	public float bonusTime = 0.1F;
	private Vector3 scale;

	// Use this for initialization
	void Start () {
		source = gameObject.AddComponent<AudioSource> ();
		source.loop = false;
		player = GameObject.FindGameObjectWithTag ("Player");
		scale = gameObject.transform.localScale;
		part = gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (state == 1) {
			time += Time.fixedDeltaTime;
			gameObject.transform.localScale = Vector3.Lerp (scale, Vector3.zero, despawnCurve.Evaluate (time / despawnTime));
			if (time >= despawnTime) {
				state = 3;
				time = 0.0F;
			}
		}
		else if (state == 2) {
			time += Time.fixedDeltaTime;
			gameObject.transform.localScale = Vector3.Lerp (Vector3.zero, scale, respawnCurve.Evaluate (time / respawnTime));
			if (time >= respawnTime) {
				state = 0;
				time = 0.0F;
			}
		}
	}

	public void Remake() {
		if (state == 3 || state == 1) {
			gameObject.GetComponent<CircleCollider2D> ().enabled = true;
			time = 0.0F;
			state = 2;
		}
	}

	public void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject == player && !player.GetComponent<Rigidbody2D>().isKinematic && state == 0) {
			part.Play ();
			int streak = player.GetComponent<Streak> ().GetStreak ();
			player.GetComponent<Streak> ().UpStreak ();
			source.clip = soundEffects [Mathf.Clamp(streak, 0, soundEffects.Length-1)];
			source.Play ();
			GameObject.FindGameObjectWithTag ("Timer").GetComponent<Timer> ().ChangeTimer (-bonusTime * Mathf.Clamp(streak, 1, soundEffects.Length));
			gameObject.GetComponent<TimerBonus> ().Activate (-bonusTime * Mathf.Clamp (streak+1, 1, soundEffects.Length));
			state = 1;
		}
	}
}
