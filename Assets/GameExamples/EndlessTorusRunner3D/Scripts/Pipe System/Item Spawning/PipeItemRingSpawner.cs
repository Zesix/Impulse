using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    /// <summary>
    /// Spawns items in a spiral, either clockwise or counterclockwise.
    /// </summary>
    public class PipeItemRingSpawner : PipeItemGenerator
    {
        #region Properties
        // Items to spawn.
        [SerializeField]
        PipeItem[] itemPrefabs;
        #endregion

        public override void GenerateItems(Pipe pipe)
        {
            for (int i = 0; i < pipe.PipeSegmentCount; i++)
            {
                float direction = Random.value < 0.5f ? 1f : -1f;

                float angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
                for (int y = 0; y < pipe.CurveSegmentCount; y++)
                {
                    PipeItem item = Instantiate<PipeItem>(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
                    float pipeRotation = (y * direction) * 360f / pipe.PipeSegmentCount;
                    item.Position(pipe, y * angleStep, pipeRotation);
                }
            }
        }
    }
}
