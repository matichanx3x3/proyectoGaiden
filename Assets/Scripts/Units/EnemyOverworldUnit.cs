using UnityEngine;
using Game.Combat;

namespace Game.Units {
    /// <summary>
    /// Zombie o criatura visible en los pasillos durante la fase de exploración.
    /// Inicia el combate al entrar en contacto con el jugador (soporta 3D y 2D).
    /// </summary>
    public class EnemyOverworldUnit : UnitBase {
        [SerializeField] private EnemyType _enemyType = EnemyType.ZombieStandard;
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

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                TriggerEncounter();
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                TriggerEncounter();
            }
        }

        private void TriggerEncounter() {
            if (CombatManager.Instance != null) {
                CombatManager.Instance.StartCombat(_enemyType, this);
            }
        }
    }
}
