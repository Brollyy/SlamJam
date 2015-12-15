using UnityEngine;
using System.Collections;

public class ColliderToMesh {

	private GameObject source; 
	private Vector3[] vertices;
	private Mesh mesh;

	public ColliderToMesh(GameObject src) {
		this.source = src;
		vertices = new Vector3[4];
		for (int j = 0; j < 4; ++j) {
			vertices [j] = new Vector3 ();
		}
		mesh = new Mesh ();
	}

	public void GetMesh() {
		PolygonCollider2D pc2 = source.GetComponent<PolygonCollider2D>();

		MeshFilter mf = source.GetComponent<MeshFilter>();
		Vector2[] points = pc2.points;
		for(int j=0; j<4; j++){
			vertices[j].Set(points[j].x, points[j].y, 0);
		}
		int[] triangles = { 0, 1, 2, 1, 3, 2 };
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mf.mesh = mesh;
	}
}