using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    public class PipeSystem : MonoBehaviour
    {
        #region Properties
        // Pipe prefab.
        [SerializeField]
        Pipe pipePrefab;

        // Number of pipes.
        [SerializeField]
        int pipeCount;

        // Number of pipes without any items. Empty pipes are always spawned at the start of the game.
        [SerializeField]
        int emptyPipeCount = 2;

        // Used to rotate the system since we want the player to move forward for convenience.
        [SerializeField]
        Transform worldOrienter;
        public Transform WorldOrienter
        {
            get { return worldOrienter; }
        }

        float deltaToRotation;
        public float DeltaToRotation
        {
            get { return deltaToRotation; }
            set { deltaToRotation = value; }
        }

        float systemRotation;
        public float SystemRotation
        {
            get { return systemRotation; }
            set { systemRotation = value; }
        }

        float worldRotation;

        Pipe[] pipes;
        #endregion

        public void Init()
        {
            pipes = new Pipe[pipeCount];
            for (int i = 0; i < pipes.Length; i++)
            {
                Pipe pipe = pipes[i] = Instantiate<Pipe>(pipePrefab);
                pipe.transform.SetParent(transform, false);
                pipe.Generate(i > emptyPipeCount);

                // After creating the first pipe, align future pipes with the one created previously.
                if (i > 0)
                {
                    pipe.AlignWith(pipes[i - 1]);
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
            transform.localPosition = new Vector3(0f, -pipes[1].CurveRadius);
            return pipes[1];
        }

        /// <summary>
        /// Orients the world object that parents the Pipe System.
        /// </summary>
        /// <param name="currentPipe">The pipe we want to orient the world to.</param>
        public void OrientWorld(Pipe currentPipe)
        {
            // Set initial delta to rotation. This is used in rotating the system so the player can keep moving forward.
            deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
            worldRotation += currentPipe.RelativeRotation;

            if (worldRotation < 0f)
            {
                worldRotation += 360f;
            }
            else if (worldRotation >= 360f)
            {
                worldRotation -= 360f;
            }
            worldOrienter.localRotation = Quaternion.Euler(worldRotation, 0f, 0f);
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
            pipes[pipes.Length - 1].Generate();
            pipes[pipes.Length - 1].AlignWith(pipes[pipes.Length - 2]);

            // Reset its position.
            transform.localPosition = new Vector3(0f, -pipes[1].CurveRadius);
            return pipes[1];
        }

        void ShiftPipes()
        {
            Pipe temp = pipes[0];
            for (int i = 1; i < pipes.Length; i++)
            {
                pipes[i - 1] = pipes[i];
            }
            pipes[pipes.Length - 1] = temp;
        }

        void AlignNextPipeWithOrigin()
        {
            Transform transformToAlign = pipes[1].transform;

            // Temporarily make all other pipes a children of this pipe.
            for (int i = 0; i < pipes.Length; i++)
            {
                if (i != 1)
                    pipes[i].transform.SetParent(transformToAlign);
            }

            // Reset transform position and rotation, which shifts all other pipes with it.
            transformToAlign.localPosition = Vector3.zero;
            transformToAlign.localRotation = Quaternion.identity;

            // Re-parent all pipes.
            for (int i = 0; i < pipes.Length; i++)
            {
                if (i != 1)
                    pipes[i].transform.SetParent(transform);
            }
        }
    }
}
