using UnityEngine;

namespace EndlessTorusRunner3D
{
    [RequireComponent(typeof(TorusRunnerModel))]
    public class TorusRunnerView : MonoBehaviour
    {
        #region Properties
        // References
        [SerializeField] private ParticleSystem _shape;
        [SerializeField] private ParticleSystem _trail;
        [SerializeField] private ParticleSystem _burst;
        [SerializeField] private Transform _burstPosition;
        [SerializeField] private GameObject _runnerObject;

        private TorusRunnerModel _model;

        // Death countdown time.
        [SerializeField] private float _deathCountdownTime = -1f;

        private ParticleSystem.EmissionModule _shapeEmission;
        private ParticleSystem.EmissionModule _trailEmission;

        public TorusRunnerView(ParticleSystem trail)
        {
            _trail = trail;
        }

        // Death notification.
        public const string TorusRunnerDeathNotification = "TorusRunnerModel.DeathNotification";
        #endregion

        private void Awake()
        {
            _model = GetComponent<TorusRunnerModel>();
            _shapeEmission = _shape.emission;
            _trailEmission = _trail.emission;
        }

        /// <summary>
        /// Show or hide the runner mesh model based on bool argument.
        /// </summary>
        /// <param name="setting">A bool value.</param>
        public void ToggleRunnerObject(bool setting)
        {
            _runnerObject.SetActive(setting);
        }

        private void OnTriggerEnter ()
        {
            // Prevent the player from moving the object after we start death animation.
            _model.Movable = false;

            // Begin death animation.
            if (_deathCountdownTime < 0f)
            {
                _shapeEmission.enabled = false;
                _trailEmission.enabled = false;
                _burst.transform.position = _burstPosition.position;
                _burst.Emit(_burst.main.maxParticles);
                _deathCountdownTime = _burst.main.duration;
            }
        }

        private void Update()
        {
            // Check whether a death countdown is active, and if so, then progress.
            // Once time has run out, restore the particle systems' settings and call Die() method.
            if (_deathCountdownTime >= 0f)
            {
                _deathCountdownTime -= Time.deltaTime;
                if (_deathCountdownTime <= 0f)
                {
                    _deathCountdownTime = -1f;
                    _shapeEmission.enabled = true;
                    _trailEmission.enabled = true;
                    ToggleRunnerObject(false);
                    _model.Die();
                    // Post death event.
                    this.PostNotification(TorusRunnerDeathNotification);
                }
            }
        }
    }
}
