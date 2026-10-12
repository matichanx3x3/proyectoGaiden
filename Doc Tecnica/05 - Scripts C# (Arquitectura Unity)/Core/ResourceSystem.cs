using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Units;
using Game.Weapons;
using Game.Items;

namespace Game.Core {
    /// <summary>
    /// Repositorio de recursos y ScriptableObjects en memoria con acceso O(1) vía diccionarios.
    /// </summary>
    public class ResourceSystem : MonoBehaviour {
        private static ResourceSystem _instance;
        public static ResourceSystem Instance {
            get {
                if (_instance == null) {
                    _instance = UnityEngine.Object.FindFirstObjectByType<ResourceSystem>();
                    if (_instance == null) {
                        var go = new GameObject("ResourceSystem");
                        _instance = go.AddComponent<ResourceSystem>();
                    }
                }
                if (_instance.Enemies == null || _instance.Enemies.Count == 0) {
                    _instance.AssembleResources();
                }
                return _instance;
            }
        }

        public List<ScriptableHero> Heroes { get; private set; } = new List<ScriptableHero>();
        public List<ScriptableEnemy> Enemies { get; private set; } = new List<ScriptableEnemy>();
        public List<ScriptableWeapon> Weapons { get; private set; } = new List<ScriptableWeapon>();
        public List<ScriptableItem> Items { get; private set; } = new List<ScriptableItem>();

        private Dictionary<HeroId, ScriptableHero> _heroesDict = new Dictionary<HeroId, ScriptableHero>();
        private Dictionary<EnemyType, ScriptableEnemy> _enemiesDict = new Dictionary<EnemyType, ScriptableEnemy>();
        private Dictionary<WeaponType, ScriptableWeapon> _weaponsDict = new Dictionary<WeaponType, ScriptableWeapon>();
        private Dictionary<ItemId, ScriptableItem> _itemsDict = new Dictionary<ItemId, ScriptableItem>();

        private void Awake() {
            if (_instance == null) {
                _instance = this;
                if (transform.parent != null) transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
                AssembleResources();
            } else if (_instance != this) {
                Destroy(gameObject);
            }
        }

        public void AssembleResources() {
            Heroes = Resources.LoadAll<ScriptableHero>("Units/Heroes").ToList();
            _heroesDict.Clear();
            foreach (var h in Heroes) {
                if (h != null && !_heroesDict.ContainsKey(h.HeroId)) {
                    _heroesDict[h.HeroId] = h;
                }
            }

            Enemies = Resources.LoadAll<ScriptableEnemy>("Units/Enemies").ToList();
            _enemiesDict.Clear();
            foreach (var e in Enemies) {
                if (e != null && !_enemiesDict.ContainsKey(e.EnemyType)) {
                    _enemiesDict[e.EnemyType] = e;
                }
            }

            Weapons = Resources.LoadAll<ScriptableWeapon>("Weapons").ToList();
            _weaponsDict.Clear();
            foreach (var w in Weapons) {
                if (w != null && !_weaponsDict.ContainsKey(w.WeaponType)) {
                    _weaponsDict[w.WeaponType] = w;
                }
            }

            Items = Resources.LoadAll<ScriptableItem>("Items").ToList();
            _itemsDict.Clear();
            foreach (var i in Items) {
                if (i != null && !_itemsDict.ContainsKey(i.ItemId)) {
                    _itemsDict[i.ItemId] = i;
                }
            }
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
