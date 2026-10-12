using System.Collections;
using UnityEngine;
using DG.Tweening;
using Game.Interfaces;
using Game.Items;
using Game.Core;
using Game.UI;

namespace Game.Exploration {
    /// <summary>
    /// Objeto interactuable que representa una puerta en el escenario.
    /// Soporta apertura en sentido opuesto al jugador (girar en contra), apertura lenta con opción
    /// de empujar/abrir de golpe al presionar de nuevo, cerrojo con llaves, y transición de nivel/zona con fundido a negro.
    /// </summary>
    public class DoorInteractable : MonoBehaviour, IInteractable {
        [Header("Estado")]
        [SerializeField] private bool _isOpen = false;
        [SerializeField] private bool _isLocked = false;
        [SerializeField] private ItemId _requiredKey = ItemId.None;
        [SerializeField] private bool _consumeKeyOnUnlock = false;

        [Header("Tiempos de Animación")]
        [SerializeField] private float _slowOpenDuration = 2.2f;
        [SerializeField] private float _burstOpenDuration = 0.25f;
        [SerializeField] private float _closeDuration = 0.5f;

        [Header("Transform y Rotación")]
        [SerializeField] private Transform _doorPivot;
        [SerializeField] private Vector3 _closedRotation = Vector3.zero;
        [SerializeField] private Vector3 _openRotation = new Vector3(0f, 90f, 0f);

        [Header("Transición de Nivel / Zona")]
        [SerializeField] private bool _isLevelTransition = false;
        [SerializeField] private Vector3 _targetSpawnPosition = new Vector3(0f, 1f, -8f);

        [Header("Audio")]
        [SerializeField] private AudioClip _openSound;
        [SerializeField] private AudioClip _closeSound;
        [SerializeField] private AudioClip _lockedSound;
        [SerializeField] private AudioClip _unlockSound;
        [SerializeField] private AudioClip _burstSound;
        private AudioSource _audioSource;

        private bool _isOpeningSlowly = false;
        private Vector3 _currentTargetOpenRotation;

        public bool IsOpen => _isOpen;
        public bool IsLocked => _isLocked;
        public bool IsOpeningSlowly => _isOpeningSlowly;
        public bool IsLevelTransition => _isLevelTransition;

        public void Configure(bool isLocked, ItemId requiredKey, Vector3 openRotation, bool consumeKey = false, bool isLevelTransition = false, Vector3 targetSpawn = default) {
            _isLocked = isLocked;
            _requiredKey = requiredKey;
            _openRotation = openRotation;
            _consumeKeyOnUnlock = consumeKey;
            _isLevelTransition = isLevelTransition;
            if (targetSpawn != default) _targetSpawnPosition = targetSpawn;
        }

        private void Awake() {
            if (_doorPivot == null) {
                var childHinge = transform.Find("Hinge");
                if (childHinge == null) childHinge = transform.Find("DoorPivot");
                _doorPivot = childHinge != null ? childHinge : transform;
            }

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null) {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.spatialBlend = 1f;
            }

            _doorPivot.localRotation = Quaternion.Euler(_isOpen ? _openRotation : _closedRotation);
            _currentTargetOpenRotation = _openRotation;
        }

        public string GetInteractionPrompt() {
            if (_isLocked) {
                return $"Abrir Puerta (Requiere {GetKeyDisplayName(_requiredKey)})";
            }
            if (_isOpeningSlowly) {
                return "¡Abrir de Golpe! [E]";
            }
            if (_isLevelTransition) {
                return _isOpen ? "Entrar a la Siguiente Zona [E]" : "Abrir Puerta a Siguiente Zona [E]";
            }
            return _isOpen ? "Cerrar Puerta [E]" : "Abrir Puerta [E]";
        }

