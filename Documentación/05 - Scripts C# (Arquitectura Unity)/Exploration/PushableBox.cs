using UnityEngine;
using DG.Tweening;
using Game.Interfaces;

namespace Game.Exploration {
    /// <summary>
    /// Componente para cajas u obstáculos interactivos empujables y arrastrables.
    /// Solo se mueve cuando el jugador interactúa con ella (la agarra con las manos).
    /// El peldaño escalón para trepar encima solo se activa cuando la caja alcanza un punto destino rojo.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class PushableBox : MonoBehaviour, IInteractable {
        [Header("Propiedades de Agarre y Empuje")]
        [SerializeField] private float _pushForce = 2.4f;
        [SerializeField] private float _drag = 8f;
        [SerializeField] private float _mass = 25f;
        [SerializeField] private float _maxGrabDistance = 2.2f;

        [Header("Peldaño Escalón para Trepar")]
        [SerializeField] private GameObject _stepLedge;

        [Header("Límite y Bloqueo de Posición")]
        [SerializeField] private bool _isLockedInPlace = false;
        [SerializeField] private float _maxPushDistance = 0f; // 0 = sin límite por distancia pura, guiado por sockets

        [Header("Audio")]
        [SerializeField] private AudioClip _pushSound;
        [SerializeField] private AudioClip _lockSound;
        [SerializeField] private AudioClip _grabSound;

        private AudioSource _audioSource;
        private Rigidbody _rb;
        private Vector3 _startPosition;
        private bool _isBeingGrabbed;
        private ExplorationController _grabber;
        private BoxDestinationPoint _currentBlueSocket;

        public float PushForce => _pushForce;
        public bool IsLockedInPlace => _isLockedInPlace;
        public bool IsBeingGrabbed => _isBeingGrabbed;

        public void Configure(float pushForce, float mass = 25f, float drag = 8f, float maxDistance = 0f) {
            _pushForce = pushForce;
            _mass = mass;
            _drag = drag;
            _maxPushDistance = maxDistance;
            if (_rb == null) _rb = GetComponent<Rigidbody>();
            if (_rb != null) {
                _rb.mass = mass;
                _rb.linearDamping = drag;
            }
        }

        private void Awake() {
            _startPosition = transform.position;
            _rb = GetComponent<Rigidbody>();
            _rb.mass = _mass;
            _rb.linearDamping = _drag;
            _rb.angularDamping = 10f;

            // Bloquear rotaciones para que no se ladee ni vuelque
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            // Cinemático por defecto para evitar que el jugador la empuje simplemente corriendo contra ella
            _rb.isKinematic = true;

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null && (_pushSound != null || _lockSound != null || _grabSound != null)) {
                _audioSource = gameObject.AddComponent<AudioSource>();
                if (_pushSound != null) _audioSource.clip = _pushSound;
                _audioSource.loop = true;
                _audioSource.spatialBlend = 1f;
            }

            // Localizar peldaño escalón si no está asignado en Inspector
            if (_stepLedge == null) {
                var ledge = transform.Find("Step_Ledge");
                if (ledge != null) _stepLedge = ledge.gameObject;
            }

