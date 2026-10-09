using UnityEngine;
using Gaiden.Combat;

namespace Gaiden.Units {
    /// <summary>
    /// Zombie o criatura visible en los pasillos durante la fase de exploración.
    /// Inicia el combate al contacto con el jugador.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class EnemyOverworldUnit : UnitBase {
        [SerializeField] private EnemyType _enemyType;
        public EnemyType EnemyType => _enemyType;

        private ScriptableEnemy _data;

        public void Initialize(ScriptableEnemy data) {
            _data = data;
            _enemyType = data.EnemyType;
            SetStats(new UnitStats {
                Health = data.MaxHealth,
                MaxHealth = data.MaxHealth,
                AttackPower = data.AttackPower
            });
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                CombatManager.Instance.StartCombat(_enemyType, this);
            }
        }
    }
}
