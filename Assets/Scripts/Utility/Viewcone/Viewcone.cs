// Used to procedurally generate a viewcone. 
// Use polar coordinates for drawing all vertices. 
using UnityEngine;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]

public class Viewcone : MonoBehaviour
{
	public float Length = 2.0f;
	public float Radius = 2.0f;
	public int Sections = 20;
	
	private float _step;
	private float _cAngle;
	
	public bool SharedVertices;

	private MeshCollider _ourCollider;
	private ViewconeDetection _spotter;

	private void Start ()
	{
		_spotter = GetComponentInParent<ViewconeDetection> ();
		if (_spotter == null)
		{
			Debug.LogWarning ("ViewconeDetection script not found on parent of " + gameObject.name);
		}
	}

	public void Rebuild ()
	{
		var meshFilter = GetComponent<MeshFilter> ();
		if (meshFilter == null) {
			Debug.LogError ("MeshFilter not found!");
			return;
		}
                
		var mesh = meshFilter.sharedMesh;
		if (mesh == null) {
			meshFilter.mesh = new Mesh ();
			mesh = meshFilter.sharedMesh;
		}
		mesh.Clear ();
                
		if (Sections < 3) {
			Debug.LogError ("Number of viewcone sections must be 3 or more");
			return;
		}

		_step = (2 * Mathf.PI) / Sections;
		_cAngle = 2 * Mathf.PI; //-- start in 360 and going decrement
			
		// -- each point along circle --plus center  & vertex peak--//
		var cVertices = new Vector3[Sections + 1 + 1];	
			
		//--First vertex --//
		cVertices [0] = new Vector3 (0, 0, 0); // center of circle
		
		//--Generate vertices remains --//
		for (var i=1; i<(Sections+1); i++) {
			cVertices [i] = new Vector3 (Mathf.Sin (_cAngle) * Radius, Mathf.Cos (_cAngle) * Radius, 0);
			_cAngle += _step;
		}
		
		//--Peak cone vertex --//
		cVertices [cVertices.Length - 1] = new Vector3 (0, 0, Length); // center of circle
			
		var idx = 1;
		var indices = (Sections) * 3; // Only for circle triangles
		indices *= 2; //-- X2 for every triangle in wall of cone
		
		// -- Already have vertices, now build triangles --//
		var cTriangles = new int[indices]; // one triagle for each section (has 3 vertex per triang)
			
		//Debug.Log (cVertices [0].x + "   " + cVertices [0].z + "   " + cVertices [0].z);
			
		//-- Fill Circle mesh --//
		for (var i = 0; i < indices * .5; i += 3) {
			cTriangles [i] = 0; //center of circle
			cTriangles [i + 1] = idx; //next vertex
				
			
			if (i >= indices * .5 - 3) {
				//-- if is the last vertex (one loop)
				cTriangles [i + 2] = 1;	
			} else {
				//-- if is the next point --//
				cTriangles [i + 2] = idx + 1; //next next vertex	
			}
			idx++;
		}
		
		//-- Reset idx (indices pointer)-- //
		idx = 1;
		//--Fill cone wall--//
		for (var i=(int)(indices*.5); i<indices; i+=3) {
				
			cTriangles [i] = idx; //next vertex
			cTriangles [i + 1] = cVertices.Length - 1; // Peak vertex
				
			
			if (i >= indices - 3) {
				//-- if is the last vertex (one loop)
				cTriangles [i + 2] = 1; // Peak vertex;	
			} else {
				//-- if is the next point --//
				cTriangles [i + 2] = idx + 1; //next next vertex	
			}
			
			idx++;
		}
				
		mesh.vertices = cVertices;
		mesh.triangles = cTriangles;

		GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Standard")) {color = Color.yellow}; // REF1
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();

		_ourCollider = GetComponent<MeshCollider> ();
		if (_ourCollider != null) {
			// Redraw convex for new cone.
			_ourCollider.isTrigger = false;
			_ourCollider.convex = false;
			_ourCollider.sharedMesh = mesh;
			_ourCollider.convex = true;
			_ourCollider.isTrigger = true;
		} else {
			_ourCollider = gameObject.AddComponent<MeshCollider> ();
			_ourCollider.sharedMesh = mesh;
			_ourCollider.convex = true;
			_ourCollider.isTrigger = true;
		}
	}

	private void OnTriggerStay (Collider col)
	{
		_spotter.ObjectSpotted (col);
	}

	private void OnTriggerExit (Collider col)
	{
		_spotter.ObjectLeft (col);
	}
        
}