using System.Collections.Generic;
using UnityEngine;
using Gaiden.Core;
using Gaiden.Units;

namespace Gaiden.Managers {
    /// <summary>
    /// Administrador de instanciación y control de unidades presentes en la escena actual.
    /// </summary>
    public class UnitManager : StaticInstance<UnitManager> {
        [Header("Jerarquía")]
        [SerializeField] private Transform _heroesParent;
        [SerializeField] private Transform _enemiesParent;

        public HeroUnit ActivePlayerUnit { get; private set; }
        public List<EnemyOverworldUnit> ActiveEnemies { get; private set; } = new List<EnemyOverworldUnit>();

        public HeroUnit SpawnPlayer(HeroId id, Vector3 position) {
            var data = ResourceSystem.Instance.GetHero(id);
            if (data == null) return null;

            var go = Instantiate(data.Prefab, position, Quaternion.identity, _heroesParent);
            go.name = $"Player_{data.CharacterName}";

            var unit = go.GetComponent<HeroUnit>();
            unit.Initialize(data);
            ActivePlayerUnit = unit;
            return unit;
        }

        public EnemyOverworldUnit SpawnEnemy(EnemyType type, Vector3 position) {
            var data = ResourceSystem.Instance.GetEnemy(type);
            if (data == null) return null;

            var go = Instantiate(data.OverworldPrefab, position, Quaternion.identity, _enemiesParent);
            go.name = $"Enemy_{data.EnemyName}_{ActiveEnemies.Count}";

            var unit = go.GetComponent<EnemyOverworldUnit>();
            unit.Initialize(data);
            ActiveEnemies.Add(unit);
            return unit;
        }

        public void RemoveEnemy(EnemyOverworldUnit enemy) {
            if (ActiveEnemies.Contains(enemy)) {
                ActiveEnemies.Remove(enemy);
                Destroy(enemy.gameObject);
            }
        }
    }
}
