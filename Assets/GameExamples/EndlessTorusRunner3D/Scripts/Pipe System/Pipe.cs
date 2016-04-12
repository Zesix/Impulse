using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    public class Pipe : MonoBehaviour
    {
        #region Properties
        // Radius of the pipe.
        [SerializeField]
        float pipeRadius;
        public float PipeRadius
        {
            get { return pipeRadius; }
        }

        // Radius of the curve the pipe follows.
        [SerializeField]
        float minCurveRadius;
        [SerializeField]
        float maxCurveRadius;

        // Curve segment counts.
        [SerializeField]
        int minCurveSegmentCount;
        [SerializeField]
        int maxCurveSegmentCount;

        // Segments on the pipe. Used to separate the pipe into quads.
        [SerializeField]
        int pipeSegmentCount;
        public int PipeSegmentCount
        {
            get { return pipeSegmentCount; }
        }

        // Distance between rings, since this pipe is only a fraction of the torus.
        [SerializeField]
        float ringDistance;

        // Spot spawner.
        [SerializeField]
        PipeItemRingSpawner spotSpawner;

        // Random item spawners.
        [SerializeField]
        PipeItemGenerator[] randomItemSpawners;

        // Angle curve of previous pipe.
        float curveAngle;
        public float CurveAngle
        {
            get { return curveAngle; }
        }

        // Curve radius of this pipe.
        float curveRadius;
        public float CurveRadius
        {
            get { return curveRadius; }
        }

        // Relative rotation.
        float relativeRotation;
        public float RelativeRotation
        {
            get { return relativeRotation; }
        }

        // Number of segments along a curve.
        int curveSegmentCount;
        public int CurveSegmentCount
        {
            get { return curveSegmentCount; }
        }

        Mesh mesh;
        Vector3[] vertices;
        int[] triangles;

        // Used for setting texture.
        Vector2[] uv;
        #endregion

        void Awake()
        {
            // Error checks.
            if (pipeRadius == 0f)
                Debug.LogError("Pipe Radius not specified in Pipe component.");
            if (minCurveRadius == 0f || maxCurveRadius == 0f)
                Debug.LogError("Min and Max Curve Radii not specified in Pipe component.");
            if (minCurveSegmentCount == 0 || maxCurveSegmentCount == 0)
                Debug.LogError("Min and Max Curve Segment Counts not specified in Pipe component.");
            if (pipeSegmentCount == 0)
                Debug.LogError("Pipe Segment Count not specified in Pipe component.");
            if (ringDistance == 0)
                Debug.LogError("Ring Distance not specified in Pipe component.");

            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Pipe";
        }

        /// <summary>
        /// Generates a pipe.
        /// </summary>
        public void Generate(bool withItems = true)
        {
            // Generate curve radius and curve segment count based on min/max values.
            curveRadius = Random.Range(minCurveRadius, maxCurveRadius);
            curveSegmentCount = Random.Range(minCurveSegmentCount, maxCurveSegmentCount + 1);

            mesh.Clear();
            SetVertices();
            SetUV();
            SetTriangles();
            mesh.RecalculateNormals();

            // Destroy previously generated items.
            for (int i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);

            // Generate spot items.
            spotSpawner.GenerateItems(this);

            // Generate random items.
            if (withItems)
            {
                randomItemSpawners[Random.Range(0, randomItemSpawners.Length)].GenerateItems(this);
            }
        }

        /// <summary>
        /// Aligns a pipe with a previous pipe based on curveAngle.
        /// </summary>
        /// <param name="pipe">The previous pipe we are aligning with.</param>
        public void AlignWith(Pipe pipe)
        {
            // Generate a random value for the pipe's rotation.
            relativeRotation = Random.Range(0, curveSegmentCount) * 360f / pipeSegmentCount;

            transform.SetParent(pipe.transform, false);

            // Ensure this pipe is at its parent's origin.
            transform.localPosition = Vector3.zero;
            // Align with the parent rotation.
            transform.localRotation = Quaternion.Euler(0f, 0f, -pipe.curveAngle);
            // Move up so this pipe's origin sits at the end point of the parent's pipe.
            transform.Translate(0f, pipe.curveRadius, 0f);
            // Perform random rotation.
            transform.Rotate(relativeRotation, 0f, 0f);
            // Move down, from this pipe's point of view, to align the pipe end and start.
            transform.Translate(0f, -curveRadius, 0f);
            transform.SetParent(pipe.transform.parent);
            transform.localScale = Vector3.one;
        }

        // We give each quad its own four vertices instead of sharing them with neighboring quads.
        void SetVertices()
        {
            vertices = new Vector3[pipeSegmentCount * curveSegmentCount * 4];
            float uStep = ringDistance / curveRadius;
            curveAngle = uStep * curveSegmentCount * (360f / (2f * Mathf.PI));
            CreateFirstQuadRing(uStep);
            int iDelta = pipeSegmentCount * 4;
            for (int u = 2, i = iDelta; u <= curveSegmentCount; u++, i += iDelta)
            {
                CreateQuadRing(u * uStep, i);
            }
            mesh.vertices = vertices;
        }

        void SetUV()
        {
            uv = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; i += 4)
            {
                uv[i] = Vector2.zero;
                uv[i + 1] = Vector2.right;
                uv[i + 2] = Vector2.up;
                uv[i + 3] = Vector2.one;
            }
            mesh.uv = uv;
        }

        void SetTriangles()
        {
            triangles = new int[pipeSegmentCount * curveSegmentCount * 6];
            for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4)
            {
                triangles[t] = i;
                triangles[t + 1] = triangles[t + 4] = i + 2;
                triangles[t + 2] = triangles[t + 3] = i + 1;
                triangles[t + 5] = i + 3;
            }
            mesh.triangles = triangles;
        }

        // Works the same as CreateFirstQuadRing() except it only needs to add a single vertex each step.
        // It copies the first two vertices per quad from the previous ring.
        void CreateQuadRing(float u, int i)
        {
            float vStep = (2f * Mathf.PI) / pipeSegmentCount;
            int ringOffset = pipeSegmentCount * 4;

            Vector3 vertex = GetPointOnTorus(u, 0f);
            for (int v = 1; v <= pipeSegmentCount; v++, i += 4)
            {
                vertices[i] = vertices[i - ringOffset + 2];
                vertices[i + 1] = vertices[i - ringOffset + 3];
                vertices[i + 2] = vertex;
                vertices[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
            }
        }

        void CreateFirstQuadRing(float u)
        {
            float vStep = (2f * Mathf.PI) / pipeSegmentCount;

            // Get two vertices along u
            Vector3 vertexA = GetPointOnTorus(0f, 0f);
            Vector3 vertexB = GetPointOnTorus(u, 0f);

            // Step along v and loop until we go full circle. Assign the vertices to the quads as we work through the segments.
            for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i += 4)
            {
                vertices[i] = vertexA;
                vertices[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
                vertices[i + 2] = vertexB;
                vertices[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
            }
        }

        // A torus can be described as a 3D sinusoid function.
        // x = (R + r(cos(v))cos(u))
        // y = (R + r(cos(v))sin(u))
        // z = r(cos(v))
        Vector3 GetPointOnTorus(float u, float v)
        {
            Vector3 p;
            float r = (curveRadius + pipeRadius * Mathf.Cos(v));
            p.x = r * Mathf.Sin(u);
            p.y = r * Mathf.Cos(u);
            p.z = pipeRadius * Mathf.Sin(v);
            return p;
        }

        /* Used for debug purposes.
        void OnDrawGizmos()
        {
            float uStep = (2f * Mathf.PI) / curveSegmentCount;
            float vStep = (2f * Mathf.PI) / pipeSegmentCount;

            for (int u = 0; u < curveSegmentCount; u++)
            {
                for (int v = 0; v < pipeSegmentCount; v++)
                {
                    Vector3 point = GetPointOnTorus(u * uStep, v * vStep);
                    Gizmos.color = new Color(
                        1f,
                        (float)v / pipeSegmentCount,
                        (float)u / curveSegmentCount);
                    Gizmos.DrawSphere(point, 0.1f);
                }
            }
        }
        */
    }
}
