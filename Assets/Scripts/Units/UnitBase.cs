using UnityEngine;
using Game.Interfaces;

namespace Game.Units {
    [System.Serializable]
    public struct UnitStats {
        public int Health;
        public int MaxHealth;
        public int AttackPower;
    }

    /// <summary>
    /// Clase base abstracta para todas las unidades vivientes del juego.
    /// Implementa IDamageable asegurando contratos limpios.
    /// </summary>
    public abstract class UnitBase : MonoBehaviour, IDamageable {
        [SerializeField] protected UnitStats _stats;

        public int CurrentHp => _stats.Health;
        public int MaxHp => _stats.MaxHealth;
        public bool IsDead => _stats.Health <= 0;

        public virtual void SetStats(UnitStats stats) => _stats = stats;

        public virtual void TakeDamage(int amount) {
            _stats.Health = Mathf.Max(0, _stats.Health - amount);
            if (IsDead) Die();
        }

        public virtual void Heal(int amount) {
            _stats.Health = Mathf.Min(_stats.MaxHealth, _stats.Health + amount);
        }

        protected virtual void Die() {
            // Lógica común de muerte
        }
    }
}
