using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	private AudioSource aSource;
	public float[] samples = new float[64];
	public AnimationCurve trackPriority = AnimationCurve.Linear(0.0F,1.0F,63.0F,1.0F);
	public int trackIndex = 0;

	public float amplitude = 50.0F;
	public Vector2 direction = new Vector2 (1.0F, 0.0F);
	private Vector2 movement = Vector2.zero;

	private Vector2 smoothingVelocity = Vector2.zero;
	public float smoothingTime = 0.05F;

	// Use this for initialization
	void Start () {
		GameObject track = GameObject.FindGameObjectWithTag ("Tracklist");
		if (track) {
			AudioSource[] aSources = track.GetComponents<AudioSource> ();
			if (trackIndex >= 0 && trackIndex < aSources.Length) {
				aSource = aSources [trackIndex];
			}
		}

		direction.Normalize ();
	}
	
	// Update is called once per frame
	void Update () {
		if (aSource) {
			aSource.GetSpectrumData (samples, 0, FFTWindow.BlackmanHarris);
			float average = 0.0F;
			for (int i = 0; i < 64; ++i) {
				average += samples [i] * trackPriority.Evaluate(i);
			}
			average /= 64.0F;

			movement = Vector2.SmoothDamp (movement, direction * average * amplitude, ref smoothingVelocity, smoothingTime);
			gameObject.transform.localPosition = movement;
		}
	}
}
