using UnityEngine;
using System.Collections;

public class InterpolatedTrack : MonoBehaviour
{
	private GameObject tracklist;
	public int trackIndex = 0;
	private AudioSource aSource;
	//An AudioSource object so the music can be played
	public AnimationCurve trackPriority = AnimationCurve.Linear(0.0F, 1.0F, 64.0F, 1.0F);
	//A float array that stores the audio samples
	public float[] samples = new float[64];
	//A renderer that will draw a line at the screen
	private LineRenderer lRenderer;
	//The transform attached to this game object
	private Transform goTransform;
	//The position of the current point. Will also be the position of each point of the line.
	private Vector2 targetPointPos;
	//An array that stores the Transforms of all instantiated points
	private Vector2[] pointsTransform;
	private Vector2[] relativePoints;
	public int length = 32;
	public AnimationCurve curve = AnimationCurve.Linear(0.0F, 1.0F, 1.0F, 1.0F);

	private GameObject[] areas;
	private Vector2[] areaVertices;
	private ColliderToMesh[] meshGenerators;

	private Vector2 smoothingVelocity = Vector2.zero;
	public float smoothingTime = 0.05F;
	public float amplitude = 1.0F;

	void Awake ()
	{
		//Get and store a reference to the following attached components:
		//LineRenderer
		this.lRenderer = GetComponent<LineRenderer>();
		//Transform
		this.goTransform = GetComponent<Transform>();

		tracklist = GameObject.FindGameObjectWithTag ("Tracklist");
		aSource = tracklist.GetComponents<AudioSource> () [trackIndex];
	}

	void Start()
	{
		lRenderer.SetVertexCount(length);

		pointsTransform = new Vector2[length];
		relativePoints = new Vector2[length];

		areas = new GameObject[length-1];
		areaVertices = new Vector2[4];
		areaVertices [0] = new Vector2 ();
		areaVertices [1] = new Vector2 ();
		meshGenerators = new ColliderToMesh[length - 1];

		//For each sample
		for(int i=0; i<length;i++)
		{
			relativePoints [i] = new Vector2 ((float)(i) / length, 0.0F);
			pointsTransform [i] = goTransform.TransformPoint (relativePoints [i]);
		}

		for (int i = 0; i < length-1; ++i) {
			areas [i] = new GameObject ("Wave area " + i);
			areas [i].transform.parent = goTransform;
			areas [i].transform.localPosition = Vector3.zero;
			areas [i].transform.localScale = new Vector3 (1.0F, 1.0F, 1.0F);
			areas [i].transform.localRotation = Quaternion.Euler(Vector3.zero);
			areas[i].AddComponent<EdgeCollider2D> ();
			areaVertices [0].Set (relativePoints[i].x, 0);
			areaVertices [1].Set (relativePoints[i+1].x, 0);
			areaVertices [2] = relativePoints[i];
			areaVertices [3] = relativePoints[i + 1];
			areas [i].GetComponent<EdgeCollider2D> ().points = areaVertices;
			areas [i].AddComponent<MeshRenderer> ().material = lRenderer.material;
			areas [i].AddComponent<MeshFilter> ();
			areas [i].AddComponent<Rigidbody2D> ().isKinematic = true;
			meshGenerators [i] = new ColliderToMesh (areas [i]);
		}
	}

	void Update ()
	{
		float samplesSum = 0.0F;
		aSource.GetSpectrumData (samples, 0, FFTWindow.BlackmanHarris);
		for (int j = 0; j < 64; ++j) {
			samplesSum += samples [j] * trackPriority.Evaluate (j);
		}
		samplesSum *= aSource.volume;

		//For each sample
		for(int i=0; i<length;i++)
		{
			float newValue =  samplesSum * amplitude * curve.Evaluate(relativePoints[i].x);
			targetPointPos = relativePoints [i];
			targetPointPos.y = newValue;
			smoothingVelocity.y *= 0.01F;
			relativePoints[i] = Vector2.SmoothDamp(relativePoints[i], targetPointPos, ref smoothingVelocity, smoothingTime);
			pointsTransform[i] = goTransform.TransformPoint (relativePoints [i]);

			lRenderer.SetPosition(i, new Vector3(pointsTransform[i].x, pointsTransform[i].y, 0));
		}
		for (int i = 0; i < length-1; ++i) {
			areaVertices [0].Set (relativePoints[i].x, 0);
			areaVertices [1].Set (relativePoints[i+1].x, 0);
			areaVertices [2] = relativePoints[i];
			areaVertices [3] = relativePoints[i + 1];
			areas [i].GetComponent<EdgeCollider2D> ().points = areaVertices;
			meshGenerators [i].GetMesh ();
		}
	}
}