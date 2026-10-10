using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Weapons;
using Game.Units;

namespace Game.Items {
    /// <summary>
    /// Gestión de 8 slots de inventario, municiones (tope 99) y consumibles.
    /// Soporta uso directo y descarte/soltado de objetos al mundo 3D.
    /// </summary>
    public class InventorySystem : Singleton<InventorySystem> {
        public static event Action OnInventoryUpdated;

        private const int MAX_SLOTS = 8;
        [SerializeField] private List<ItemId> _slots = new List<ItemId>(MAX_SLOTS);
        private Dictionary<WeaponType, int> _ammo = new Dictionary<WeaponType, int>();

        public ScriptableWeapon EquippedWeapon { get; private set; }
        public IReadOnlyList<ItemId> Slots => _slots;

        protected override void Awake() {
            base.Awake();
            InitDefaults();
        }

        private void InitDefaults() {
            _ammo[WeaponType.MeleeKnife] = 999;
            _ammo[WeaponType.Handgun] = 30;
            _ammo[WeaponType.Shotgun] = 8;
            _ammo[WeaponType.Rifle] = 0;
            _ammo[WeaponType.HeavyExplosive] = 0;

            if (ResourceSystem.Instance != null) {
                EquippedWeapon = ResourceSystem.Instance.GetWeapon(WeaponType.Handgun);
            }

            if (_slots.Count == 0) {
                _slots.Add(ItemId.FirstAidMed);
                _slots.Add(ItemId.GreenHerb);
            }
        }

        public bool AddItem(ItemId id) {
            if (_slots.Count >= MAX_SLOTS) return false;
            _slots.Add(id);
            OnInventoryUpdated?.Invoke();
            return true;
        }

        public bool HasItem(ItemId id) => _slots.Contains(id);

        public bool RemoveItem(ItemId id) {
            bool removed = _slots.Remove(id);
            if (removed) OnInventoryUpdated?.Invoke();
            return removed;
        }

        public void UseItem(int index) {
            if (index < 0 || index >= _slots.Count) return;
            ItemId item = _slots[index];
            var hero = PartyManager.Instance != null ? PartyManager.Instance.ActiveHero : null;
            if (hero == null) return;

            switch (item) {
                case ItemId.FirstAidMed:
                    hero.Heal(100);
                    PartyManager.Instance.CurePoison();
                    _slots.RemoveAt(index);
                    break;
                case ItemId.GreenHerb:
                    hero.Heal(30);
                    _slots.RemoveAt(index);
                    break;
                case ItemId.MixedHerb:
                    hero.Heal(120);
                    PartyManager.Instance.CurePoison();
                    _slots.RemoveAt(index);
                    break;
                default:
                    return;
            }
            OnInventoryUpdated?.Invoke();
        }

        public bool DropItem(int index, Vector3 dropPosition) {
            if (index < 0 || index >= _slots.Count) return false;
            ItemId item = _slots[index];
            _slots.RemoveAt(index);

            var pickupGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pickupGo.name = $"Pickup_{item}";
            pickupGo.transform.position = dropPosition + Vector3.up * 0.35f;
            pickupGo.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

            var rend = pickupGo.GetComponent<Renderer>();
            if (rend != null) {
                if (item == ItemId.GreenHerb) rend.material.color = Color.green;
                else if (item == ItemId.RedHerb) rend.material.color = Color.red;
                else if (item == ItemId.FirstAidMed) rend.material.color = Color.cyan;
                else rend.material.color = Color.yellow;
            }

            var col = pickupGo.GetComponent<Collider>();
            if (col != null) col.isTrigger = true;

            var pickup = pickupGo.AddComponent<Game.Exploration.ItemPickup>();
            pickup.Configure(item);

            OnInventoryUpdated?.Invoke();
            Debug.Log($"[Inventory] Objeto soltado al escenario: {item} en {dropPosition}");
            return true;
        }

        public int GetAmmo(WeaponType t) => _ammo.TryGetValue(t, out int count) ? count : 0;

        public void AddAmmo(WeaponType t, int amount) {
            if (!_ammo.ContainsKey(t)) _ammo[t] = 0;
            _ammo[t] = Mathf.Clamp(_ammo[t] + amount, 0, 99);
            OnInventoryUpdated?.Invoke();
        }

        public void ConsumeAmmo(WeaponType t, int amount = 1) {
            if (t == WeaponType.MeleeKnife) return;
            if (_ammo.ContainsKey(t)) {
                _ammo[t] = Mathf.Max(0, _ammo[t] - amount);
                OnInventoryUpdated?.Invoke();
            }
        }

        public void CycleNextWeapon() {
            if (ResourceSystem.Instance == null) return;

            int current = EquippedWeapon != null ? (int)EquippedWeapon.WeaponType : 0;
            for (int i = 1; i <= 5; i++) {
                WeaponType next = (WeaponType)((current + i) % 5);
                if (next == WeaponType.MeleeKnife || GetAmmo(next) > 0) {
                    var w = ResourceSystem.Instance.GetWeapon(next);
                    if (w != null) {
                        EquippedWeapon = w;
                        OnInventoryUpdated?.Invoke();
                        return;
                    }
                }
            }
        }
    }
}
