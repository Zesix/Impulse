using UnityEngine;

namespace EndlessTorusRunner3D
{
    public class PipeItemRandomSpawner : PipeItemGenerator
    {
        // Items to spawn.
        [SerializeField] private PipeItem[] _itemPrefabs;

        /// <summary>
        /// Generates one random item per ring segment of the given pipe, aligning it with a random quad on each ring.
        /// </summary>
        /// <param name="pipe">The pipe to generate random items on.</param>
        public override void GenerateItems(Pipe pipe)
        {
            var angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
            for (var i = 0; i < pipe.CurveSegmentCount; i++)
            {
                var item = Instantiate(_itemPrefabs[Random.Range(0, _itemPrefabs.Length)]);
                var pipeRotation = (Random.Range(0, pipe.PipeSegmentCount) + 0.5f) * 360f / pipe.PipeSegmentCount;
                item.Position(pipe, i * angleStep, pipeRotation);
            }
        }
    }
}
