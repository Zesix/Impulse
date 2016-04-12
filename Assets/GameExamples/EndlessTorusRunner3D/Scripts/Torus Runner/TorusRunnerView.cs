using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    [RequireComponent(typeof(TorusRunnerModel))]
    public class TorusRunnerView : MonoBehaviour
    {
        #region Properties
        // References
        [SerializeField]
        ParticleSystem shape;
        [SerializeField]
        ParticleSystem trail;
        [SerializeField]
        ParticleSystem burst;
        [SerializeField]
        Transform burstPosition;
        [SerializeField]
        GameObject runnerObject;

        TorusRunnerModel model;

        // Death countdown time.
        [SerializeField]
        float deathCountdownTime = -1f;

        ParticleSystem.EmissionModule shapeEmission;
        ParticleSystem.EmissionModule trailEmission;

        // Death notification.
        public const string TorusRunnerDeathNotification = "TorusRunnerModel.DeathNotification";
        #endregion

        void Awake()
        {
            model = GetComponent<TorusRunnerModel>();
            shapeEmission = shape.emission;
            trailEmission = trail.emission;
        }

        /// <summary>
        /// Show or hide the runner mesh model based on bool argument.
        /// </summary>
        /// <param name="setting">A bool value.</param>
        public void ToggleRunnerObject(bool setting)
        {
            runnerObject.SetActive(setting);
        }

        void OnTriggerEnter (Collider collider)
        {
            // Prevent the player from moving the object after we start death animation.
            model.Movable = false;

            // Begin death animation.
            if (deathCountdownTime < 0f)
            {
                shapeEmission.enabled = false;
                trailEmission.enabled = false;
                burst.transform.position = burstPosition.position;
                burst.Emit(burst.maxParticles);
                deathCountdownTime = burst.startLifetime;
            }
        }

        void Update()
        {
            // Check whether a death countdown is active, and if so, then progress.
            // Once time has run out, restore the particle systems' settings and call Die() method.
            if (deathCountdownTime >= 0f)
            {
                deathCountdownTime -= Time.deltaTime;
                if (deathCountdownTime <= 0f)
                {
                    deathCountdownTime = -1f;
                    shapeEmission.enabled = true;
                    trailEmission.enabled = true;
                    ToggleRunnerObject(false);
                    model.Die();
                    // Post death event.
                    this.PostNotification(TorusRunnerDeathNotification);
                }
            }
        }
    }
}
