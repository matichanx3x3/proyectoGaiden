using UnityEngine;

namespace Game.Cameras {
    /// <summary>
    /// Cámara de perspectiva cenital / alta angulación.
    /// Proporciona proyección en perspectiva con inclinación angular (pitch),
    /// seguimiento suavizado y anticipación de movimiento (look-ahead).
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour {
        [Header("Objetivo a Seguir")]
        [SerializeField] private Transform _target;

        [Header("Configuración de Posición y Ángulo")]
        [Tooltip("Altura sobre el objetivo en el eje Y")]
        [SerializeField] private float _height = 8.5f;

        [Tooltip("Distancia hacia atrás en el eje Z")]
        [SerializeField] private float _distance = 5.5f;

        [Tooltip("Inclinación vertical en grados mirando hacia abajo (~50° a 60°)")]
        [Range(30f, 80f)]
        [SerializeField] private float _pitchAngle = 55f;

        [Header("Suavizado y Dinámica")]
        [SerializeField] private float _smoothTime = 0.18f;
        [Tooltip("Anticipación de cámara hacia donde avanza el personaje")]
        [SerializeField] private float _lookAheadFactor = 1.2f;

        private Vector3 _currentVelocity;
        private Camera _cam;
        public Transform Target => _target;

        private void Awake() {
            _cam = GetComponent<Camera>();
            ConfigureCameraDefaults();
        }

        private void OnValidate() {
            _cam = GetComponent<Camera>();
            ConfigureCameraDefaults();
        }

        private void ConfigureCameraDefaults() {
            if (_cam != null) {
                _cam.orthographic = false; // Perspectiva 3D
                if (_cam.fieldOfView < 30f || _cam.fieldOfView > 70f) {
                    _cam.fieldOfView = 48f;
                }
            }
        }

        public void SetTarget(Transform target) {
            _target = target;
        }

        private void LateUpdate() {
            if (_target == null) {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) _target = player.transform;
                else return;
            }

            Vector3 targetPos = _target.position;

            Vector3 lookAhead = Vector3.zero;
            var explorationCtrl = _target.GetComponent<Game.Exploration.ExplorationController>();
            if (explorationCtrl != null && explorationCtrl.IsMoving) {
                lookAhead = explorationCtrl.FacingDirection * _lookAheadFactor;
            }

            Vector3 desiredPosition = new Vector3(
                targetPos.x + lookAhead.x,
                targetPos.y + _height,
                targetPos.z - _distance + lookAhead.z
            );

            if (Application.isPlaying) {
                transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _currentVelocity, _smoothTime);
            } else {
                transform.position = desiredPosition;
            }

            transform.rotation = Quaternion.Euler(_pitchAngle, 0f, 0f);
        }
    }
}
