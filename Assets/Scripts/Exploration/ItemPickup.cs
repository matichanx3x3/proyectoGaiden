using UnityEngine;
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

        private void Update() {
            if (!_enableBobbing) return;

            transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f, Space.World);

            float newY = _startPos.y + (Mathf.Sin(Time.time * _bobFrequency) * _bobHeight);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
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
                Destroy(gameObject);
            }
        }

        public string GetInteractionPrompt() {
            return _pickupType == PickupType.Item ? $"Recoger {_itemId}" : $"Recoger {_ammoAmount}x munición de {_weaponType}";
        }
    }
}
