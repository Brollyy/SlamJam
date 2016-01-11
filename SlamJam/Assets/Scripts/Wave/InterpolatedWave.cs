using UnityEngine;
using System.Collections;

public class InterpolatedWave : MonoBehaviour
{
	//An AudioSource object so the music can be played
	private AudioSource[] aSources = new AudioSource[4];
	public AudioClip[] tracks = new AudioClip[4];
	public AnimationCurve[] tracksPriority = new AnimationCurve[4]
	{
		AnimationCurve.Linear(0.0F, 1.0F, 64.0F, 1.0F), 
		AnimationCurve.Linear(0.0F, 1.0F, 64.0F, 1.0F), 
		AnimationCurve.Linear(0.0F, 1.0F, 64.0F, 1.0F), 
		AnimationCurve.Linear(0.0F, 1.0F, 64.0F, 1.0F)
	};
	public bool[] enabledTracks = new bool[4];
	//A float array that stores the audio samples
	public float[] samples = new float[64];
	//A renderer that will draw a line at the screen
	private LineRenderer lRenderer;
	//The transform attached to this game object
	private Transform goTransform;
	private Vector2 goTransform2D;
	//The position of the current point. Will also be the position of each point of the line.
	private Vector2 pointPos;
	private Vector2 targetPointPos;
	//An array that stores the Transforms of all instantiated points
	private Vector2[] pointsTransform;
	private Vector2[] relativePoints;
	public int length = 128;

	private Vector2[] interpolationPoints;
	private float cutOut = Mathf.Exp (-Mathf.Exp(1.0F));
	private float expScale;
	public float inBetweenRatio = 0.5F;
	private float[] A = new float[8];
	private float[] B = new float[8];
	private float[] C = new float[8];

	private GameObject[] areas;
	private Vector2[] areaVertices;
	private ColliderToMesh[] meshGenerators;

	private Vector2[] smoothingVelocities;
	public float smoothingTime = 0.05F;
	public float amplitude = 50.0F;
	private float[] volume;

	void Awake ()
	{
		//Get and store a reference to the following attached components:
		//LineRenderer
		this.lRenderer = GetComponent<LineRenderer>();
		//Transform
		this.goTransform = GetComponent<Transform>();
	}

	void Start()
	{
		expScale = 64.0F * Mathf.Exp (1.0F) / (length * length);
		goTransform2D = new Vector2(goTransform.position.x, goTransform.position.y);
		//The line should have the same number of points as the number of samples
		lRenderer.SetVertexCount(length);
		//The pointsTransform array should be initialized with the same length as the samples array
		pointsTransform = new Vector2[length];
		relativePoints = new Vector2[length];

		interpolationPoints = new Vector2[9];
		for (int i = 0; i < 9; ++i) {
			interpolationPoints [i] = new Vector2 (0.125F*i*length, 0.0F);
		}

		areas = new GameObject[length-1];
		areaVertices = new Vector2[4];
		areaVertices [0] = new Vector2 ();
		areaVertices [1] = new Vector2 ();
		meshGenerators = new ColliderToMesh[length - 1];
		smoothingVelocities = new Vector2[length];
		volume = new float[4] {0.0F, 0.0F, 0.0F, 0.0F};

		for (int i = 0; i < 4; ++i) {
			if (enabledTracks [i]) {
				aSources [i] = gameObject.AddComponent<AudioSource> ();
				aSources [i].clip = tracks [i];
				aSources [i].loop = true;
				aSources [i].volume = volume[i];
				aSources [i].Play ();
			}
		}

		//For each sample
		for(int i=0; i<length;i++)
		{
			//Get the recently instantiated point Transform component
			smoothingVelocities[i] = Vector2.zero;
			relativePoints [i] = new Vector2 (i * goTransform.lossyScale.x, 0);
			pointsTransform [i] = goTransform2D + relativePoints [i];

		}
		for (int i = 0; i < length-1; ++i) {
			areas [i] = new GameObject ("Wave area " + i);
			areas [i].transform.parent = goTransform;
			areas[i].AddComponent<EdgeCollider2D> ();
			areaVertices [0].Set (pointsTransform[i].x, goTransform2D.y);
			areaVertices [1].Set (pointsTransform[i+1].x, goTransform2D.y);
			areaVertices [2] = pointsTransform [i];
			areaVertices [3] = pointsTransform [i + 1];
			areas [i].GetComponent<EdgeCollider2D> ().points = areaVertices;
			areas [i].AddComponent<MeshRenderer> ().material = lRenderer.material;
			areas [i].AddComponent<MeshFilter> ();
			areas [i].AddComponent<Rigidbody2D> ().isKinematic = true;
			areas [i].AddComponent<SmoothingSpeedStorage> ();
			meshGenerators [i] = new ColliderToMesh (areas [i]);
		}
	}

