using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    public class PipeItem : MonoBehaviour
    {
        #region Properties
        // This should be attached to the root object of an item with a rotater child, which in turn has a 3D model child.
        Transform rotater;

        #endregion

        void Awake()
        {
            rotater = transform.GetChild(0);
        }

        public void Position (Pipe pipe, float curveRotation, float ringRotation)
        {
            transform.SetParent(pipe.transform, false);
            transform.localRotation = Quaternion.Euler(0f, 0f, -curveRotation);
            rotater.localPosition = new Vector3(0f, pipe.CurveRadius);
            rotater.localRotation = Quaternion.Euler(ringRotation + Random.Range(0, pipe.CurveRadius), 0f, 0f);
        }
    }
}
