using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Units;

namespace Game.Managers {
    /// <summary>
    /// Administrador de instanciación y control de unidades presentes en la escena actual.
    /// </summary>
    public class UnitManager : StaticInstance<UnitManager> {
        [Header("Jerarquía de Contenedores")]
        [SerializeField] private Transform _heroesParent;
        [SerializeField] private Transform _enemiesParent;

        public HeroUnit ActivePlayerUnit { get; private set; }
        public List<EnemyOverworldUnit> ActiveEnemies { get; private set; } = new List<EnemyOverworldUnit>();

        public HeroUnit SpawnPlayer(HeroId id, Vector3 position) {
            ScriptableHero data = ResourceSystem.Instance != null ? ResourceSystem.Instance.GetHero(id) : null;

            GameObject go;
            if (data != null && data.Prefab != null) {
                go = Instantiate(data.Prefab, position, Quaternion.identity, _heroesParent);
            } else {
                go = new GameObject($"Player_{id}");
                if (_heroesParent != null) go.transform.SetParent(_heroesParent);
                go.transform.position = position;
            }

            var unit = go.GetComponent<HeroUnit>();
            if (unit == null) unit = go.AddComponent<HeroUnit>();

            if (data != null) unit.Initialize(data);
            ActivePlayerUnit = unit;
            return unit;
        }

        public EnemyOverworldUnit SpawnEnemy(EnemyType type, Vector3 position) {
            ScriptableEnemy data = ResourceSystem.Instance != null ? ResourceSystem.Instance.GetEnemy(type) : null;

            GameObject go;
            if (data != null && data.OverworldPrefab != null) {
                go = Instantiate(data.OverworldPrefab, position, Quaternion.identity, _enemiesParent);
            } else {
                go = new GameObject($"Enemy_{type}_{ActiveEnemies.Count}");
                if (_enemiesParent != null) go.transform.SetParent(_enemiesParent);
                go.transform.position = position;
            }

            var unit = go.GetComponent<EnemyOverworldUnit>();
            if (unit == null) unit = go.AddComponent<EnemyOverworldUnit>();

            if (data != null) unit.Initialize(data);
            ActiveEnemies.Add(unit);
            return unit;
        }

        private void Start() {
            if (ActiveEnemies.Count == 0) {
                var found = FindObjectsByType<EnemyOverworldUnit>(FindObjectsSortMode.None);
                ActiveEnemies.AddRange(found);
            }
        }

        public void RemoveEnemy(EnemyOverworldUnit enemy) {
            if (enemy == null) return;
            if (ActiveEnemies.Contains(enemy)) {
                ActiveEnemies.Remove(enemy);
            }
            Destroy(enemy.gameObject);
        }
    }
}