	void Update ()
	{
		for (int i = 0; i < 4; ++i) {
			float samplesSum = 0.0F;
			if (enabledTracks [i]) {
				aSources[i].GetSpectrumData (samples, 0, FFTWindow.BlackmanHarris);
				for (int j = 0; j < 64; ++j) {
					samplesSum += samples [j] * tracksPriority [i].Evaluate (j);
				}
				interpolationPoints [2*i + 1].y = samplesSum * amplitude;
			}
		}

		for (int i = 1; i < 4; ++i) {
			interpolationPoints [2 * i].y = inBetweenRatio * Mathf.Min (interpolationPoints [2 * i - 1].y, interpolationPoints [2 * i + 1].y);
		}

		for (int j = 0; j < 8; ++j) {
			A [j] = Mathf.Abs (interpolationPoints [j + 1].y - interpolationPoints [j].y);
			B [j] = interpolationPoints [j + ((j % 2 == 0) ? 1 : 0)].x;
			C [j] = interpolationPoints [j + ((j % 2 == 0) ? 0 : 1)].y;
		}

		//For each sample
		for(int i=0; i<length;i++)
		{
			int j = 8 * i / length;
			float newValue =  A[j]* (Mathf.Exp (expScale * (i - B[j]) * (B[j] - i)) - cutOut) + C[j];
			targetPointPos = relativePoints [i];
			targetPointPos.y = goTransform.lossyScale.y * newValue;
			/*if (smoothingVelocities [i].y * targetPointPos.y < 0.0F) {
				smoothingVelocities [i].y = 0.0F;
			}*/
			smoothingVelocities [i].y *= 0.01F;
			pointPos = Vector2.SmoothDamp(relativePoints[i], targetPointPos, ref smoothingVelocities[i], smoothingTime);
			//Set the point to the new Y position
			relativePoints[i] = pointPos;
			pointsTransform[i] = relativePoints [i] + goTransform2D;

			/*Set the position of each vertex of the line based on the point position.
			 * Since this method only takes absolute World space positions, it has
			 * been subtracted by the current game object position.*/
			lRenderer.SetPosition(i, new Vector3(pointsTransform[i].x, pointsTransform[i].y, 0));
		}
		for (int i = 0; i < length-1; ++i) {
			areaVertices [0].Set (pointsTransform[i].x, goTransform2D.y);
			areaVertices [1].Set (pointsTransform[i+1].x, goTransform2D.y);
			areaVertices [2] = pointsTransform [i];
			areaVertices [3] = pointsTransform [i + 1];
			/*areas [i].GetComponent<PolygonCollider2D> ().points[2].y = pointsTransform[i].y;
			areas [i].GetComponent<PolygonCollider2D> ().points[3].y = pointsTransform [i+1].y;*/
			areas [i].GetComponent<EdgeCollider2D> ().points = areaVertices;
			meshGenerators [i].GetMesh ();
			areas [i].GetComponent<SmoothingSpeedStorage> ().smoothingSpeed1 = smoothingVelocities [i];
			areas [i].GetComponent<SmoothingSpeedStorage> ().smoothingSpeed2 = smoothingVelocities [i + 1];
		}
	}

	public void setVolume(int track, float volume) {
		if (track >= 0 && track < 4) {
			if (volume >= 0.0F && volume <= 1.0F) {
				this.volume [track] = volume;
				if (enabledTracks [track]) {
					aSources [track].volume = volume;
				}
			}
		}
	}

	public void ToggleTrack(int track) {
		if (track >= 0 && track < 4) {
			enabledTracks [track] = !enabledTracks [track];
			if (enabledTracks [track])
				aSources [track].Play ();
			else
				aSources [track].Stop ();
		}
	}
}