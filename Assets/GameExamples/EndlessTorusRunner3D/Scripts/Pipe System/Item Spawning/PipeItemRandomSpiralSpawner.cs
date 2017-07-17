using UnityEngine;

namespace EndlessTorusRunner3D
{
    /// <summary>
    /// Spawns items in a spiral, either clockwise or counterclockwise.
    /// </summary>
    public class PipeItemRandomSpiralSpawner : PipeItemGenerator
    {
        // Items to spawn.
        [SerializeField] private PipeItem[] _itemPrefabs;

        public override void GenerateItems(Pipe pipe)
        {
            var start = (Random.Range(0, pipe.PipeSegmentCount) + 0.5f);
            var direction = Random.value < 0.5f ? 1f : -1f;

            var angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
            for (var i = 0; i < pipe.CurveSegmentCount; i++)
            {
                var item = Instantiate(_itemPrefabs[Random.Range(0, _itemPrefabs.Length)]);
                var pipeRotation = (start + i * direction) * 360f / pipe.PipeSegmentCount;
                item.Position(pipe, i * angleStep, pipeRotation);
            }
        }
    }
}
