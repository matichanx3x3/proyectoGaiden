using System;
using System.Collections.Generic;
using UnityEngine;
using Gaiden.Core;
using Gaiden.Weapons;
using Gaiden.Units;

namespace Gaiden.Items {
    public enum ItemId {
        None = 0,
        GreenHerb = 1,
        RedHerb = 2,
        MixedHerb = 3,
        FirstAidSpray = 4,
        BodyArmor = 5,
        CaptainsKey = 6,
        PassengerKey = 7,
        BoilerKey = 8,
        SecurityCard = 9
    }

    /// <summary>
    /// Gestión de 8 slots de inventario, municiones (tope 99) y consumibles.
    /// </summary>
    public class InventorySystem : Singleton<InventorySystem> {
        public static event Action OnInventoryUpdated;

        private const int MAX_SLOTS = 8;
        [SerializeField] private List<ItemId> _slots = new List<ItemId>(MAX_SLOTS);
        private Dictionary<WeaponType, int> _ammo = new Dictionary<WeaponType, int>();

        public ScriptableWeapon EquippedWeapon { get; private set; }

        protected override void Awake() {
            base.Awake();
            InitDefaults();
        }

        private void InitDefaults() {
            _ammo[WeaponType.Knife] = 999;
            _ammo[WeaponType.Handgun] = 30;
            _ammo[WeaponType.Shotgun] = 8;
            _ammo[WeaponType.GrenadeLauncher] = 0;
            _ammo[WeaponType.RocketLauncher] = 0;

            EquippedWeapon = ResourceSystem.Instance.GetWeapon(WeaponType.Handgun);

            _slots.Clear();
            _slots.Add(ItemId.FirstAidSpray);
            _slots.Add(ItemId.GreenHerb);
        }

        public bool AddItem(ItemId id) {
            if (_slots.Count >= MAX_SLOTS) return false;
            _slots.Add(id);
            OnInventoryUpdated?.Invoke();
            return true;
        }

        public void UseItem(int index) {
            if (index < 0 || index >= _slots.Count) return;
            ItemId item = _slots[index];
            var hero = PartyManager.Instance.ActiveHero;

            switch (item) {
                case ItemId.FirstAidSpray:
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

        public int GetAmmo(WeaponType t) => _ammo.TryGetValue(t, out int count) ? count : 0;

        public void AddAmmo(WeaponType t, int amount) {
            if (!_ammo.ContainsKey(t)) _ammo[t] = 0;
            _ammo[t] = Mathf.Clamp(_ammo[t] + amount, 0, 99);
            OnInventoryUpdated?.Invoke();
        }

        public void ConsumeAmmo(WeaponType t, int amount = 1) {
            if (t == WeaponType.Knife) return;
            if (_ammo.ContainsKey(t)) {
                _ammo[t] = Mathf.Max(0, _ammo[t] - amount);
                OnInventoryUpdated?.Invoke();
            }
        }

        public void CycleNextWeapon() {
            int current = (int)EquippedWeapon.WeaponType;
            for (int i = 1; i <= 5; i++) {
                WeaponType next = (WeaponType)((current + i) % 5);
                if (next == WeaponType.Knife || GetAmmo(next) > 0) {
                    EquippedWeapon = ResourceSystem.Instance.GetWeapon(next);
                    OnInventoryUpdated?.Invoke();
                    return;
                }
            }
        }
    }
}