        public void Interact(ExplorationController player) {
            if (_isLocked) {
                TryUnlock(player);
                return;
            }

            // Si está abriéndose lentamente y el jugador pulsa otra vez: ¡abrir de golpe!
            if (_isOpeningSlowly) {
                BurstOpen();
                return;
            }

            // Si ya está completamente abierta
            if (_isOpen) {
                if (_isLevelTransition && player != null) {
                    ExecuteLevelTransition(player);
                } else {
                    CloseDoor();
                }
                return;
            }

            // Si es transición de nivel, abre hacia la oscuridad y dispara la cinemática de fundido
            if (_isLevelTransition) {
                StartLevelTransitionSequence(player);
                return;
            }

            // Apertura normal: calcular giro en contra del jugador e iniciar apertura lenta
            StartSlowOpen(player);
        }

        private void TryUnlock(ExplorationController player) {
            var inv = InventorySystem.Instance;
            if (inv != null && _requiredKey != ItemId.None && inv.HasItem(_requiredKey)) {
                _isLocked = false;
                if (_consumeKeyOnUnlock) {
                    inv.RemoveItem(_requiredKey);
                    Debug.Log($"[Door] Cerradura abierta. Se consumió {_requiredKey}.");
                } else {
                    Debug.Log($"[Door] Cerradura desbloqueada usando {_requiredKey}.");
                }

                PlaySound(_unlockSound);

                if (_isLevelTransition) {
                    StartLevelTransitionSequence(player);
                } else {
                    StartSlowOpen(player);
                }
            } else {
                Debug.Log($"[Door] La puerta está bloqueada. Necesitas {GetKeyDisplayName(_requiredKey)}.");
                PlaySound(_lockedSound);

                // Feedback visual de forcejeo
                if (Application.isPlaying && _doorPivot != null) {
                    _doorPivot.DOKill();
                    _doorPivot.DOShakeRotation(0.35f, new Vector3(0f, 4f, 0f), 10, 90f).SetEase(Ease.OutQuad);
                }
            }
        }

        /// <summary>
        /// Calcula la rotación para que la puerta gire EN CONTRA del jugador (hacia el lado opuesto a su posición).
        /// </summary>
        private Vector3 CalculateOpenRotationAwayFrom(ExplorationController player) {
            if (_doorPivot == null) {
                var childHinge = transform.Find("Hinge");
                if (childHinge == null) childHinge = transform.Find("DoorPivot");
                _doorPivot = childHinge != null ? childHinge : transform;
            }

            if (player == null) return _openRotation;

            float swingAngle = Mathf.Abs(_openRotation.y != 0f ? _openRotation.y : 90f);

            // Probar ambos sentidos de giro y elegir el que aleja más el batiente del jugador
            Vector3 slabPosPlus = _doorPivot.position + Quaternion.Euler(0f, _closedRotation.y + swingAngle, 0f) * (transform.right * 0.8f);
            Vector3 slabPosMinus = _doorPivot.position + Quaternion.Euler(0f, _closedRotation.y - swingAngle, 0f) * (transform.right * 0.8f);

            float distPlus = (slabPosPlus - player.transform.position).sqrMagnitude;
            float distMinus = (slabPosMinus - player.transform.position).sqrMagnitude;

            float targetY = (distPlus >= distMinus) ? (_closedRotation.y + swingAngle) : (_closedRotation.y - swingAngle);
            return new Vector3(_closedRotation.x, targetY, _closedRotation.z);
        }

        private void StartSlowOpen(ExplorationController player) {
            if (_doorPivot == null) _doorPivot = transform;

            _currentTargetOpenRotation = CalculateOpenRotationAwayFrom(player);
            _isOpeningSlowly = true;
            _isOpen = false;

            PlaySound(_openSound);

            if (Application.isPlaying) {
                _doorPivot.DOKill();
                _doorPivot.DOLocalRotate(_currentTargetOpenRotation, _slowOpenDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => {
                        _isOpeningSlowly = false;
                        _isOpen = true;
                    });
            } else {
                _doorPivot.localRotation = Quaternion.Euler(_currentTargetOpenRotation);
                _isOpen = true;
                _isOpeningSlowly = false;
            }
        }

