using UnityEngine;

namespace EndlessTorusRunner3D
{
    public class PipeSystem : MonoBehaviour
    {
        // Pipe prefab.
        [SerializeField] private Pipe _pipePrefab;

        // Number of pipes.
        [SerializeField] private int _pipeCount;

        // Number of pipes without any items. Empty pipes are always spawned at the start of the game.
        [SerializeField] private int _emptyPipeCount = 2;

        // Used to rotate the system since we want the player to move forward for convenience.
        [SerializeField] private Transform _worldOrienter;
        public Transform WorldOrienter => _worldOrienter;

        public float DeltaToRotation { get; set; }

        public float SystemRotation { get; set; }

        private float _worldRotation;

        private Pipe[] _pipes;

        public void Init()
        {
            _pipes = new Pipe[_pipeCount];
            for (var i = 0; i < _pipes.Length; i++)
            {
                var pipe = _pipes[i] = Instantiate(_pipePrefab);
                pipe.transform.SetParent(transform, false);
                pipe.Generate(i > _emptyPipeCount);

                // After creating the first pipe, align future pipes with the one created previously.
                if (i > 0)
                {
                    pipe.AlignWith(_pipes[i - 1]);
                }
            }
            AlignNextPipeWithOrigin();
        }

        /// <summary>
        /// Sets up the first pipe by moving the entire pipe system down so the first pipe is at the origin.
        /// </summary>
        /// <returns>The first pipe.</returns>
        public Pipe SetupFirstPipe()
        {
            transform.localPosition = new Vector3(0f, -_pipes[1].CurveRadius);
            return _pipes[1];
        }

        /// <summary>
        /// Orients the world object that parents the Pipe System.
        /// </summary>
        /// <param name="currentPipe">The pipe we want to orient the world to.</param>
        public void OrientWorld(Pipe currentPipe)
        {
            // Set initial delta to rotation. This is used in rotating the system so the player can keep moving forward.
            DeltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
            _worldRotation += currentPipe.RelativeRotation;

            if (_worldRotation < 0f)
            {
                _worldRotation += 360f;
            }
            else if (_worldRotation >= 360f)
            {
                _worldRotation -= 360f;
            }
            _worldOrienter.localRotation = Quaternion.Euler(_worldRotation, 0f, 0f);
        }

        /// <summary>
        /// Sets up the next pipe.
        /// </summary>
        /// <returns>The next pipe.</returns>
        public Pipe SetupNextPipe()
        {
            // Shift the pipes in the array.
            ShiftPipes();

            // Align the next pipe with the origin.
            AlignNextPipeWithOrigin();

            // Generate next pipe.
            _pipes[_pipes.Length - 1].Generate();
            _pipes[_pipes.Length - 1].AlignWith(_pipes[_pipes.Length - 2]);

            // Reset its position.
            transform.localPosition = new Vector3(0f, -_pipes[1].CurveRadius);
            return _pipes[1];
        }

        private void ShiftPipes()
        {
            var temp = _pipes[0];
            for (int i = 1; i < _pipes.Length; i++)
            {
                _pipes[i - 1] = _pipes[i];
            }
            _pipes[_pipes.Length - 1] = temp;
        }

        private void AlignNextPipeWithOrigin()
        {
            var transformToAlign = _pipes[1].transform;

            // Temporarily make all other pipes a children of this pipe.
            for (var i = 0; i < _pipes.Length; i++)
            {
                if (i != 1)
                    _pipes[i].transform.SetParent(transformToAlign);
            }

            // Reset transform position and rotation, which shifts all other pipes with it.
            transformToAlign.localPosition = Vector3.zero;
            transformToAlign.localRotation = Quaternion.identity;

            // Re-parent all pipes.
            for (var i = 0; i < _pipes.Length; i++)
            {
                if (i != 1)
                    _pipes[i].transform.SetParent(transform);
            }
        }
    }
}
