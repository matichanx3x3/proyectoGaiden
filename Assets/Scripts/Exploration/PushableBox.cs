using UnityEngine;

namespace Game.Exploration {
    /// <summary>
    /// Componente para cajas u obstáculos empujables por el jugador durante la exploración.
    /// Configura Rigidbody, amortiguación y fricción para permitir puzles de empujar estilo survival horror.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class PushableBox : MonoBehaviour {
        [Header("Propiedades de Empuje")]
        [SerializeField] private float _pushForce = 2.5f;
        [SerializeField] private float _drag = 6f;
        [SerializeField] private float _mass = 20f;
        [SerializeField] private bool _freezeYPosition = true;

        [Header("Audio")]
        [SerializeField] private AudioClip _pushSound;
        private AudioSource _audioSource;
        private Rigidbody _rb;

        public float PushForce => _pushForce;

        public void Configure(float pushForce, float mass = 20f, float drag = 6f) {
            _pushForce = pushForce;
            _mass = mass;
            _drag = drag;
            if (_rb != null) {
                _rb.mass = mass;
                _rb.linearDamping = drag;
            }
        }

        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _rb.mass = _mass;
            _rb.linearDamping = _drag;
            _rb.angularDamping = 10f;

            // Restringir rotaciones para que la caja se mueva recta sin volcar
            RigidbodyConstraints constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            if (_freezeYPosition) {
                constraints |= RigidbodyConstraints.FreezePositionY;
            }
            _rb.constraints = constraints;

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null && _pushSound != null) {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.clip = _pushSound;
                _audioSource.loop = true;
                _audioSource.spatialBlend = 1f;
            }
        }

        private void Update() {
            if (_audioSource == null || _pushSound == null) return;

            bool isMoving = _rb.linearVelocity.sqrMagnitude > 0.05f;
            if (isMoving && !_audioSource.isPlaying) {
                _audioSource.Play();
            } else if (!isMoving && _audioSource.isPlaying) {
                _audioSource.Pause();
            }
        }
    }
}