        private void BurstOpen() {
            if (_doorPivot == null) _doorPivot = transform;

            _isOpeningSlowly = false;
            _isOpen = true;

            if (_burstSound != null) {
                PlaySound(_burstSound);
            } else if (_openSound != null && _audioSource != null) {
                _audioSource.pitch = 1.25f;
                _audioSource.PlayOneShot(_openSound, 1.2f);
                DOVirtual.DelayedCall(0.3f, () => { if (_audioSource != null) _audioSource.pitch = 1f; });
            }

            if (Application.isPlaying) {
                _doorPivot.DOKill();
                _doorPivot.DOLocalRotate(_currentTargetOpenRotation, _burstOpenDuration)
                    .SetEase(Ease.OutBack);

                transform.DOKill();
                transform.DOPunchScale(new Vector3(0.04f, 0.04f, 0.04f), 0.2f, 5, 0.5f);
            } else {
                _doorPivot.localRotation = Quaternion.Euler(_currentTargetOpenRotation);
            }

            Debug.Log("[Door] ¡Puerta abierta de golpe!");
        }

        private void CloseDoor() {
            if (_doorPivot == null) _doorPivot = transform;

            _isOpen = false;
            _isOpeningSlowly = false;
            PlaySound(_closeSound);

            if (Application.isPlaying) {
                _doorPivot.DOKill();
                _doorPivot.DOLocalRotate(_closedRotation, _closeDuration).SetEase(Ease.OutQuad);
            } else {
                _doorPivot.localRotation = Quaternion.Euler(_closedRotation);
            }
        }

        // --- TRANSICIÓN DE NIVEL ESTILO RESIDENT EVIL ---
        private void StartLevelTransitionSequence(ExplorationController player) {
            if (_doorPivot == null) _doorPivot = transform;

            _currentTargetOpenRotation = CalculateOpenRotationAwayFrom(player);
            _isOpen = true;
            _isOpeningSlowly = false;

            PlaySound(_openSound);

            // 1. Abrir hacia la zona oscura
            if (Application.isPlaying) {
                _doorPivot.DOKill();
                _doorPivot.DOLocalRotate(_currentTargetOpenRotation, 1.4f).SetEase(Ease.OutQuad);
            }

            // 2. Fundido a negro de la escena y transporte al spawn
            ExecuteLevelTransition(player);
        }

        private void ExecuteLevelTransition(ExplorationController player) {
            Debug.Log("[Door] Iniciando transición cinematográfica hacia nueva zona...");

            ScreenFader.Instance.FadeOut(0.9f, () => {
                if (player != null) {
                    var cc = player.GetComponent<CharacterController>();
                    if (cc != null) cc.enabled = false;
                    player.transform.position = _targetSpawnPosition;
                    player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    if (cc != null) cc.enabled = true;
                }

                // Cerrar la puerta tras la transición
                if (_doorPivot != null) {
                    _doorPivot.localRotation = Quaternion.Euler(_closedRotation);
                    _isOpen = false;
                }

                DOVirtual.DelayedCall(0.35f, () => {
                    ScreenFader.Instance.FadeIn(0.8f, () => {
                        Debug.Log("[Door] Transición completada. Jugador en nueva zona.");
                    });
                });
            });
        }

        private string GetKeyDisplayName(ItemId key) {
            return key switch {
                ItemId.RedKeyCard => "Llave Roja",
                ItemId.KeyCard_Level1 => "Tarjeta Nivel 1",
                ItemId.KeyCard_Level2 => "Tarjeta Nivel 2",
                ItemId.MaintenanceKey => "Llave de Mantenimiento",
                _ => key.ToString()
            };
        }

        private void PlaySound(AudioClip clip) {
            if (clip != null && _audioSource != null) {
                _audioSource.PlayOneShot(clip);
            }
        }

        private void OnDestroy() {
            if (_doorPivot != null) {
                _doorPivot.DOKill();
            }
            transform.DOKill();
        }
    }
}
