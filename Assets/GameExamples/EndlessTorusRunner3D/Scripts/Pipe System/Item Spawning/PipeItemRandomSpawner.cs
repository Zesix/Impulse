using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    public class PipeItemRandomSpawner : PipeItemGenerator
    {
        #region Properties
        // Items to spawn.
        [SerializeField]
        PipeItem[] itemPrefabs;
        #endregion

        /// <summary>
        /// Generates one random item per ring segment of the given pipe, aligning it with a random quad on each ring.
        /// </summary>
        /// <param name="pipe">The pipe to generate random items on.</param>
        public override void GenerateItems(Pipe pipe)
        {
            float angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
            for (int i = 0; i < pipe.CurveSegmentCount; i++)
            {
                PipeItem item = Instantiate<PipeItem>(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
                float pipeRotation = (Random.Range(0, pipe.PipeSegmentCount) + 0.5f) * 360f / pipe.PipeSegmentCount;
                item.Position(pipe, i * angleStep, pipeRotation);
            }
        }
    }
}
