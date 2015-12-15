using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour
{
	//An AudioSource object so the music can be played
	private AudioSource aSource;
	//A float array that stores the audio samples
	public float[] samples = new float[64];
	//A renderer that will draw a line at the screen
	private LineRenderer lRenderer;
	//The transform attached to this game object
	private Transform goTransform;
	private Vector2 goTransform2D;
	//The position of the current point. Will also be the position of each point of the line.
	private Vector2 pointPos;
	//An array that stores the Transforms of all instantiated points
	private Vector2[] pointsTransform;
	private Vector2[] relativePoints;

	private GameObject[] areas;
	private Vector2[] areaVertices;
	private ColliderToMesh[] meshGenerators;

	private Vector2[] smoothingVelocities;
	public float smoothingTime = 0.05F;
	public float amplitude = 50.0F;
	public float volume = 1.0F;

	public int samplesPerPoint = 1;

	void Awake ()
	{
		//Get and store a reference to the following attached components:
		//AudioSource
		this.aSource = GetComponent<AudioSource>();
		//LineRenderer
		this.lRenderer = GetComponent<LineRenderer>();
		//Transform
		this.goTransform = GetComponent<Transform>();
		goTransform2D = new Vector2(goTransform.position.x, goTransform.position.y);
	}

	void Start()
	{
		int length = samples.Length / samplesPerPoint + (samples.Length % samplesPerPoint > 0 ? 1 : 0);
		//The line should have the same number of points as the number of samples
		lRenderer.SetVertexCount(length);
		//The pointsTransform array should be initialized with the same length as the samples array
		pointsTransform = new Vector2[length];
		relativePoints = new Vector2[length];
		areas = new GameObject[length-1];
		areaVertices = new Vector2[4];
		areaVertices [0] = new Vector2 ();
		areaVertices [1] = new Vector2 ();
		meshGenerators = new ColliderToMesh[length - 1];
		smoothingVelocities = new Vector2[length];
		//Center the audio visualization line at the X axis, according to the samples array length
		goTransform.position = new Vector2(-samples.Length/2,goTransform.position.y);

		//For each sample
		for(int i=0; i<length;i++)
		{
			//Get the recently instantiated point Transform component
			smoothingVelocities[i] = Vector2.zero;
			pointsTransform[i] = new Vector2(goTransform.position.x + i*samplesPerPoint, goTransform.position.y);
			relativePoints [i] = pointsTransform [i] - goTransform2D;
		}
		for (int i = 0; i < length-1; ++i) {
			areas [i] = new GameObject ("Wave area " + i);
			areas [i].transform.parent = goTransform;
			areas[i].AddComponent<PolygonCollider2D> ();
			areaVertices [0].Set (goTransform2D.x + i*samplesPerPoint, 0);
			areaVertices [1].Set (goTransform2D.x + (i + 1)*samplesPerPoint, 0);
			areaVertices [2] = pointsTransform [i];
			areaVertices [3] = pointsTransform [i + 1];
			areas[i].GetComponent<PolygonCollider2D> ().points = areaVertices;
			areas [i].AddComponent<MeshRenderer> ().material = lRenderer.material;
			areas [i].AddComponent<MeshFilter> ();
			areas [i].AddComponent<Rigidbody2D> ().isKinematic = true;
			areas [i].AddComponent<SmoothingSpeedStorage> ();
			meshGenerators [i] = new ColliderToMesh (areas [i]);
		}
	}

	void Update ()
	{
		int length = samples.Length / samplesPerPoint + (samples.Length % samplesPerPoint > 0 ? 1 : 0);
		//Obtain the samples from the frequency bands of the attached AudioSource
		aSource.GetSpectrumData(this.samples,0,FFTWindow.BlackmanHarris);

		//For each sample
		for(int i=0; i<length;i++)
		{
			/*Set the pointPos Vector3 to the same value as the position of the corresponding
			 * point. However, set it's Y element according to the current sample.*/
			float samplesSum = 0;
			for (int j = 0; j < samplesPerPoint && j + i * samplesPerPoint < samples.Length; ++j) {
				samplesSum += samples [i * samplesPerPoint + j];
			}
			pointPos = Vector2.SmoothDamp(pointsTransform[i], new Vector2(pointsTransform[i].x, volume*Mathf.Clamp(samplesSum*(amplitude+amplitude*i*i/50),0,amplitude)), ref smoothingVelocities[i], smoothingTime);

			//Set the point to the new Y position
			pointsTransform[i] = pointPos;
			relativePoints[i] = pointsTransform [i] - goTransform2D;

			/*Set the position of each vertex of the line based on the point position.
			 * Since this method only takes absolute World space positions, it has
			 * been subtracted by the current game object position.*/
			lRenderer.SetPosition(i, new Vector3(pointPos.x, pointPos.y, 0));
		}
		for (int i = 0; i < length-1; ++i) {
			areaVertices [0].Set (goTransform.position.x + i*samplesPerPoint, 0);
			areaVertices [1].Set (goTransform.position.x + (i + 1)*samplesPerPoint, 0);
			areaVertices [2] = pointsTransform [i];
			areaVertices [3] = pointsTransform [i + 1];
			areas [i].GetComponent<PolygonCollider2D> ().points = areaVertices;
			meshGenerators [i].GetMesh ();
			areas [i].GetComponent<SmoothingSpeedStorage> ().smoothingSpeed1 = smoothingVelocities [i];
			areas [i].GetComponent<SmoothingSpeedStorage> ().smoothingSpeed2 = smoothingVelocities [i + 1];
		}
	}
}