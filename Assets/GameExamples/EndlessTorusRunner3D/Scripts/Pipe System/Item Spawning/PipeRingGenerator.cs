using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    // Generate a pipe ring item. This class is currently TODO.
    public class PipeRingItem : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        float curveRadius;

        [SerializeField]
        float pipeRadius;

        [SerializeField]
        int curveSegmentCount;

        [SerializeField]
        int pipeSegmentCount;

        Transform rotater;
        #endregion

        private void Awake()
        {
            rotater = transform.GetChild(0);
        }

        public void GenerateRing(float curveRadius, float pipeRadius, int curveSegmentCount, int pipeSegmentCount)
        {
            this.curveRadius = curveRadius;
            this.pipeRadius = pipeRadius;
            this.curveSegmentCount = curveSegmentCount;
            this.pipeSegmentCount = pipeSegmentCount;

            float vStep = (2f * Mathf.PI) / pipeSegmentCount;

            // Gets a bunch of points that form a ring, but right now there is no way to connect them. Use OnDrawGizmos to see the points.
            for (int v = 0; v < pipeSegmentCount; v++)
            {
                Vector3 point = GetPointOnTorus(0f, v * vStep);
            }
        }

        Vector3 GetPointOnTorus(float u, float v)
        {
            Vector3 p;
            float r = (curveRadius + pipeRadius * Mathf.Cos(v));
            p.x = r * Mathf.Sin(u);
            p.y = r * Mathf.Cos(u);
            p.z = pipeRadius * Mathf.Sin(v);
            return p;
        }

        public void Position (Pipe pipe, float curveRotation, float ringRotation)
        {
            transform.SetParent(pipe.transform, false);
            transform.localRotation = Quaternion.Euler(0f, 0f, -curveRotation);
            rotater.localPosition = new Vector3(0f, pipe.CurveRadius);
            rotater.localRotation = Quaternion.Euler(ringRotation, 0f, 0f);
        }
    }
}