            // Desactivar peldaño escalón por defecto: solo se podrá trepar cuando alcance el punto rojo final
            if (_stepLedge != null) {
                _stepLedge.SetActive(false);
            }
        }

        // --- IINTERACTABLE ---
        public string GetInteractionPrompt() {
            if (_isLockedInPlace) return ""; // Ya es una plataforma estática
            return _isBeingGrabbed ? "Soltar Caja [E]" : "Agarrar Caja [E]";
        }

        public void Interact(ExplorationController player) {
            if (_isLockedInPlace) return;

            if (_isBeingGrabbed) {
                ReleaseGrab();
            } else {
                StartGrab(player);
            }
        }

        public void StartGrab(ExplorationController player) {
            if (_isLockedInPlace || player == null) return;
            if (_rb == null) _rb = GetComponent<Rigidbody>();

            _isBeingGrabbed = true;
            _grabber = player;
            if (_rb != null) _rb.isKinematic = false;

            player.SetGrabbedBox(this);

            if (_audioSource != null && _grabSound != null) {
                _audioSource.PlayOneShot(_grabSound);
            }

            transform.DOKill();
            transform.DOPunchScale(new Vector3(0.04f, -0.04f, 0.04f), 0.18f, 5, 0.5f);
            Debug.Log($"[PushableBox] {name} agarrada con las manos por el jugador.");
        }

        public void ReleaseGrab() {
            if (!_isBeingGrabbed) return;

            _isBeingGrabbed = false;
            if (_grabber != null) {
                var p = _grabber;
                _grabber = null;
                p.ClearGrabbedBox(this);
            }

            if (_rb != null) {
                if (!_rb.isKinematic) {
                    _rb.linearVelocity = Vector3.zero;
                    _rb.angularVelocity = Vector3.zero;
                }
                if (!_isLockedInPlace) {
                    _rb.isKinematic = true; // Volver sólida / inamovible
                }
            }

            if (_audioSource != null && _audioSource.isPlaying) {
                _audioSource.Pause();
            }

            Debug.Log($"[PushableBox] {name} soltada.");
        }

        public void MoveWithPlayer(Vector3 moveDir, float speed) {
            if (_isLockedInPlace || !_isBeingGrabbed || _rb == null) return;

            _rb.linearVelocity = new Vector3(moveDir.x * speed, 0f, moveDir.z * speed);

            if (_audioSource != null && _pushSound != null) {
                if (!_audioSource.isPlaying) _audioSource.Play();
            }
        }

        public void StopMoving() {
            if (_rb != null && _isBeingGrabbed) {
                _rb.linearVelocity = Vector3.zero;
            }

            if (_audioSource != null && _audioSource.isPlaying) {
                _audioSource.Pause();
            }
        }

        private void FixedUpdate() {
            if (_isLockedInPlace) return;

            // Comprobar distancia máxima de agarre (si el jugador se aleja, se suelta)
            if (_isBeingGrabbed && _grabber != null) {
                float distToPlayer = Vector3.Distance(transform.position, _grabber.transform.position);
                if (distToPlayer > _maxGrabDistance) {
                    ReleaseGrab();
                    return;
                }
            }

            // Comprobación de Puntos Destino (Azules y Rojos)
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.2f);
            bool insideAnyBlue = false;

            foreach (var col in colliders) {
                var dest = col.GetComponentInParent<BoxDestinationPoint>();
                if (dest != null) {
                    float dist2D = Vector3.Distance(
                        new Vector3(transform.position.x, 0f, transform.position.z),
                        new Vector3(dest.transform.position.x, 0f, dest.transform.position.z)
                    );

                    if (dist2D <= dest.DetectionRadius) {
                        if (dest.DestinationType == BoxDestinationType.Blue) {
                            insideAnyBlue = true;
                            if (_currentBlueSocket != dest) {
                                _currentBlueSocket = dest;
                                dest.NotifyBoxEntered(this);
                                Debug.Log($"[PushableBox] {name} posicionada en punto de destino AZUL. Se puede seguir moviendo.");
                            }
                        } else if (dest.DestinationType == BoxDestinationType.Red) {
                            dest.NotifyBoxEntered(this);
                            SnapAndLock(dest.transform.position);
                            return;
                        }
                    }
                }
            }

            if (!insideAnyBlue) {
                _currentBlueSocket = null;
            }

            // Límite por distancia máxima configurada
            if (_maxPushDistance > 0f) {
                float distMoved = Vector3.Distance(
                    new Vector3(transform.position.x, 0f, transform.position.z),
                    new Vector3(_startPosition.x, 0f, _startPosition.z)
                );
                if (distMoved >= _maxPushDistance) {
                    LockInPlace();
                }
            }
        }

        public void SnapAndLock(Vector3 targetPos) {
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            LockInPlace();
        }

        public void LockInPlace() {
            if (_isLockedInPlace) return;
            _isLockedInPlace = true;

            ReleaseGrab();

            if (_rb == null) _rb = GetComponent<Rigidbody>();
            if (_rb != null) {
                if (!_rb.isKinematic) {
                    _rb.linearVelocity = Vector3.zero;
                    _rb.angularVelocity = Vector3.zero;
                }
                _rb.isKinematic = true;
                _rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            if (_audioSource != null) {
                if (_audioSource.isPlaying) _audioSource.Stop();
                if (_lockSound != null) _audioSource.PlayOneShot(_lockSound);
            }

            if (_stepLedge == null) {
                var l = transform.Find("Step_Ledge");
                if (l != null) _stepLedge = l.gameObject;
            }

            // Habilitar peldaño escalón ÚNICAMENTE cuando se alcanza el destino final
            if (_stepLedge != null) {
                _stepLedge.SetActive(true);
                _stepLedge.transform.DOKill();
                _stepLedge.transform.localScale = Vector3.one;
                _stepLedge.transform.DOPunchScale(new Vector3(0.15f, 0.2f, 0.15f), 0.35f, 6, 0.5f);
            }

            if (Application.isPlaying) {
                transform.DOKill();
                transform.DOPunchScale(new Vector3(0.08f, -0.06f, 0.08f), 0.25f, 6, 0.5f);
            }

            Debug.Log($"[PushableBox] {name} bloqueada definitivamente en punto ROJO. Ahora es posible subirse a ella.");
        }

        private void OnDestroy() {
            transform.DOKill();
            if (_stepLedge != null) _stepLedge.transform.DOKill();
        }
    }
}
