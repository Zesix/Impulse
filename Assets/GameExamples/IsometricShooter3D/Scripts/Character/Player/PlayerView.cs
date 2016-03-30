using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{
    [RequireComponent(typeof(PlayerModel))]
    public class PlayerView : MonoBehaviour
    {

        /// <summary>
        /// Look at the specified position.
        /// </summary>
        /// <param name="lookPosition">Vector3 position to look at.</param>
        public void LookAt(Vector3 lookPosition)
        {
            // Prevent the player object from tilting and looking at the ground.
            Vector3 heightCorrectedPosition = new Vector3(lookPosition.x, transform.position.y, lookPosition.z);
            transform.LookAt(heightCorrectedPosition);
        }
    }
}