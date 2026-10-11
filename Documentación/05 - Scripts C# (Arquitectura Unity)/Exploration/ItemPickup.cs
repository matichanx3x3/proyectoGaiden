using UnityEngine;
using DG.Tweening;
using Game.Interfaces;
using Game.Items;
using Game.Weapons;
using Game.Core;

namespace Game.Exploration {
    public enum PickupType { Item, Ammo }

    /// <summary>
    /// Objeto recolectable en el escenario (hierba, spray, caja de munición).
    /// Se recoge automáticamente al caminar sobre él (Trigger) o al presionar E (IInteractable).
    /// Incluye animación suave de flotación y rotación estilo survival horror.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ItemPickup : MonoBehaviour, IInteractable {
        [Header("Tipo de Recogida")]
        [SerializeField] private PickupType _pickupType = PickupType.Item;
        [SerializeField] private ItemId _itemId = ItemId.GreenHerb;
        [SerializeField] private WeaponType _weaponType = WeaponType.Handgun;
        [SerializeField] private int _ammoAmount = 15;

        [Header("Efecto de Audio")]
        [SerializeField] private AudioClip _pickupSound;

        [Header("Animación")]
        [SerializeField] private bool _enableBobbing = true;
        [SerializeField] private float _rotationSpeed = 45f;
        [SerializeField] private float _bobFrequency = 2.5f;
        [SerializeField] private float _bobHeight = 0.08f;

        private Vector3 _startPos;
        private bool _isCollected;

        public ItemId CurrentItemId => _itemId;

        private void Awake() {
            _startPos = transform.position;
            var col = GetComponent<Collider>();
            if (col != null) {
                col.isTrigger = true;
            }
        }

        public void Configure(ItemId itemId) {
            _pickupType = PickupType.Item;
            _itemId = itemId;
            _startPos = transform.position;
            name = $"Pickup_{itemId}";
        }

        private void Start() {
            if (!_enableBobbing) return;

            // Animación continua de flotación y rotación con DOTween
            float bobDuration = _bobFrequency > 0f ? (2f / _bobFrequency) : 1.2f;
            transform.DOMoveY(_startPos.y + _bobHeight, bobDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            transform.DORotate(new Vector3(0f, 360f, 0f), 360f / _rotationSpeed, RotateMode.LocalAxisAdd)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);
        }

        private void OnTriggerEnter(Collider other) {
            if (_isCollected) return;

            if (other.CompareTag("Player") || other.GetComponent<ExplorationController>() != null) {
                ExecutePickup();
            }
        }

        public void Interact(ExplorationController player) {
            if (_isCollected) return;
            ExecutePickup();
        }

        private void ExecutePickup() {
            var inv = InventorySystem.Instance;
            if (inv == null) return;

            bool success = false;
            if (_pickupType == PickupType.Item) {
                success = inv.AddItem(_itemId);
                if (success) {
                    Debug.Log($"[ItemPickup] Objeto recogido con éxito: {_itemId}");
                } else {
                    Debug.LogWarning("[ItemPickup] Inventario lleno (máximo 8 casillas). No se pudo recoger.");
                }
            } else {
                inv.AddAmmo(_weaponType, _ammoAmount);
                Debug.Log($"[ItemPickup] Munición recogida: +{_ammoAmount} balas de {_weaponType}");
                success = true;
            }

            if (success) {
                _isCollected = true;
                if (_pickupSound != null && AudioSystem.Instance != null) {
                    AudioSystem.Instance.PlaySound(_pickupSound);
                }

                // Desactivar collider inmediatamente para evitar recolección duplicada
                var col = GetComponent<Collider>();
                if (col != null) col.enabled = false;

                // Feedback visual de recogida: impulso hacia arriba y reducción de escala
                transform.DOKill();
                Sequence pickupSeq = DOTween.Sequence();
                pickupSeq.Append(transform.DOScale(transform.localScale * 1.35f, 0.12f).SetEase(Ease.OutQuad));
                pickupSeq.Join(transform.DOMoveY(transform.position.y + 0.35f, 0.2f).SetEase(Ease.OutQuad));
                pickupSeq.Append(transform.DOScale(Vector3.zero, 0.12f).SetEase(Ease.InBack));
                pickupSeq.OnComplete(() => Destroy(gameObject));
            }
        }

        private void OnDestroy() {
            transform.DOKill();
        }

        public string GetInteractionPrompt() {
            return _pickupType == PickupType.Item ? $"Recoger {_itemId}" : $"Recoger {_ammoAmount}x munición de {_weaponType}";
        }
    }
}
