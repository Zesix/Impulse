/*****************************************
 * This file is part of Impulse Framework.

    Impulse Framework is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    Impulse Framework is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Impulse Framework.  If not, see <http://www.gnu.org/licenses/>.
*****************************************/

// Used to procedurally generate a viewcone. 
// Use polar coordinates for drawing all vertices. 
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]

public class Viewcone : MonoBehaviour
{
	public float length = 2.0f;
	public float radius = 2.0f;
	public int sections = 20;
	float step;
	float cAngle;
	public bool sharedVertices = false;

	private MeshCollider ourCollider;
	private ViewconeDetection spotter;

	void Start ()
	{
		spotter = GetComponentInParent<ViewconeDetection> ();
		if (spotter == null)
		{
			Debug.LogWarning ("ViewconeDetection script not found on parent of " + gameObject.name);
		}
	}

	public void Rebuild ()
	{
		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		if (meshFilter == null) {
			Debug.LogError ("MeshFilter not found!");
			return;
		}
                
		Mesh mesh = meshFilter.sharedMesh;
		if (mesh == null) {
			meshFilter.mesh = new Mesh ();
			mesh = meshFilter.sharedMesh;
		}
		mesh.Clear ();
                
		if (sections < 3) {
			Debug.LogError ("Number of viewcone sections must be 3 or more");
			return;
		}

		step = (2 * Mathf.PI) / sections;
		cAngle = 2 * Mathf.PI; //-- start in 360 and going decrement
			
		// -- each point along circle --plus center  & vertex peak--//
		Vector3 [] cVertices = new Vector3[sections + 1 + 1];	
			
		//--First vertex --//
		cVertices [0] = new Vector3 (0, 0, 0); // center of circle
		
		//--Generate vertices remains --//
		for (int i=1; i<(sections+1); i++) {
			cVertices [i] = new Vector3 (Mathf.Sin (cAngle) * radius, Mathf.Cos (cAngle) * radius, 0);
			cAngle += step;
		}
		
		//--Peak cone vertex --//
		cVertices [cVertices.Length - 1] = new Vector3 (0, 0, length); // center of circle
			
		int idx = 1;
		int indices = (sections) * 3; // Only for circle triangles
		indices *= 2; //-- X2 for every triangle in wall of cone
		
		// -- Already have vertices, now build triangles --//
		int [] cTriangles = new int[indices]; // one triagle for each section (has 3 vertex per triang)
			
		//Debug.Log (cVertices [0].x + "   " + cVertices [0].z + "   " + cVertices [0].z);
			
		//-- Fill Circle mesh --//
		for (int i=0; i<(indices*.5); i+=3) {
			cTriangles [i] = 0; //center of circle
			cTriangles [i + 1] = idx; //next vertex
				
			
			if (i >= (indices * .5 - 3)) {
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
		for (int i=(int)(indices*.5); i<(indices); i+=3) {
				
			cTriangles [i] = idx; //next vertex
			cTriangles [i + 1] = cVertices.Length - 1; // Peak vertex
				
			
			if (i >= (indices - 3)) {
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
		
		GetComponent<Renderer> ().sharedMaterial = new Material (Shader.Find ("Standard")); // REF1
		GetComponent<Renderer> ().sharedMaterial.color = Color.yellow;
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		;

		ourCollider = GetComponent<MeshCollider> ();
		if (ourCollider != null) {
			// Redraw convex for new cone.
			ourCollider.isTrigger = false;
			ourCollider.convex = false;
			ourCollider.sharedMesh = mesh;
			ourCollider.convex = true;
			ourCollider.isTrigger = true;
		} else {
			ourCollider = gameObject.AddComponent<MeshCollider> ();
			ourCollider.sharedMesh = mesh;
			ourCollider.convex = true;
			ourCollider.isTrigger = true;
		}
	}

	void OnTriggerStay (Collider col)
	{
		spotter.ObjectSpotted (col);
	}

	void OnTriggerExit (Collider col)
	{
		spotter.ObjectLeft (col);
	}
        
}