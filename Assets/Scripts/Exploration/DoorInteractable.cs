using System.Collections;
using UnityEngine;
using Game.Interfaces;
using Game.Items;
using Game.Core;

namespace Game.Exploration {
    /// <summary>
    /// Objeto interactuable que representa una puerta en el escenario.
    /// Soporta estados Abierta/Cerrada, bloqueo por llave/tarjeta (ItemId)
    /// y animación suave de rotación o desplazamiento.
    /// </summary>
    public class DoorInteractable : MonoBehaviour, IInteractable {
        [Header("Estado")]
        [SerializeField] private bool _isOpen = false;
        [SerializeField] private bool _isLocked = false;
        [SerializeField] private ItemId _requiredKey = ItemId.KeyCard_Level1;
        [SerializeField] private bool _consumeKeyOnUnlock = false;

        [Header("Transform y Rotación")]
        [SerializeField] private Transform _doorPivot;
        [SerializeField] private Vector3 _closedRotation = Vector3.zero;
        [SerializeField] private Vector3 _openRotation = new Vector3(0f, 90f, 0f);
        [SerializeField] private float _animationSpeed = 4f;

        [Header("Audio")]
        [SerializeField] private AudioClip _openSound;
        [SerializeField] private AudioClip _closeSound;
        [SerializeField] private AudioClip _lockedSound;
        [SerializeField] private AudioClip _unlockSound;
        private AudioSource _audioSource;

        private Coroutine _animCoroutine;

        public void Configure(bool isLocked, ItemId requiredKey, Vector3 openRotation, bool consumeKey = false) {
            _isLocked = isLocked;
            _requiredKey = requiredKey;
            _openRotation = openRotation;
            _consumeKeyOnUnlock = consumeKey;
        }

        public bool IsOpen => _isOpen;
        public bool IsLocked => _isLocked;

        private void Awake() {
            if (_doorPivot == null) {
                _doorPivot = transform;
            }

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null && (_openSound != null || _lockedSound != null)) {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.spatialBlend = 1f;
            }

            // Establecer rotación inicial inmediata
            _doorPivot.localRotation = Quaternion.Euler(_isOpen ? _openRotation : _closedRotation);
        }

        public string GetInteractionPrompt() {
            if (_isLocked) {
                return $"Abrir Puerta (Requiere {_requiredKey})";
            }
            return _isOpen ? "Cerrar Puerta" : "Abrir Puerta";
        }

        public void Interact(ExplorationController player) {
            if (_isLocked) {
                TryUnlock();
                return;
            }

            ToggleDoor();
        }

        private void TryUnlock() {
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
                ToggleDoor();
            } else {
                Debug.Log($"[Door] La puerta está bloqueada. Necesitas {_requiredKey}.");
                PlaySound(_lockedSound);
            }
        }

        public void ToggleDoor() {
            if (_doorPivot == null) _doorPivot = transform;
            _isOpen = !_isOpen;
            PlaySound(_isOpen ? _openSound : _closeSound);

            Vector3 targetEuler = _isOpen ? _openRotation : _closedRotation;
            if (Application.isPlaying) {
                if (_animCoroutine != null) StopCoroutine(_animCoroutine);
                _animCoroutine = StartCoroutine(AnimateDoorCoroutine(targetEuler));
            } else {
                _doorPivot.localRotation = Quaternion.Euler(targetEuler);
            }
        }

        private IEnumerator AnimateDoorCoroutine(Vector3 targetEuler) {
            Quaternion targetRot = Quaternion.Euler(targetEuler);
            while (Quaternion.Angle(_doorPivot.localRotation, targetRot) > 0.5f) {
                _doorPivot.localRotation = Quaternion.Slerp(_doorPivot.localRotation, targetRot, Time.deltaTime * _animationSpeed);
                yield return null;
            }
            _doorPivot.localRotation = targetRot;
            _animCoroutine = null;
        }

        private void PlaySound(AudioClip clip) {
            if (clip != null && _audioSource != null) {
                _audioSource.PlayOneShot(clip);
            }
        }
    }
}
