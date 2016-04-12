using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    public abstract class PipeItemGenerator : MonoBehaviour
    {
        public abstract void GenerateItems(Pipe pipe);
    }
}
