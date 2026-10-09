using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Gaiden.Units;
using Gaiden.Weapons;
using Gaiden.Items;

namespace Gaiden.Core {
    /// <summary>
    /// Repositorio de recursos y ScriptableObjects en memoria con acceso O(1) vía diccionarios.
    /// </summary>
    public class ResourceSystem : StaticInstance<ResourceSystem> {
        public List<ScriptableHero> Heroes { get; private set; }
        public List<ScriptableEnemy> Enemies { get; private set; }
        public List<ScriptableWeapon> Weapons { get; private set; }
        public List<ScriptableItem> Items { get; private set; }

        private Dictionary<HeroId, ScriptableHero> _heroesDict;
        private Dictionary<EnemyType, ScriptableEnemy> _enemiesDict;
        private Dictionary<WeaponType, ScriptableWeapon> _weaponsDict;
        private Dictionary<ItemId, ScriptableItem> _itemsDict;

        protected override void Awake() {
            base.Awake();
            AssembleResources();
        }

        private void AssembleResources() {
            Heroes = Resources.LoadAll<ScriptableHero>("Units/Heroes").ToList();
            _heroesDict = Heroes.ToDictionary(h => h.HeroId, h => h);

            Enemies = Resources.LoadAll<ScriptableEnemy>("Units/Enemies").ToList();
            _enemiesDict = Enemies.ToDictionary(e => e.EnemyType, e => e);

            Weapons = Resources.LoadAll<ScriptableWeapon>("Weapons").ToList();
            _weaponsDict = Weapons.ToDictionary(w => w.WeaponType, w => w);

            Items = Resources.LoadAll<ScriptableItem>("Items").ToList();
            _itemsDict = Items.ToDictionary(i => i.ItemId, i => i);
        }

        public ScriptableHero GetHero(HeroId id) => _heroesDict.TryGetValue(id, out var h) ? h : null;
        public ScriptableEnemy GetEnemy(EnemyType type) => _enemiesDict.TryGetValue(type, out var e) ? e : null;
        public ScriptableWeapon GetWeapon(WeaponType type) => _weaponsDict.TryGetValue(type, out var w) ? w : null;
        public ScriptableItem GetItem(ItemId id) => _itemsDict.TryGetValue(id, out var i) ? i : null;

        public ScriptableEnemy GetRandomEnemy() {
            if (Enemies == null || Enemies.Count == 0) return null;
            return Enemies[Random.Range(0, Enemies.Count)];
        }
    }
}
