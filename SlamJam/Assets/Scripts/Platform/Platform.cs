using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	private AudioSource aSource;
	public int trackIndex = 0;

	public Vector3 positionChange = Vector3.zero;
	public Vector3 rotationChange = Vector3.zero;
	public Vector3 scaleChange = new Vector3(1.0F, 1.0F, 1.0F);

	private Vector3[] start;
	private Vector3[] end;
	private Vector3[] transformation = new Vector3[3];
	private Vector3 fixedRotation;

	private Vector3[] smoothingVelocities = new Vector3[3] {Vector3.zero, Vector3.zero, Vector3.zero};
	public float smoothingTime = 0.05F;

	// Use this for initialization
	void Start () {
		GameObject track = GameObject.FindGameObjectWithTag ("Tracklist");
		if (track && trackIndex >= 0 && trackIndex < track.GetComponents<AudioSource>().Length) {
			aSource = track.GetComponents<AudioSource> () [trackIndex];
		}
			
		start = new Vector3[3] {
			gameObject.transform.localPosition,
			gameObject.transform.localEulerAngles,
			gameObject.transform.localScale
		};
		end = new Vector3[3] {
			start[0] + positionChange,
			start[1] + rotationChange,
			new Vector3(start[2].x * scaleChange.x, start[2].y * scaleChange.y, start[2].z * scaleChange.z)
		};
		fixedRotation = start [1];
	}
	
	// Update is called once per frame
	void Update () {
		if (aSource) {
			for (int i = 0; i < 3; ++i) {
				transformation [i] = Vector3.Lerp (start [i], end [i], aSource.volume);
			}
			gameObject.transform.localPosition = Vector3.SmoothDamp (gameObject.transform.localPosition,
				transformation [0], ref smoothingVelocities [0], smoothingTime);
			fixedRotation = Vector3.SmoothDamp (fixedRotation,
				transformation [1], ref smoothingVelocities [1], smoothingTime);
			gameObject.transform.localEulerAngles = fixedRotation;
			gameObject.transform.localScale = Vector3.SmoothDamp (gameObject.transform.localScale,
				transformation [2], ref smoothingVelocities [2], smoothingTime);
		}
	}
}
